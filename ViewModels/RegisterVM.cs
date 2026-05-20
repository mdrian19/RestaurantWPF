using System.Windows.Input;
using RestaurantApp.BusinessLogicLayer;
using RestaurantApp.Exceptions;
using RestaurantApp.Helpers;
using RestaurantApp.Models;

namespace RestaurantApp.ViewModels
{
    public class RegisterVM : BasePropertyChanged
    {
        private readonly UserBLL userBLL = new();

        private string firstName = "";
        public string FirstName
        {
            get => firstName;
            set 
            { 
                firstName = value; 
                NotifyPropertyChanged(); 
            }
        }

        private string lastName = "";
        public string LastName
        {
            get => lastName;
            set 
            {
                lastName = value; 
                NotifyPropertyChanged(); 
            }
        }

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

        private string phone = "";
        public string Phone
        {
            get => phone;
            set
            { 
                phone = value; 
                NotifyPropertyChanged(); 
            }
        }

        private string deliveryAddress = "";
        public string DeliveryAddress
        {
            get => deliveryAddress;
            set 
            { 
                deliveryAddress = value;
                NotifyPropertyChanged(); 
            }
        }

        public string Password { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";

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

        private string successMessage = "";
        public string SuccessMessage
        {
            get => successMessage;
            set 
            { 
                successMessage = value; 
                NotifyPropertyChanged(); 
            }
        }

        private bool registerSuccess;
        public bool RegisterSuccess
        {
            get => registerSuccess;
            set 
            { 
                registerSuccess = value; 
                NotifyPropertyChanged(); 
            }
        }


        private ICommand? _registerCmd;
        public ICommand RegisterCommand => _registerCmd ??=
            new RelayCommand<object>(_ => DoRegister());

        private void DoRegister()
        {
            try
            {
                var user = new UserAccount
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    Phone = Phone,
                    DeliveryAddress = DeliveryAddress
                };
                userBLL.Register(user, Password, ConfirmPassword);
                SuccessMessage ="Cont creat cu succes! Te poti autentifica acum.";
                ErrorMessage = "";
                RegisterSuccess = true;
            }
            catch (RestaurantException ex)
            {
                ErrorMessage = ex.Message;
                SuccessMessage = "";
                RegisterSuccess = false;
            }
        }
    }
}
