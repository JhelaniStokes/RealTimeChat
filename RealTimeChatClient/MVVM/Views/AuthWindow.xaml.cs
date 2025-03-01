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
using System.Windows.Shapes;

namespace RealTimeChatClient.MVVM.Views
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthViewModel ViewModel { get; set; } = new AuthViewModel();
        public AuthWindow()
        {
            
            ViewModel.CloseAction = new Action(() => this.Close());
            DataContext = ViewModel;
            InitializeComponent();
            
        }
    }
}
