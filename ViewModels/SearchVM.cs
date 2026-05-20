using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RestaurantApp.BusinessLogicLayer;
using RestaurantApp.Helpers;
using RestaurantApp.Models;

namespace RestaurantApp.ViewModels
{
    public class SearchVM : BasePropertyChanged
    {
        private readonly DishBLL dishBLL = new();

        private string keyword = "";
        public string Keyword
        {
            get => keyword;
            set
            {
                keyword = value;
                NotifyPropertyChanged();
            }
        }

        private string searchType = "Name";
        public string SearchType
        {
            get => searchType;
            set
            {
                searchType = value;
                NotifyPropertyChanged();
            }
        }

        private bool mustContain = true;
        public bool MustContain
        {
            get => mustContain;
            set
            {
                mustContain = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<Dish> results = new();
        public ObservableCollection<Dish> Results
        {
            get => results;
            set
            {
                results = value;
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

        private ICommand? _searchCmd;
        public ICommand SearchCommand => _searchCmd ??= new RelayCommand<object>(_ => DoSearch());

        private void DoSearch()
        {
            if (string.IsNullOrWhiteSpace(Keyword))
            {
                ErrorMessage = "Introdu un cuvant cheie pentru cautare.";
                return;
            }
            ErrorMessage = "";
            Results = dishBLL.Search(SearchType, Keyword, MustContain);
        }
    }
}
