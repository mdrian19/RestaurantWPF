using System;
using System.Collections.ObjectModel;
using System.Linq;
using RestaurantApp.DataAccessLayer;
using RestaurantApp.Exceptions;
using RestaurantApp.Models;

namespace RestaurantApp.BusinessLogicLayer
{
    public class OrderBLL
    {
        private readonly OrderDAL orderDAL = new();
        private readonly OrderItemDAL itemDAL = new();

        public decimal CalculateFoodCost(ObservableCollection<OrderItem> items)
            => items.Sum(i => i.LineTotal);

        public decimal CalculateDiscount(decimal foodCost, int userId)
        {
            decimal discountPct = 0;
            if (foodCost >= DALHelper.LargeOrderThreshold)
                discountPct = DALHelper.OrderDiscountPercent;

            var fromDate = DateTime.Now.AddDays(-DALHelper.FrequentOrderDays);

            int recentOrders = orderDAL.GetOrdersForClient(userId)
                .Count(o => o.OrderDate >= fromDate && o.Status == "Livrata");
            if (recentOrders >= DALHelper.FrequentOrderCount)
                discountPct = Math.Max(discountPct, DALHelper.OrderDiscountPercent);

            return discountPct;
        }

        public decimal CalculateDelivery(decimal foodCostAfterDiscount)
            => foodCostAfterDiscount >= DALHelper.FreeDeliveryThreshold ? 0 : DALHelper.DeliveryCost;

        public void PlaceOrder(Order order, ObservableCollection<OrderItem> items)
        {
            if (items == null || items.Count == 0)
                throw new RestaurantException("Comanda nu contine niciun produs.");

            order.FoodCost = CalculateFoodCost(items);
            order.DiscountPercent = CalculateDiscount(order.FoodCost, order.UserID ?? 0);
            decimal afterDiscount = order.FoodCost * (1 - order.DiscountPercent / 100);
            order.DeliveryCost = CalculateDelivery(afterDiscount);
            order.TotalCost = afterDiscount + order.DeliveryCost;
            order.OrderCode = $"ORD-{DateTime.Now:yyyyMMddHHmmss}-{order.UserID}";
            order.EstimatedDelivery = DateTime.Now.AddMinutes(DALHelper.EstimatedDeliveryMinutes);
            orderDAL.AddOrder(order);

            foreach (var item in items)
            {
                item.OrderID = order.OrderID;
                itemDAL.AddOrderItem(item);

                if (item.DishID.HasValue)
                    orderDAL.DecreaseDishStock(item.DishID.Value, item.Quantity);
            }
        }

        public void ChangeStatus(Order order, string newStatus)
        {
            if (order == null)
                throw new RestaurantException("Selecteaza o comanda.");
            if (!order.IsActive)
                throw new RestaurantException("Comanda nu mai poate fi modificata.");

            orderDAL.UpdateOrderStatus(order.OrderID ?? 0, newStatus);
            order.Status = newStatus;
        }

        public void CancelOrder(Order order)
            => ChangeStatus(order, "Anulata");

        public ObservableCollection<Order> GetClientOrders(int userId)
            => orderDAL.GetOrdersForClient(userId);
        public ObservableCollection<Order> GetAllActiveOrders()
            => orderDAL.GetAllActiveOrders();
        public ObservableCollection<Order> GetAllOrders()
            => orderDAL.GetAllOrders();
    }
}
