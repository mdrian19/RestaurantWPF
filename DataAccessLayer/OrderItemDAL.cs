using System.Collections.ObjectModel;
using System.Data;
using Microsoft.Data.SqlClient;
using RestaurantApp.Models;

namespace RestaurantApp.DataAccessLayer
{
    public class OrderItemDAL
    {
        public void AddOrderItem(OrderItem item)
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("AddOrderItem", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@orderId", item.OrderID);
            cmd.Parameters.AddWithValue("@dishID", (object?)item.DishID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@menuID", (object?)item.MenuID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@quantity", item.Quantity);
            cmd.Parameters.AddWithValue("@unitPrice", item.UnitPrice);
            var outParam = new SqlParameter("@itemId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outParam);
            con.Open();
            cmd.ExecuteNonQuery();
            item.OrderItemID = (int)outParam.Value;
        }

        public ObservableCollection<OrderItem> GetItemsForOrder(int orderId)
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("GetOrderItems", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@orderId", orderId);
            var result = new ObservableCollection<OrderItem>();
            con.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string productName;
                int? dishId = null;
                int? menuId = null;

                if (!reader.IsDBNull(2))
                {
                    dishId = reader.GetInt32(2);
                    productName = reader.IsDBNull(3) ? "" : reader.GetString(3);
                }
                else
                {
                    menuId = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4);
                    productName = reader.IsDBNull(5) ? "" : reader.GetString(5);
                }

                result.Add(new OrderItem
                {
                    OrderItemID = reader.GetInt32(0),
                    OrderID = reader.GetInt32(1),
                    DishID = dishId,
                    MenuID = menuId,
                    Quantity = reader.GetInt32(6),
                    UnitPrice = reader.GetDecimal(7),
                    ProductName = productName
                });
            }
            return result;
        }
    }
}
