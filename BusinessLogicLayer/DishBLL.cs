using System.Collections.ObjectModel;
using RestaurantApp.DataAccessLayer;
using RestaurantApp.Exceptions;
using RestaurantApp.Models;

namespace RestaurantApp.BusinessLogicLayer
{
    public class DishBLL
    {
        private readonly DishDAL dal = new();
        private readonly AllergenDAL allergenDAL = new();
        public ObservableCollection<Dish> DishList { get; set; } = new();

        public ObservableCollection<Dish> GetAll()
        {
            DishList = dal.GetAllDishes();
            foreach (var dish in DishList)
                dish.Allergens = [.. allergenDAL.GetAllergensForDish(dish.DishID ?? 0)];
            return DishList;
        }

        public ObservableCollection<Dish> GetAvailable()
        {
            var all = GetAll();
            var available = new ObservableCollection<Dish>();
            foreach (var d in all)
                if (d.IsAvailable)
                    available.Add(d);
            return available;
        }

        public ObservableCollection<Dish> Search(
            string searchType, string keyword, bool mustContain)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                throw new RestaurantException(
                    "Cuvantul cheie nu poate fi gol.");

            var results = dal.SearchDishes(searchType, keyword, mustContain);
            foreach (var dish in results)
                dish.Allergens = [.. allergenDAL.GetAllergensForDish(dish.DishID ?? 0)];
            return results;
        }

        public ObservableCollection<Dish> GetLowStock()
        {
            return dal.GetLowStockDishes();
        }

        public void Add(Dish dish)
        {
            Validate(dish);
            dal.AddDish(dish);
            DishList?.Add(dish);
        }

        public void Update(Dish dish)
        {
            if (dish == null)
                throw new RestaurantException(
                    "Selecteaza un preparat din lista.");
            Validate(dish);
            dal.UpdateDish(dish);
        }

        public void Delete(Dish dish)
        {
            if (dish == null)
                throw new RestaurantException(
                    "Selecteaza un preparat din lista.");
            dal.DeleteDish(dish);
            DishList?.Remove(dish);
        }

        private void Validate(Dish dish)
        {
            if (string.IsNullOrWhiteSpace(dish.Name))
                throw new RestaurantException(
                    "Numele preparatului nu poate fi gol.");
            if (dish.CategoryID == null || dish.CategoryID <= 0)
                throw new RestaurantException(
                    "Selecteaza o categorie.");
            if (dish.Price <= 0)
                throw new RestaurantException(
                    "Pretul trebuie sa fie mai mare decat 0.");
            if (dish.PortionQuantity <= 0)
                throw new RestaurantException(
                    "Cantitatea per portie trebuie sa fie mai mare decat 0.");
            if (dish.TotalQuantity < 0)
                throw new RestaurantException(
                    "Cantitatea totala nu poate fi negativa.");
        }
    }
}
