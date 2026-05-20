using RestaurantApp.BusinessLogicLayer;
using RestaurantApp.Exceptions;
using RestaurantApp.Helpers;
using RestaurantApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace RestaurantApp.ViewModels
{
    public class EmployeeAllergenVM : BasePropertyChanged
    {
        private readonly AllergenBLL allergenBLL = new();

        public ObservableCollection<Allergen> AllergenList
        {
            get => allergenBLL.AllergenList;
            set => allergenBLL.AllergenList = value;
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

        public EmployeeAllergenVM()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
                return;
            allergenBLL.GetAll();
            NotifyPropertyChanged(nameof(AllergenList));
        }


        private ICommand _addCmd;
        public ICommand AddCommand => _addCmd ??= new RelayCommand<object>(DoAdd);

        private void DoAdd(object obj)
        {
            try
            {
                Allergen? allergen = obj as Allergen;
                if (allergen != null)
                { 
                    allergenBLL.Add(allergen);
                    ErrorMessage = "";
                }
                else
                {                
                    ErrorMessage = "Nu s-a putut adauga alergenul.";
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
                Allergen? allergen = obj as Allergen;
                if (allergen != null)
                {
                    allergenBLL.Update(allergen);
                    ErrorMessage = "";
                }
                else
                {
                    ErrorMessage = "Nu s-a putut actualiza alergenul.";
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
                Allergen? allergen = obj as Allergen;
                if (allergen != null)
                {
                    allergenBLL.Delete(allergen);
                    ErrorMessage = "";
                }
                else
                {
                    ErrorMessage = "Nu s-a putut sterge alergenul.";
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
