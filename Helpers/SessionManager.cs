using RestaurantApp.Models;

namespace RestaurantApp.Helpers
{
    public static class SessionManager
    {
        public static UserAccount CurrentUser { get; set; } = null!;
        public static bool IsLoggedIn => CurrentUser != null;
        public static bool IsClient => IsLoggedIn && CurrentUser.Role == "Client";
        public static bool IsEmployee => IsLoggedIn && CurrentUser.Role == "Employee";
        
        public static void Logout() => CurrentUser = null!;
    }
}
