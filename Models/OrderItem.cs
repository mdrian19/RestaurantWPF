using RestaurantApp.Helpers;

namespace RestaurantApp.Models
{
    internal class OrderItem : BasePropertyChanged
    {
        public int? OrderItemID { get; set; }
        public int? OrderID { get; set; }
        public int? DishID { get; set; }
        public int? MenuID { get; set; }

        private int quantity = 1;
        public int Quantity
        {
            get => quantity;
            set
            {
                quantity = value;
                NotifyPropertyChanged();
            }
        }

        public decimal UnitPrice { get; set; }
        public string ProductName { get; set; } = "";
        public decimal LineTotal => Quantity * UnitPrice;
    }
}
