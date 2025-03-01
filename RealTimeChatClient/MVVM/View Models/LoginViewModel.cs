using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using RealTimeChatClient.Services;
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
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _password;
        private string _errorMessage;
        private bool _showError;
        private bool _loginValid = false;
        private readonly AuthService _authService;
        private readonly AuthViewModel authViewModel;


        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public Action CloseAction { get; set; }

        public LoginViewModel(AuthViewModel authViewModel)
        {
            _authService = AuthService.Instance;
            LoginCommand = new RelayCommand(async () => await Login(), () => LoginValid);
            RegisterCommand = new RelayCommand(Register);
            this.authViewModel = authViewModel;
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
                ValidateLogin();
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                ValidateLogin();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public bool ShowError
        {
            get => _showError;
            set
            {
                _showError = value;
                OnPropertyChanged();
            }
        }

        public bool LoginValid
        {
            get => _loginValid;
            set
            {
                _loginValid = value;
                OnPropertyChanged();
            }
        }


        private void ValidateLogin()
        {
            LoginValid = !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(Username);
            ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
        }

        private async Task Login()
        {
            var response = await _authService.AuthenticateUser(Username, Password);
            if (response)
            {
                authViewModel.Login();
            }
            else
            {
                ShowError = true;
                ErrorMessage = "invalid username or password please try again";
            }

        }

        public async void Register()
        {
            await authViewModel.SelectPage("Register");
        }



        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
