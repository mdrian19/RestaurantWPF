using System.Collections.ObjectModel;
using RestaurantApp.DataAccessLayer;
using RestaurantApp.Exceptions;
using RestaurantApp.Models;

namespace RestaurantApp.BusinessLogicLayer
{
    public class AllergenBLL
    {
        private readonly AllergenDAL dal = new();
        public ObservableCollection<Allergen> AllergenList { get; set; } = new();

        public ObservableCollection<Allergen> GetAll()
        {
            AllergenList = dal.GetAllAllergens();
            return AllergenList;
        }

        public ObservableCollection<Allergen> GetForDish(int dishId)
        {
            return dal.GetAllergensForDish(dishId);
        }

        public void Add(Allergen allergen)
        {
            if (string.IsNullOrWhiteSpace(allergen.Name))
                throw new RestaurantException(
                    "Numele alergenului nu poate fi gol.");
            dal.AddAllergen(allergen);
            AllergenList?.Add(allergen);
        }

        public void Update(Allergen allergen)
        {
            if (allergen == null)
                throw new RestaurantException(
                    "Selecteaza un alergen din lista.");
            if (string.IsNullOrWhiteSpace(allergen.Name))
                throw new RestaurantException(
                    "Numele alergenului nu poate fi gol.");
            dal.UpdateAllergen(allergen);
        }

        public void Delete(Allergen allergen)
        {
            if (allergen == null)
                throw new RestaurantException(
                    "Selecteaza un alergen din lista.");
            dal.DeleteAllergen(allergen);
            AllergenList?.Remove(allergen);
        }

        public void AddAllergenToDish(int dishId, int allergenId)
        {
            dal.AddAllergenToDish(dishId, allergenId);
        }

        public void RemoveAllergenFromDish(int dishId, int allergenId)
        {
            dal.RemoveAllergenFromDish(dishId, allergenId);
        }
    }
}
