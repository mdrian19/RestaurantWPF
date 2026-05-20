using System.Windows.Input;
using RestaurantApp.BusinessLogicLayer;
using RestaurantApp.Exceptions;
using RestaurantApp.Helpers;

namespace RestaurantApp.ViewModels
{
    public class LoginVM : BasePropertyChanged
    {
        private readonly UserBLL userBLL = new();

        private string email = "";
        public string Email
        {
            get => email;
            set
            {
                email = value;
                NotifyPropertyChanged();
            }
        }

        public string Password { get; set; } = "";

        private string errorMessage = "";
        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                NotifyPropertyChanged();
            }
        }

        private bool loginSuccess;
        public bool LoginSuccess
        {
            get => loginSuccess;
            set
            {
                loginSuccess = value;
                NotifyPropertyChanged();
            }
        }

        private ICommand? _loginCmd;
        public ICommand LoginCommand => _loginCmd ??= new RelayCommand<object>(DoLogin);

        private void DoLogin(object? parameter)
        {
            try
            {
                if (parameter is string pwd && !string.IsNullOrWhiteSpace(pwd))
                    Password = pwd;

                var user = userBLL.Login(Email, Password);
                SessionManager.CurrentUser = user;
                LoginSuccess = true;
                ErrorMessage = "";
            }
            catch (RestaurantException ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
