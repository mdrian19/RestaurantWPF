using System.Configuration;
using Microsoft.Data.SqlClient;

namespace RestaurantApp.DataAccessLayer
{
    public static class DALHelper
    {
        public static int EstimatedDeliveryMinutes = 45;

        private static readonly string connectionString =
            ConfigurationManager.ConnectionStrings["RestaurantDB"].ConnectionString;

        public static SqlConnection Connection()
            => new SqlConnection(connectionString);

        public static decimal MenuDiscountPercent
            => decimal.Parse(ConfigurationManager.AppSettings["MenuDiscountPercent"] ?? "0");

        public static decimal LargeOrderThreshold
            => decimal.Parse(ConfigurationManager.AppSettings["LargeOrderThreshold"] ?? "0");

        public static decimal OrderDiscountPercent
            => decimal.Parse(ConfigurationManager.AppSettings["OrderDiscountPercent"] ?? "0");

        public static int FrequentOrderCount 
            => int.Parse(ConfigurationManager.AppSettings["FrequentOrderCount"] ?? "0");

        public static int FrequentOrderDays
            => int.Parse(ConfigurationManager.AppSettings["FrequentOrderDays"] ?? "0");

        public static decimal FreeDeliveryThreshold
            => decimal.Parse(ConfigurationManager.AppSettings["FreeDeliveryThreshold"] ?? "0");

        public static decimal DeliveryCost 
            => decimal.Parse(ConfigurationManager.AppSettings["DeliveryCost"] ?? "0");

        public static decimal LowStockThreshold
            => decimal.Parse(ConfigurationManager.AppSettings["LowStockThreshold"] ?? "0");
    }
}
