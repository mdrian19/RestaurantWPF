using System.Collections.ObjectModel;
using System.Data;
using Microsoft.Data.SqlClient;
using RestaurantApp.Models;

namespace RestaurantApp.DataAccessLayer
{
    public class AllergenDAL
    {
        public ObservableCollection<Allergen> GetAllAllergens()
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("GetAllAllergens", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            var result = new ObservableCollection<Allergen>();
            con.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Allergen
                {
                    AllergenID = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }
            return result;
        }

        public ObservableCollection<Allergen> GetAllergensForDish(int dishId)
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("GetAllergensForDish", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@dishId", dishId);
            var result = new ObservableCollection<Allergen>();
            con.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Allergen
                {
                    AllergenID = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }
            return result;
        }

        public void AddAllergen(Allergen allergen)
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("AddAllergen", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@name", allergen.Name);
            var outParam = new SqlParameter("@allergenId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outParam);
            con.Open();
            cmd.ExecuteNonQuery();
            allergen.AllergenID = (int)outParam.Value;
        }

        public void UpdateAllergen(Allergen allergen)
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("UpdateAllergen", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@allergenId", allergen.AllergenID);
            cmd.Parameters.AddWithValue("@name", allergen.Name);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteAllergen(Allergen allergen)
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("DeleteAllergen", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@allergenId", allergen.AllergenID);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void AddAllergenToDish(int dishId, int allergenId)
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("AddAllergenToDish", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@dishId", dishId);
            cmd.Parameters.AddWithValue("@allergenId", allergenId);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void RemoveAllergenFromDish(int dishId, int allergenId)
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("RemoveAllergenFromDish", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@dishId", dishId);
            cmd.Parameters.AddWithValue("@allergenId", allergenId);
            con.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
