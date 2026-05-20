using System.Collections.ObjectModel;
using System.Windows.Input;
using RestaurantApp.BusinessLogicLayer;
using RestaurantApp.Exceptions;
using RestaurantApp.Helpers;
using RestaurantApp.Models;

namespace RestaurantApp.ViewModels
{
    public class EmployeeMenuVM : BasePropertyChanged
    {
        private readonly MenuBLL menuBLL = new();
        private readonly CategoryBLL categoryBLL = new();
        private readonly DishBLL dishBLL = new();

        public ObservableCollection<Menu> MenuList
        {
            get => menuBLL.MenuList;
            set => menuBLL.MenuList = value;
        }

        public ObservableCollection<Category> CategoryList
        {
            get => categoryBLL.CategoryList;
            set => categoryBLL.CategoryList = value;
        }

        public ObservableCollection<Dish> AllDishes
        {
            get => dishBLL.DishList;
            set => dishBLL.DishList = value;
        }

        private Menu? selectedMenu;
        public Menu SelectedMenu
        {
            get => selectedMenu!;
            set
            {
                selectedMenu = value;
                NotifyPropertyChanged();
                if (selectedMenu != null)
                    MenuDishes = new ObservableCollection<Dish>(selectedMenu.Dishes);
                else
                    MenuDishes = new();
                NotifyPropertyChanged(nameof(SelectedMenu));
            }
        }

        private ObservableCollection<Dish>? menuDishes;
        public ObservableCollection<Dish> MenuDishes
        {
            get => menuDishes!;
            set
            {
                menuDishes = value;
                NotifyPropertyChanged();
            }
        }

        private Dish? selectedDishToAdd;
        public Dish SelectedDishToAdd
        {
            get => selectedDishToAdd!;
            set
            {
                selectedDishToAdd = value;
                NotifyPropertyChanged();
            }
        }

        public decimal MenuPrice => SelectedMenu != null ? menuBLL.GetMenuPrice(SelectedMenu) : 0;


        private string newMenuName = "";
        public string NewMenuName
        {
            get => newMenuName;
            set
            {
                newMenuName = value;
                NotifyPropertyChanged();
            }
        }

        private Category? selectedCategory;
        public Category SelectedCategory
        {
            get => selectedCategory!;
            set
            {
                selectedCategory = value;
                NotifyPropertyChanged();
            }
        }

        private string errorMessage = "";
        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                NotifyPropertyChanged();
            }
        }


        public EmployeeMenuVM()
        {
            menuBLL.GetAll();
            categoryBLL.GetAll();
            dishBLL.GetAll();
            MenuDishes = new();
            NotifyPropertyChanged(nameof(MenuList));
            NotifyPropertyChanged(nameof(CategoryList));
            NotifyPropertyChanged(nameof(AllDishes));
        }


        private ICommand _addDishToMenuCmd;
        public ICommand AddDishToMenuCommand => _addDishToMenuCmd ??=
            new RelayCommand<object>(_ => DoAddDishToMenu());

        private void DoAddDishToMenu()
        {
            if (SelectedDishToAdd == null)
            {
                ErrorMessage = "Selecteaza un preparat de adaugat.";
                return;
            }
            foreach (var d in MenuDishes)
                if (d.DishID == SelectedDishToAdd.DishID)
                {
                    ErrorMessage = "Preparatul este deja adaugat in meniu.";
                    return;
                }
            MenuDishes.Add(SelectedDishToAdd);
            NotifyPropertyChanged(nameof(MenuPrice));
            ErrorMessage = "";
        }


        private ICommand _removeDishFromMenuCmd;
        public ICommand RemoveDishFromMenuCommand => _removeDishFromMenuCmd ??=
                new RelayCommand<Dish>(DoRemoveDishFromMenu);

        private void DoRemoveDishFromMenu(Dish dish)
        {
            if (dish == null) return;
            MenuDishes.Remove(dish);
            NotifyPropertyChanged(nameof(MenuPrice));
        }


        private ICommand _addMenuCmd;
        public ICommand AddMenuCommand => _addMenuCmd ??=
            new RelayCommand<object>(_ => DoAddMenu());

        private void DoAddMenu()
        {
            try
            {
                var menu = new Menu
                {
                    Name = NewMenuName,
                    CategoryID = SelectedCategory?.CategoryID,
                    Dishes = new System.Collections.Generic.List<Dish>(MenuDishes)
                };
                menuBLL.Add(menu);
                NewMenuName = "";
                SelectedCategory = null!;
                MenuDishes = new();
                NotifyPropertyChanged(nameof(MenuList));
                ErrorMessage = "";
            }
            catch (RestaurantException ex)
            {
                ErrorMessage = ex.Message;
            }
        }


        private ICommand _updateMenuCmd;
        public ICommand UpdateMenuCommand => _updateMenuCmd ??=
            new RelayCommand<object>(_ => DoUpdateMenu());

        private void DoUpdateMenu()
        {
            try
            {
                if (SelectedMenu == null)
                {
                    ErrorMessage = "Selecteaza un meniu din lista.";
                    return;
                }
                SelectedMenu.Dishes = new System.Collections.Generic.List<Dish>(MenuDishes);
                menuBLL.Update(SelectedMenu);
                NotifyPropertyChanged(nameof(MenuPrice));
                ErrorMessage = "";
            }
            catch (RestaurantException ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private ICommand _deleteMenuCmd;
        public ICommand DeleteMenuCommand => _deleteMenuCmd ??=
            new RelayCommand<object>(_ => DoDeleteMenu());

        private void DoDeleteMenu()
        {
            try
            {
                menuBLL.Delete(SelectedMenu);
                MenuDishes = new();
                ErrorMessage = "";
                NotifyPropertyChanged(nameof(MenuList));
            }
            catch (RestaurantException ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
