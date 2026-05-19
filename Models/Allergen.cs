using System.ComponentModel.DataAnnotations;
using RestaurantApp.Helpers;

namespace RestaurantApp.Models
{
    public class Allergen : BasePropertyChanged
    {
        private int? allergenID;
        public int? AllergenID
        {
            get => allergenID;
            set
            {
                allergenID = value;
                NotifyPropertyChanged();
            }
        }
        private string name = "";
        [MaxLength(100), Required]
        public string Name
        {
            get => name;
            set
            {
                name = value;
                NotifyPropertyChanged();
            }
        }
    }
}
