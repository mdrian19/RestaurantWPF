using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using RestaurantApp.BusinessLogicLayer;
using RestaurantApp.Helpers;
using RestaurantApp.Models;

namespace RestaurantApp.ViewModels
{
    public class MenuDisplayVM : BasePropertyChanged
    {
        private readonly DishBLL dishBLL = new();
        private readonly MenuBLL menuBLL = new();

        public Dictionary<string, ObservableCollection<object>> MenuByCategory { get; set; } = new();

        public MenuDisplayVM()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                MenuByCategory = new();
                return;
            }
            LoadMenu();
        }

        private void LoadMenu()
        {
            MenuByCategory = new();
            var dishes = dishBLL.GetAll();
            var menus = menuBLL.GetAll();

            foreach (var dish in dishes)
            {
                if (!MenuByCategory.ContainsKey(dish.CategoryName))
                    MenuByCategory[dish.CategoryName] = new();
                MenuByCategory[dish.CategoryName].Add(dish);
            }

            foreach (var menu in menus)
            {
                if (!MenuByCategory.ContainsKey(menu.Name))
                    MenuByCategory[menu.Name] = new();
                MenuByCategory[menu.Name].Add(menu);
            }

            NotifyPropertyChanged(nameof(MenuByCategory));
        }

        public bool IsEmployee => SessionManager.IsEmployee;
    }
}
