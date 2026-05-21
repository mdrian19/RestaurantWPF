using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using RestaurantApp.Helpers;

namespace RestaurantApp.Models
{
    public class Menu : BasePropertyChanged
    {
        private int? menuID;
        public int? MenuID
        {
            get => menuID;
            set
            {
                menuID = value;
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

        public List<Dish> Dishes { get; set; } = new();

        public decimal BasePrice => Dishes.Sum(d => d.Price);

        public decimal CalculatePrice(decimal discountPercent)
            => BasePrice * (1 - discountPercent / 100);

        public bool IsAvailable => Dishes.All(d => d.IsAvailable);

        public string DisplayDishes =>
            string.Join(", ", Dishes.Select(d =>
                $"{d.Name} - {d.PortionQuantity}{d.PortionUnit}"));

        public string ImagePath =>
            Dishes.FirstOrDefault()?.ImagePath ?? "";
    }
}
