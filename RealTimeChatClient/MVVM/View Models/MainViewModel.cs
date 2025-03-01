using GalaSoft.MvvmLight.Command;
using RealTimeChatClient.MVVM.Models;
using RealTimeChatClient.MVVM.Views;
using RealTimeChatClient.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Input;


namespace RealTimeChatClient.MVVM.View_Models
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly GroupServices groupServices;
        private GroupModel _selectedItem;
        private string currentGroupName;
        private bool createTextBoxVisible = false;
        private readonly SignalRClient client;

        public ObservableCollection<MessageModel> Messages { get; set; }
        public ObservableCollection<GroupModel> Groups { get; set; }

        public ICommand LoadCommand { get; }
        public ICommand LoginButtonCommand { get; }
        public ICommand SendCommand { get; }
        public ICommand ToggleCreateTextBoxVisible { get; }
        public ICommand CreateGroupCommand { get; }


        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            Messages = new ObservableCollection<MessageModel>();
            Groups = new ObservableCollection<GroupModel>();
            LoadCommand = new RelayCommand(async () => await LoadDataAsync());
            groupServices = new GroupServices();
            LoginButtonCommand = new RelayCommand(LoginButton);
            SendCommand = new RelayCommand(async () => await Send());
            ToggleCreateTextBoxVisible = new RelayCommand(CreateBoxToggle);
            CreateGroupCommand = new RelayCommand(async () => await GroupCreate());
            client = new SignalRClient();
            
            client.OnMessageReceived += (user, message) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Messages.Add(message);
                    OnPropertyChanged(nameof(Messages));
                });
            };
            client.GroupAdded += async () =>
            {
                await Application.Current.Dispatcher.InvokeAsync(async () =>
                await LoadDataAsync());
            };
        }
        private string addGroupName;

        public string AddGroupName
        {
            get { return addGroupName; }
            set 
            { 
                addGroupName = value; 
                OnPropertyChanged(); 
            }
        }


        private string message;

        public string Message
        {
            get { return message; }
            set 
            { 
                message = value;
                OnPropertyChanged();
            }
        }


        public async Task LoadDataAsync()
        {
            List<GroupModel> groups = await groupServices.GetGroups();
            Groups.Clear();
            Groups = new ObservableCollection<GroupModel>(groups);
            OnPropertyChanged(nameof(Groups));
        }

        public async Task LoadMessageAsync()
        {
            List<MessageModel> messages = await groupServices.GetMessages(SelectedItem.Id);
            Messages = new ObservableCollection<MessageModel>(messages);
            OnPropertyChanged(nameof(Messages));
        }

        public bool CreateTextBoxVisible 
        {
            get => createTextBoxVisible;
            set
            {
                createTextBoxVisible = value;
                OnPropertyChanged();
            } 
        }

        public string CurrentGroupName
        {
            get { return currentGroupName; }
            set
            {
                currentGroupName = value;
                OnPropertyChanged();
            }
        }

        public GroupModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (!Equals(_selectedItem, value))
                {
                    
                    SetSelectedItem(value);
                    //OnPropertyChanged();// Fire and forget async method
                }
                else
                {
                    // ✅ FORCE UPDATE by setting to null first
                    _selectedItem = null;
                    OnPropertyChanged(nameof(SelectedItem));

                    Task.Run(async () =>
                    {
                        await Task.Delay(10); // ✅ Tiny delay allows UI update
                        
                        
                        await SetSelectedItem(value);
                    });
                }
            }
        }
        public async Task SetSelectedItem(GroupModel value)
        {
            if (_selectedItem != value)
            {
                if (SelectedItem != null) await client.LeaveGroup(SelectedItem.Id);
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                await OnSelectedItemChanged();
            }
        }

        public void LoginButton()
        {
            AuthWindow authWindow = new AuthWindow();
            authWindow.ViewModel.MainViewModel = this;
            authWindow.ShowDialog();
        }

        public async Task InitializeClient()
        {
            await client.StartConnectionAsync();
        }

        public async Task Send()
        {
            
            if(SelectedItem != null && !string.IsNullOrEmpty(Message))
            {
                await client.SendMessage(SelectedItem.Id, Message);
                
                Message = "";
            }
        }

        public async Task GroupCreate()
        {
            if (!string.IsNullOrEmpty(AddGroupName))
            {
                await groupServices.CreateGroup(AddGroupName);
                CreateBoxToggle();
            }
        }

        public void CreateBoxToggle()
        {
            CreateTextBoxVisible = !CreateTextBoxVisible;
        }

       
        private async Task OnSelectedItemChanged()
        {
            if (SelectedItem != null)
            {
                Messages.Clear();
                await client.JoinGroup(SelectedItem.Id);
                await LoadMessageAsync();

                CurrentGroupName = SelectedItem.GroupName;
                
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
