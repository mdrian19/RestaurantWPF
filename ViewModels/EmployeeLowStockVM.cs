using RestaurantApp.BusinessLogicLayer;
using RestaurantApp.DataAccessLayer;
using RestaurantApp.Helpers;
using RestaurantApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace RestaurantApp.ViewModels
{
    public class EmployeeLowStockVM : BasePropertyChanged
    {
        private readonly DishBLL dishBLL = new();

        private ObservableCollection<Dish> lowStockDishes = new();
        public ObservableCollection<Dish> LowStockDishes
        {
            get => lowStockDishes;
            set { lowStockDishes = value; NotifyPropertyChanged(); }
        }

        public decimal Threshold => DALHelper.LowStockThreshold;

        public EmployeeLowStockVM()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
                return;
            LoadLowStock();
        }

        private void LoadLowStock()
        {
            LowStockDishes = dishBLL.GetLowStock();
        }


        private ICommand _refreshCmd;
        public ICommand RefreshCommand => _refreshCmd ??= new RelayCommand<object>(_ => LoadLowStock());
    }
}
