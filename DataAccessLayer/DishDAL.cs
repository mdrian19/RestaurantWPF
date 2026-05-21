using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using Microsoft.Data.SqlClient;
using RestaurantApp.Models;

namespace RestaurantApp.DataAccessLayer
{
    public class DishDAL
    {
        public ObservableCollection<Dish> GetAllDishes()
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("GetAllDishes", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            var result = new ObservableCollection<Dish>();
            con.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(MapDish(reader));
            }
            return result;
        }

        public ObservableCollection<Dish> SearchDishes(
            string searchType, string keyword, bool mustContain)
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("SearchDishes", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@searchType", searchType);
            cmd.Parameters.AddWithValue("@keyword", keyword);
            cmd.Parameters.AddWithValue("@mustContain", mustContain ? 1 : 0);
            var result = new ObservableCollection<Dish>();
            con.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(MapDish(reader));
            }
            return result;
        }

        public void AddDish(Dish d)
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("AddDish", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@categoryId", d.CategoryID);
            cmd.Parameters.AddWithValue("@name", d.Name);
            cmd.Parameters.AddWithValue("@price", d.Price);
            cmd.Parameters.AddWithValue("@portionQty", d.PortionQuantity);
            cmd.Parameters.AddWithValue("@portionUnit", d.PortionUnit);
            cmd.Parameters.AddWithValue("@totalQty", d.TotalQuantity);
            cmd.Parameters.AddWithValue("@photoPaths", (object)d.PhotoPaths ?? DBNull.Value);
            var outParam = new SqlParameter("@dishId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outParam);
            con.Open();
            cmd.ExecuteNonQuery();
            d.DishID = (int)outParam.Value;
        }

        public void UpdateDish(Dish d)
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("UpdateDish", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@dishId", d.DishID);
            cmd.Parameters.AddWithValue("@categoryId", d.CategoryID);
            cmd.Parameters.AddWithValue("@name", d.Name);
            cmd.Parameters.AddWithValue("@price", d.Price);
            cmd.Parameters.AddWithValue("@portionQty", d.PortionQuantity);
            cmd.Parameters.AddWithValue("@portionUnit", d.PortionUnit);
            cmd.Parameters.AddWithValue("@totalQty", d.TotalQuantity);
            cmd.Parameters.AddWithValue("@photoPaths", (object)d.PhotoPaths ?? DBNull.Value);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteDish(Dish d)
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("DeleteDish", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@dishId", d.DishID);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public ObservableCollection<Dish> GetLowStockDishes()
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("GetLowStockDishes", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@threshold", DALHelper.LowStockThreshold);
            var result = new ObservableCollection<Dish>();
            con.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(MapDish(reader));
            }
            return result;
        }

        private Dish MapDish(SqlDataReader r)
        {
            return new Dish
            {
                DishID = r.GetInt32(0),
                CategoryID = r.GetInt32(1),
                CategoryName = r.IsDBNull(8) ? "" : r.GetString(8),
                Name = r.GetString(2),
                Price = r.GetDecimal(3),
                PortionQuantity = r.GetDecimal(4),
                PortionUnit = r.GetString(5),
                TotalQuantity = r.GetInt32(6),
                PhotoPaths = r.IsDBNull(7) ? "" : r.GetString(7)
            };
        }


        public List<Dish> GetDishesForMenu(int menuId)
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("GetDishesForMenu", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@menuId", menuId);
            var result = new List<Dish>();
            con.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(MapDish(reader));
            }
            return result;
        }

        public void AddDishToMenu(int menuId, int dishId)
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("AddDishToMenu", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@menuId", menuId);
            cmd.Parameters.AddWithValue("@dishId", dishId);
            con.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
