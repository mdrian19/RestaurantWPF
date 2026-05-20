using System;
using System.Windows.Data;
using RestaurantApp.Models;

namespace RestaurantApp.Converters
{
    public class DishConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] == null || values[1] == null)
                return null!;

            if (!int.TryParse(values[0].ToString(), out int categoryId))
                return null!;
            if (!decimal.TryParse(values[2].ToString(),
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out decimal price))
                return null!;
            if (!decimal.TryParse(values[3].ToString(),
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out decimal portionQty))
                return null!;
            if (!decimal.TryParse(values[5].ToString(),
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out decimal totalQty))
                return null!;

            return new Dish
            {
                CategoryID = categoryId,
                Name = values[1].ToString()!,
                Price = price,
                PortionQuantity = portionQty,
                PortionUnit = values[4]?.ToString() ?? "g",
                TotalQuantity = totalQty,
                PhotoPaths = values[6]?.ToString() ?? ""
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes,
            object parameter, System.Globalization.CultureInfo culture)
                => throw new NotImplementedException();
    }
}
