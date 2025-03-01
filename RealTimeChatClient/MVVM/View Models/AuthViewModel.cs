using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using RealTimeChatClient.MVVM.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RealTimeChatClient.MVVM.View_Models
{
    public class AuthViewModel : INotifyPropertyChanged
    {
        private object _currentView;
        private bool isLoggedIn;
        private  MainViewModel mainViewModel;

        public ICommand NavigatePage { get; }
        public Action CloseAction { get; set; }
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel MainViewModel
        {
            get; set;
        }

        public AuthViewModel()
        {
            CurrentView = new LoginView(this);
            NavigatePage = new RelayCommand<string>(page => _ = SelectPage(page));
            isLoggedIn = false;
        }
        public async Task SelectPage(string pageName)
        {
            switch (pageName)
            {
                
                case "Register":
                    CurrentView = new RegisterView(this);
                    break;
                
                case "Login":
                    CurrentView = new LoginView(this);
                    break;
                
            }
        }

        public async Task Login()
        {
            await MainViewModel.LoadDataAsync();
            await MainViewModel.InitializeClient();
            CloseAction.Invoke();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
