using System.ComponentModel.DataAnnotations;
using RestaurantApp.Helpers;

namespace RestaurantApp.Models
{
    public class Category : BasePropertyChanged
    {
        private int? categoryID;
        public int? CategoryID
        {
            get => categoryID;
            set
            {
                categoryID = value;
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
