using System.Windows.Input;
using RestaurantApp.Helpers;
using RestaurantApp.Views;

namespace RestaurantApp.ViewModels
{
    public class MainWindowVM : BasePropertyChanged
    {
        public bool CanOrder => SessionManager.IsClient;
        public bool CanManage => SessionManager.IsEmployee;
        public bool IsLoggedIn => SessionManager.IsLoggedIn;
        public bool IsNotLoggedIn => !SessionManager.IsLoggedIn;

        private ICommand? _openWindowCmd;
        public ICommand OpenWindowCommand => _openWindowCmd ??= new RelayCommand<string>(OpenWindow);

        private ICommand? _logoutCmd;
        public ICommand LogoutCommand => _logoutCmd ??= new RelayCommand<object>(_ =>
                {
                    SessionManager.Logout();
                    RefreshMenuVisibility();
                });

        private void OpenWindow(string windowKey)
        {
            switch(windowKey)
            {
                case "Menu":
                    new MenuDisplayView().ShowDialog();
                    break;
                case "Search":
                    new SearchView().ShowDialog();
                    break;
                case "Login":
                    new LoginView().ShowDialog();
                    RefreshMenuVisibility();
                    break;
                case "Register":
                    new RegisterView().ShowDialog();
                    break;
                case "Order":
                    new ClientOrderView().ShowDialog();
                    break;
                case "MyOrders":
                    new ClientOrdersListView().ShowDialog();
                    break;
                case "EmpOrders":
                    new EmployeeOrdersView().ShowDialog();
                    break;
                case "EmpDishes":
                    new EmployeeDishView().ShowDialog();
                    break;
                case "EmpCats":
                    new EmployeeCategoryView().ShowDialog();
                    break;
                case "EmpLow":
                    new EmployeeLowStockView().ShowDialog();
                    break;
                case "EmpMenus":
                    new EmployeeMenuView().ShowDialog();
                    break;
                case "EmpAllergens":
                    new EmployeeAllergenView().ShowDialog();
                    break;
            }
        }

        private void RefreshMenuVisibility()
        {
            NotifyPropertyChanged(nameof(CanOrder));
            NotifyPropertyChanged(nameof(CanManage));
            NotifyPropertyChanged(nameof(IsLoggedIn));
            NotifyPropertyChanged(nameof(IsNotLoggedIn));
        }
    }
}
