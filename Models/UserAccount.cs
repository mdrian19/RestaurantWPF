using System.ComponentModel.DataAnnotations;
using RestaurantApp.Helpers;

namespace RestaurantApp.Models
{
    public class UserAccount : BasePropertyChanged
    {
        public int? UserID { get; set; }

        private string firstName = "";
        [MaxLength(100), Required]
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
        [MaxLength(100), Required]
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
        [MaxLength(200), Required]
        public string Email
        {
            get => email;
            set
            {
                email = value;
                NotifyPropertyChanged();
            }
        }

        public string Phone { get; set; } = "";
        public string DeliveryAddress { get; set; } = "";
        public string PasswordHash { get; set; } = "";

        public string Role { get; set; } = "Client";
        public string FullName => $"{FirstName} {LastName}";
    }
}
