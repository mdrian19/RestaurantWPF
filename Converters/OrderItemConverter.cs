using System;
using System.Windows.Data;
using RestaurantApp.Models;

namespace RestaurantApp.Converters
{
    public class OrderItemConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] == null || values[1] == null)
                return null!;

            if (!int.TryParse(values[0].ToString(), out int dishId))
                return null!;
            if (!int.TryParse(values[1].ToString(), out int quantity))
                return null!;
            if (!decimal.TryParse(values[2].ToString(),
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out decimal unitPrice))
                return null!;

            return new OrderItem
            {
                DishID = dishId,
                MenuID = null,
                Quantity = quantity > 0 ? quantity : 1,
                UnitPrice = unitPrice,
                ProductName = values[3]?.ToString() ?? ""
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes,
            object parameter, System.Globalization.CultureInfo culture)
                => throw new NotImplementedException();
    }
}
