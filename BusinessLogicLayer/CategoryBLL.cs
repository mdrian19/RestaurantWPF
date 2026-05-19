using System.Collections.ObjectModel;
using RestaurantApp.DataAccessLayer;
using RestaurantApp.Exceptions;
using RestaurantApp.Models;

namespace RestaurantApp.BusinessLogicLayer
{
    public class CategoryBLL
    {
        private readonly CategoryDAL dal = new();
        public ObservableCollection<Category> CategoryList { get; set; }

        public ObservableCollection<Category> GetAll()
        {
            CategoryList = dal.GetAllCategories();
            return CategoryList;
        }

        public void Add(Category cat)
        {
            if (string.IsNullOrWhiteSpace(cat.Name))
                throw new RestaurantException("Numele categoriei nu poate fi gol.");
            dal.AddCategory(cat);
            CategoryList?.Add(cat);
        }

        public void Update(Category cat)
        {
            if (cat == null)
                throw new RestaurantException("Selecteaza o categorie din lista.");
            if (string.IsNullOrWhiteSpace(cat.Name))
                throw new RestaurantException("Numele categoriei nu poate fi gol.");
            dal.UpdateCategory(cat);
        }

        public void Delete(Category cat)
        {
            if (cat == null)
                throw new RestaurantException("Selecteaza o categorie din lista.");
            dal.DeleteCategory(cat);
            CategoryList?.Remove(cat);
        }
    }
}
