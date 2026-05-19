using System;
using System.ComponentModel.DataAnnotations;
using RestaurantApp.Helpers;

namespace RestaurantApp.Models
{
    public class Order : BasePropertyChanged
    {
        public int? OrderID { get; set; }
        public int? UserID { get; set; }
        public string OrderCode { get; set; } = "";
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = "Inregistrata";
        public decimal FoodCost { get; set; }
        public decimal DeliveryCost { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime? EstimatedDelivery { get; set; }

        public string ClientFirstName { get; set; } = "";
        public string ClientLastName { get; set; } = "";
        public string ClientPhone { get; set; } = "";
        public string ClientAddress { get; set; } = "";

        public List<OrderItem> Items { get; set; } = new();

        public bool IsActive 
            => Status != "Livrata" && Status != "Anulata";
    }
}
