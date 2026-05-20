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
using RestaurantApp.ViewModels;

namespace RestaurantApp.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            pbPassword.PasswordChanged += (s, args) =>
            {
                if (DataContext is LoginVM vm)
                    vm.Password = pbPassword.Password;
            };

            if (DataContext is LoginVM viewModel)
            {
                viewModel.PropertyChanged += (s, args) =>
                {
                    if (args.PropertyName == nameof(LoginVM.LoginSuccess)
                        && viewModel.LoginSuccess)
                        Close();
                };
            }
        }
    }
}
