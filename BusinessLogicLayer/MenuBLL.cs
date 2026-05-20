using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.IdentityModel.Tokens.Experimental;
using RestaurantApp.DataAccessLayer;
using RestaurantApp.Exceptions;
using RestaurantApp.Models;

namespace RestaurantApp.BusinessLogicLayer
{
    public class MenuBLL
    {
        private readonly MenuDAL menuDAL = new();
        private readonly DishDAL dishDAL = new();
        public ObservableCollection<Menu> MenuList { get; set; } = new();

        private decimal MenuDiscountPercent => DALHelper.MenuDiscountPercent;

        public ObservableCollection<Menu> GetAll()
        {
            MenuList = menuDAL.GetAllMenus();
            foreach (var menu in MenuList)
                menu.Dishes = [.. dishDAL.GetDishesForMenu(menu.MenuID ?? 0)];
            return MenuList;
        }

        public decimal GetMenuPrice(Menu menu)
        {
            if (menu == null)
                throw new RestaurantException("Selecteaza un meniu din lista.");
            return menu.CalculatePrice(MenuDiscountPercent);
        }

        public void Add(Menu menu)
        {
            Validate(menu);
            menuDAL.AddMenu(menu);
            foreach (var dish in menu.Dishes)
                dishDAL.AddDishToMenu(menu.MenuID ?? 0, dish.DishID ?? 0);
            MenuList?.Add(menu);
        }

        public void Update(Menu menu)
        {
            if (menu == null)
                throw new RestaurantException("Selecteaza un meniu din lista.");
            Validate(menu);
            menuDAL.UpdateMenu(menu);
            menuDAL.RemoveAllDishesFromMenu(menu.MenuID ?? 0);
            foreach (var dish in menu.Dishes)
                dishDAL.AddDishToMenu(menu.MenuID ?? 0, dish.DishID ?? 0);
        }

        public void Delete(Menu menu)
        {
            if (menu == null)
                throw new RestaurantException("Selecteaza un meniu din lista.");
            menuDAL.RemoveAllDishesFromMenu(menu.MenuID ?? 0);
            menuDAL.DeleteMenu(menu);
            MenuList?.Remove(menu);
        }

        private void Validate(Menu menu)
        {
            if (string.IsNullOrWhiteSpace(menu.Name))
                throw new RestaurantException("Numele meniului nu poate fi gol.");
            if (menu.CategoryID == null || menu.CategoryID <= 0)
                throw new RestaurantException(
                    "Selecteaza o categorie pentru meniu.");
            if (menu.Dishes == null || menu.Dishes.Count == 0)
                throw new RestaurantException(
                    "Meniul trebuie sa contina cel putin un preparat.");
        }
    }
}
