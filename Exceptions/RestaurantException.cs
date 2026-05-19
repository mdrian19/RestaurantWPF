using System;

namespace RestaurantApp.Exceptions
{
    public class RestaurantException : ApplicationException
    {
        public RestaurantException(string message) : base(message)
        {
            // exit peacefully
        }
    }
}
