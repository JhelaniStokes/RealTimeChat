using Microsoft.Extensions.DependencyInjection;
using RealTimeChatClient.MVVM.View_Models;
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
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        //public LoginViewModel ViewModel { get; } = new LoginViewModel();
        public LoginView(AuthViewModel authViewModel)
        {
            LoginViewModel ViewModel = new LoginViewModel(authViewModel);
            DataContext = ViewModel;
            //ViewModel.CloseAction = new Action(() => this.Close());
            InitializeComponent();
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel viewModel)
            {
                viewModel.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}
