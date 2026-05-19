using System.Collections.ObjectModel;
using System.Data;
using Microsoft.Data.SqlClient;
using RestaurantApp.Models;

namespace RestaurantApp.DataAccessLayer
{
    public class OrderDAL
    {
        public void AddOrder(Order order)
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("AddOrder", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@userId", order.UserID);
            cmd.Parameters.AddWithValue("@orderCode", order.OrderCode);
            cmd.Parameters.AddWithValue("@foodCost", order.FoodCost);
            cmd.Parameters.AddWithValue("@deliveryCost", order.DeliveryCost);
            cmd.Parameters.AddWithValue("@discountPercent", order.DiscountPercent);
            cmd.Parameters.AddWithValue("@totalCost", order.TotalCost);
            cmd.Parameters.AddWithValue("@estimatedDelivery",
                order.EstimatedDelivery.HasValue
                ? (object)order.EstimatedDelivery.Value
                : DBNull.Value);
            var outParam = new SqlParameter("@orderId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outParam);
            con.Open();
            cmd.ExecuteNonQuery();
            order.OrderID = (int)outParam.Value;
        }

        public void UpdateOrderStatus(int orderId, string newStatus)
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("UpdateOrderStatus", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@orderId", orderId);
            cmd.Parameters.AddWithValue("@status", newStatus);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public ObservableCollection<Order> GetOrdersForClient(int userId)
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("GetOrdersForClient", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@userId", userId);
            return ReadOrders(cmd, con, false);
        }

        public ObservableCollection<Order> GetAllActiveOrders()
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("GetAllActiveOrders", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            return ReadOrders(cmd, con, true);
        }

        public ObservableCollection<Order> GetAllOrders()
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("GetAllOrders", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            return ReadOrders(cmd, con, true);
        }

        public void DecreaseDishStock(int dishId, decimal qty)
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("DecreaseDishStock", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@dishId", dishId);
            cmd.Parameters.AddWithValue("@quantity", qty);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        private ObservableCollection<Order> ReadOrders(
            SqlCommand cmd, SqlConnection con, bool withClientInfo)
        {
            var result = new ObservableCollection<Order>();
            con.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var order = new Order
                {
                    OrderID = reader.GetInt32(0),
                    OrderCode = reader.GetString(1),
                    OrderDate = reader.GetDateTime(2),
                    Status = reader.GetString(3),
                    FoodCost = reader.GetDecimal(4),
                    DeliveryCost = reader.GetDecimal(5),
                    DiscountPercent = reader.GetDecimal(6),
                    TotalCost = reader.GetDecimal(7),
                    EstimatedDelivery = reader.IsDBNull(8) ? (DateTime?)null : reader.GetDateTime(8)

                };
                if (withClientInfo && reader.FieldCount > 9)
                {
                    order.ClientFirstName = reader.GetString(9);
                    order.ClientLastName = reader.GetString(10);
                    order.ClientPhone = reader.IsDBNull(11) ? "" : reader.GetString(11);
                    order.ClientAddress = reader.IsDBNull(12) ? "" : reader.GetString(12);
                }
                result.Add(order);
            }
            return result;
        }
    }
}
