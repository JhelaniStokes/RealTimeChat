using RealTimeChatClient.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using GalaSoft.MvvmLight.Command;

namespace RealTimeChatClient.MVVM.View_Models
{
    class RegisterViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _password;
        private string _passwordConfirm;
        private string _errorMessage;
        private bool _showError;
        private bool _loginValid = false;
        private readonly AuthService _authService;
        private readonly AuthViewModel authViewModel;

        private string pageTitle;
        public string PageTitle
        {
            get => this.pageTitle;
            set
            {
                this.pageTitle = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand RegisterCommand { get; }
        public ICommand BackCommand { get; }

        public RegisterViewModel(AuthViewModel authViewModel)
        {
            _authService = AuthService.Instance;
            RegisterCommand = new RelayCommand(async () => await Register(), () => LoginValid);
            BackCommand = new RelayCommand(Back);
            PageTitle = "Register";
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
                OnPropertyChanged();
                ValidateLogin();
            }
        }
        public string PasswordConfirm
        {
            get => _passwordConfirm;
            set
            {
                _passwordConfirm = value;
                OnPropertyChanged();
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
            LoginValid = !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(PasswordConfirm) && PasswordConfirm == Password;
            ((RelayCommand)RegisterCommand).RaiseCanExecuteChanged();
        }

        private async Task Register()
        {
            var response = await _authService.RegisterUser(Username, Password);
            if (response)
            {
                await authViewModel.SelectPage("Login");
                MessageBox.Show("Registered");
            }
            else
            {
                ShowError = true;
                ErrorMessage = "username already taken";
            }

        }

        public async void Back()
        {
            await authViewModel.SelectPage("Login");
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
