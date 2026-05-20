using RestaurantApp.BusinessLogicLayer;
using RestaurantApp.Exceptions;
using RestaurantApp.Helpers;
using RestaurantApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace RestaurantApp.ViewModels
{
    public class EmployeeCategoryVM : BasePropertyChanged
    {
        private readonly CategoryBLL categoryBLL = new();

        public ObservableCollection<Category> CategoryList
        {
            get => categoryBLL.CategoryList;
            set => categoryBLL.CategoryList = value;
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

        public EmployeeCategoryVM()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
                return;

            categoryBLL.GetAll();
            NotifyPropertyChanged(nameof(CategoryList));
        }


        private ICommand _addCmd;
        public ICommand AddCommand => _addCmd ??= new RelayCommand<object>(DoAdd);

        private void DoAdd(object obj)
        {
            try
            {
                Category? cat = obj as Category;
                if (cat != null)
                {
                    categoryBLL.Add(cat);
                    ErrorMessage = "";
                }
                else
                {
                    ErrorMessage = "Nu s-a putut adauga categoria.";
                    return;
                }
            }
            catch (RestaurantException ex)
            {
                ErrorMessage = ex.Message;
            }
        }


        private ICommand _updateCmd;
        public ICommand UpdateCommand => _updateCmd ??= new RelayCommand<object>(DoUpdate);

        private void DoUpdate(object obj)
        {
            try
            {
                Category? cat = obj as Category;
                if (cat != null)
                {
                    categoryBLL.Update(cat);
                    ErrorMessage = "";
                }
                else
                {
                    ErrorMessage = "Nu s-a putut actualiza categoria.";
                    return;
                }
            }
            catch (RestaurantException ex)
            {
                ErrorMessage = ex.Message;
            }
        }


        private ICommand _deleteCmd;
        public ICommand DeleteCommand => _deleteCmd ??= new RelayCommand<object>(DoDelete);

        private void DoDelete(object obj)
        {
            try
            {
                Category? cat = obj as Category;
                if (cat != null)
                {
                    categoryBLL.Delete(cat);
                    ErrorMessage = "";
                }
                else
                {
                    ErrorMessage = "Nu s-a putut sterge categoria.";
                    return;
                }
            }
            catch (RestaurantException ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
