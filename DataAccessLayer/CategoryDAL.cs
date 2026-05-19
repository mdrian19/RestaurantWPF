using System.Collections.ObjectModel;
using System.Data;
using Microsoft.Data.SqlClient;
using RestaurantApp.Models;

namespace RestaurantApp.DataAccessLayer
{
    public class CategoryDAL
    {
        public ObservableCollection<Category> GetAllCategories()
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("GetAllCategories", con)
            { 
                CommandType = CommandType.StoredProcedure 
            };
            var result = new ObservableCollection<Category>();
            con.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Category
                {
                    CategoryID = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }
            return result;
        }

        public void AddCategory(Category cat)
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("AddCategory", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@name", cat.Name);
            var outParam = new SqlParameter("@catId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outParam);
            con.Open();
            cmd.ExecuteNonQuery();
            cat.CategoryID = (int)outParam.Value;
        }

        public void UpdateCategory(Category cat)
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("UpdateCategory", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@catId", cat.CategoryID);
            cmd.Parameters.AddWithValue("@name", cat.Name);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteCategory(Category cat)
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("DeleteCategory", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@catId", cat.CategoryID);
            con.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
