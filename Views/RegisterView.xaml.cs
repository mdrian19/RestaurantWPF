using RestaurantApp.ViewModels;
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

namespace RestaurantApp.Views
{
    /// <summary>
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    public partial class RegisterView : Window
    {
        public RegisterView()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            pbPassword.PasswordChanged += (s, args) =>
            {
                if (DataContext is RegisterVM vm)
                    vm.Password = pbPassword.Password;
            };
            pbConfirm.PasswordChanged += (s, args) =>
            {
                if (DataContext is RegisterVM vm)
                    vm.ConfirmPassword = pbConfirm.Password;
            };

            if (DataContext is RegisterVM viewModel)
            {
                viewModel.PropertyChanged += (s, args) =>
                {
                    if (args.PropertyName == nameof(RegisterVM.RegisterSuccess)
                        && viewModel.RegisterSuccess)
                        this.Close();
                };
            }
        }
    }
}
