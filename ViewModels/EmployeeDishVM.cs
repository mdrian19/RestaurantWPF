using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using RestaurantApp.BusinessLogicLayer;
using RestaurantApp.Exceptions;
using RestaurantApp.Helpers;
using RestaurantApp.Models;

namespace RestaurantApp.ViewModels
{
    public class EmployeeDishVM : BasePropertyChanged
    {
        private readonly DishBLL dishBLL = new();
        private readonly CategoryBLL categoryBLL = new();
        private readonly AllergenBLL allergenBLL = new();

        public ObservableCollection<Dish> DishList
        {
            get => dishBLL.DishList;
            set => dishBLL.DishList = value;
        }

        public ObservableCollection<Category> CategoryList
        {
            get => categoryBLL.CategoryList;
            set => categoryBLL.CategoryList = value;
        }

        public ObservableCollection<Allergen> AllAllergens
        {
            get => allergenBLL.AllergenList;
            set => allergenBLL.AllergenList = value;
        }

        private ObservableCollection<Allergen> selectedDishAllergens;
        public ObservableCollection<Allergen> SelectedDishAllergens
        {
            get => selectedDishAllergens;
            set { selectedDishAllergens = value; NotifyPropertyChanged(); }
        }

        private Dish selectedDish;
        public Dish SelectedDish
        {
            get => selectedDish;
            set
            {
                selectedDish = value;
                NotifyPropertyChanged();
                if (selectedDish?.DishID != null)
                    SelectedDishAllergens = allergenBLL.GetForDish(selectedDish.DishID ?? 0);
                else
                    SelectedDishAllergens = new();
            }
        }

        private Allergen selectedAllergen;
        public Allergen SelectedAllergen
        {
            get => selectedAllergen;
            set
            {
                selectedAllergen = value;
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

        public EmployeeDishVM()
        {
            dishBLL.GetAll();
            categoryBLL.GetAll();
            allergenBLL.GetAll();
            NotifyPropertyChanged(nameof(DishList));
            NotifyPropertyChanged(nameof(CategoryList));
            NotifyPropertyChanged(nameof(AllAllergens));
        }

        private ICommand _addCmd;
        public ICommand AddCommand => _addCmd ??=
            new RelayCommand<object>(DoAdd);

        private void DoAdd(object obj)
        {
            try
            {
                Dish? dish = obj as Dish;
                if (dish != null)
                {
                    dish.CategoryName = CategoryList
                        .FirstOrDefault(c => c.CategoryID == dish.CategoryID)?.Name ?? "";

                    dishBLL.Add(dish);
                    ErrorMessage = "";
                }
                else
                {
                    ErrorMessage = "Nu s-a putut adauga preparatul.";
                    return;
                }
            }
            catch (RestaurantException ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private ICommand _updateCmd;
        public ICommand UpdateCommand => _updateCmd ??=
            new RelayCommand<object>(DoUpdate);

        private void DoUpdate(object obj)
        {
            try
            {
                Dish? dish = obj as Dish;
                if (dish != null)
                {
                    dish.CategoryName = CategoryList
                        .FirstOrDefault(c => c.CategoryID == dish.CategoryID)?.Name ?? "";

                    dishBLL.Update(dish);
                    ErrorMessage = "";
                }
                else
                {
                    ErrorMessage = "Nu s-a putut actualiza preparatul.";
                    return;
                }
            }
            catch (RestaurantException ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private ICommand _deleteCmd;
        public ICommand DeleteCommand => _deleteCmd ??=
            new RelayCommand<object>(DoDelete);

        private void DoDelete(object obj)
        {
            try
            {
                Dish? dish = obj as Dish;
                if (dish == null)
                {
                    ErrorMessage = "Alegeti un preparat de sters.";
                    return;
                }
                var result = System.Windows.MessageBox.Show($"Esti sigur ca vrei sa stergi preparatul {dish.Name}?",
                    "Confirmare stergere", System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Warning);

                if (result != System.Windows.MessageBoxResult.Yes)
                    return;
        
                dishBLL.Delete(dish);
                ErrorMessage = "";
                SelectedDishAllergens = new();
            }
            catch (RestaurantException ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private ICommand _addAllergenCmd;
        public ICommand AddAllergenCommand => _addAllergenCmd ??=
            new RelayCommand<object>(_ => DoAddAllergen());

        private void DoAddAllergen()
        {
            try
            {
                if (SelectedDish == null)
                {
                    ErrorMessage = "Selecteaza un preparat.";
                    return;
                }
                if (SelectedAllergen == null)
                {
                    ErrorMessage = "Selecteaza un alergen de adaugat.";
                    return;
                }
                allergenBLL.AddAllergenToDish(
                    SelectedDish.DishID ?? 0,
                    SelectedAllergen.AllergenID ?? 0);
                SelectedDishAllergens = allergenBLL.GetForDish(SelectedDish.DishID ?? 0);
                ErrorMessage = "";
            }
            catch (RestaurantException ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private ICommand _removeAllergenCmd;
        public ICommand RemoveAllergenCommand => _removeAllergenCmd ??=
            new RelayCommand<Allergen>(DoRemoveAllergen);

        private void DoRemoveAllergen(Allergen allergen)
        {
            try
            {
                if (SelectedDish == null)
                {
                    ErrorMessage = "Selecteaza un preparat.";
                    return;
                }
                if (allergen == null)
                {
                    ErrorMessage = "Selecteaza un alergen de eliminat.";
                    return;
                }

                allergenBLL.RemoveAllergenFromDish(
                    SelectedDish.DishID ?? 0,
                    allergen.AllergenID ?? 0);
                SelectedDishAllergens = allergenBLL.GetForDish(SelectedDish.DishID ?? 0);
                ErrorMessage = "";
            }
            catch (RestaurantException ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
