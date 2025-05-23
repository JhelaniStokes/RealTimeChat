﻿using RealTimeChatClient.MVVM.View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RealTimeChatClient.MVVM.Views
{
    /// <summary>
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    public partial class RegisterView : UserControl
    {
        public RegisterView(AuthViewModel authViewModel)
        {
            DataContext = new RegisterViewModel(authViewModel);
            InitializeComponent();
        }
        private void PasswordConfirmBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegisterViewModel registerViewModel)
                registerViewModel.Password = ((PasswordBox)sender).Password;
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegisterViewModel registerViewModel)
                registerViewModel.PasswordConfirm = ((PasswordBox)sender).Password;
        }
    }
}
