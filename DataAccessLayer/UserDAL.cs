using System.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
using RestaurantApp.Models;

namespace RestaurantApp.DataAccessLayer
{
    public class UserDAL
    {
        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var sb = new StringBuilder();
            foreach (var b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        public UserAccount Login(string username, string password)
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("LoginUser", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@email", username);
            cmd.Parameters.AddWithValue("@passwordHash", HashPassword(password));
            con.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new UserAccount
                {
                    UserID = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3),
                    Phone = reader.IsDBNull(4) ? "" : reader.GetString(4),
                    DeliveryAddress = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    Role = reader.GetString(6)
                };
            }
            return null;
        }

        public void RegisterUser(UserAccount user)
        {
            using var con = DALHelper.Connection();
            var cmd = new SqlCommand("RegisterUser", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@firstName", user.FirstName);
            cmd.Parameters.AddWithValue("@lastName", user.LastName);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@phone", 
                string.IsNullOrEmpty(user.Phone) ? (object)DBNull.Value : user.Phone);
            cmd.Parameters.AddWithValue("@deliveryAddress", 
                string.IsNullOrEmpty(user.DeliveryAddress) ? (object)DBNull.Value : user.DeliveryAddress);
            cmd.Parameters.AddWithValue("@passwordHash", HashPassword(user.PasswordHash));
            var outParam = new SqlParameter("@userId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outParam);
            con.Open();
            cmd.ExecuteNonQuery();
            user.UserID = (int)outParam.Value;
        }
    }
}
