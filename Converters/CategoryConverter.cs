using System;
using System.Windows.Data;
using RestaurantApp.Models;

namespace RestaurantApp.Converters
{
    public class CategoryConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType,
        object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] != null)
                return new Category { Name = values[0].ToString()! };
            return null!;
        }
        public object[] ConvertBack(object value, Type[] targetTypes,
        object parameter, System.Globalization.CultureInfo culture)
        => throw new NotImplementedException();
    }
}
