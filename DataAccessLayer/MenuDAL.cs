using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using Microsoft.Data.SqlClient;
using RestaurantApp.Models;

namespace RestaurantApp.DataAccessLayer
{
    public class MenuDAL
    {
        public ObservableCollection<Menu> GetAllMenus()
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("GetAllMenus", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            var result = new ObservableCollection<Menu>();
            con.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Menu
                {
                    MenuID = reader.GetInt32(0),
                    CategoryID = reader.GetInt32(1),
                    CategoryName = reader.GetString(2),
                    Name = reader.GetString(3)
                });
            }
            return result;
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
                result.Add(new Dish
                {
                    DishID = reader.GetInt32(0),
                    CategoryID = reader.GetInt32(1),
                    Name = reader.GetString(2),
                    Price = reader.GetDecimal(3),
                    PortionQuantity = reader.GetDecimal(4),
                    PortionUnit = reader.GetString(5),
                    TotalQuantity = reader.GetInt32(6),
                    PhotoPaths = reader.IsDBNull(7) ? "" : reader.GetString(7)
                });
            }
            return result;
        }

        public void AddMenu(Menu menu)
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("AddMenu", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@categoryId", menu.CategoryID);
            cmd.Parameters.AddWithValue("@name", menu.Name);
            var outParam = new SqlParameter("@menuId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outParam);
            con.Open();
            cmd.ExecuteNonQuery();
            menu.MenuID = (int)outParam.Value;
        }

        public void UpdateMenu(Menu menu)
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("UpdateMenu", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@menuId", menu.MenuID);
            cmd.Parameters.AddWithValue("@categoryId", menu.CategoryID);
            cmd.Parameters.AddWithValue("@name", menu.Name);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteMenu(Menu menu)
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("DeleteMenu", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@menuId", menu.MenuID);
            con.Open();
            cmd.ExecuteNonQuery();
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

        public void RemoveAllDishesFromMenu(int menuId)
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("RemoveAllDishesFromMenu", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@menuId", menuId);
            con.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
