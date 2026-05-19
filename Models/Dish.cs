using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RestaurantApp.Helpers;

namespace RestaurantApp.Models
{
    public class Dish : BasePropertyChanged
    {
        private int? dishID;
        public int? DishID
        {
            get => dishID;
            set
            {
                dishID = value;
                NotifyPropertyChanged();
            }
        }

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

        private string categoryName = "";
        public string CategoryName
        {
            get => categoryName;
            set
            {
                categoryName = value;
                NotifyPropertyChanged();
            }
        }

        private string name = "";
        [MaxLength(200), Required]
        public string Name
        {
            get => name;
            set
            {
                name = value;
                NotifyPropertyChanged();
            }
        }

        private decimal price;
        public decimal Price
        {
            get => price;
            set
            {
                price = value;
                NotifyPropertyChanged();
            }
        }

        private decimal portionQuantity;
        public decimal PortionQuantity
        {
            get => portionQuantity;
            set
            {
                portionQuantity = value;
                NotifyPropertyChanged();
            }
        }

        private string portionUnit = "g";
        public string PortionUnit
        {
            get => portionUnit;
            set
            {
                portionUnit = value;
                NotifyPropertyChanged();
            }
        }

        private decimal totalQuantity;
        public decimal TotalQuantity
        {
            get => totalQuantity;
            set
            {
                totalQuantity = value;
                NotifyPropertyChanged();
            }
        }

        private string photoPaths = "";
        public string PhotoPaths
        {
            get => photoPaths;
            set
            {
                photoPaths = value;
                NotifyPropertyChanged();
            }
        }

        public List<Allergen> Allergens { get; set; } = new();

        public bool IsAvailable => TotalQuantity > 0;

        public string DisplayQuantity =>
            $"{PortionQuantity}{PortionUnit}/portie";
    }
}
