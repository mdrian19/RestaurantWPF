using RestaurantApp.BusinessLogicLayer;
using RestaurantApp.Exceptions;
using RestaurantApp.Helpers;
using RestaurantApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace RestaurantApp.ViewModels
{
    public class ClientOrderVM : BasePropertyChanged
    {
        private readonly OrderBLL orderBLL = new();
        private readonly MenuBLL menuBLL = new();
        private readonly DishBLL dishBLL = new();

        public ObservableCollection<Dish> AvailableDishes { get; } = new();
        public ObservableCollection<OrderItem> CartItems { get; } = new();

        private Dish? selectedDish;
        public Dish SelectedDish
        {
            get => selectedDish!;
            set
            {
                selectedDish = value;
                NotifyPropertyChanged();
            }
        }

        private int quantity = 1;
        public int Quantity
        {
            get => quantity;
            set
            {
                quantity = value;
                NotifyPropertyChanged();
            }
        }

        private decimal totalCost;
        public decimal TotalCost
        {
            get => totalCost;
            set
            {
                totalCost = value;
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

        private string successMessage = "";
        public string SuccessMessage
        {
            get => successMessage;
            set
            {
                successMessage = value;
                NotifyPropertyChanged();
            }
        }


        public ClientOrderVM()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
                return;
            AvailableDishes = dishBLL.GetAvailable();
        }


        private ICommand _addToCartCmd;
        public ICommand AddToCartCommand =>
            _addToCartCmd ??= new RelayCommand<object>(_ => AddToCart());

        private ICommand _removeFromCartCmd;
        public ICommand RemoveFromCartCommand
            => _removeFromCartCmd ??= new RelayCommand<OrderItem>(RemoveFromCart);

        private ICommand _placeOrderCmd;
        public ICommand PlaceOrderCommand
            => _placeOrderCmd ??= new RelayCommand<object>(_ => PlaceOrder());


        private void AddToCart()
        {
            if (SelectedDish == null)
            {
                ErrorMessage = "Selecteaza un preparat.";
                return;
            }
            if (Quantity <= 0)
            {
                ErrorMessage = "Cantitatea trebuie sa fie > 0.";
                return;
            }

            var existing = null as OrderItem;
            foreach (var item in CartItems)
                if (item.DishID == SelectedDish.DishID)
                {
                    existing = item;
                    break;
                }
            if (existing != null)
                existing.Quantity += Quantity;
            else
            {
                CartItems.Add(new OrderItem
                {
                    DishID = SelectedDish.DishID,
                    Quantity = Quantity,
                    UnitPrice = SelectedDish.Price,
                    ProductName = SelectedDish.Name
                });
            }

            RecalcTotal();
            ErrorMessage = "";
        }

        private void RemoveFromCart(OrderItem item)
        {
            if (item == null) return;
            CartItems.Remove(item);
            RecalcTotal();
        }

        private void RecalcTotal()
        {
            TotalCost = orderBLL.CalculateFoodCost(CartItems);
        }

        private void PlaceOrder()
        {
            try
            {
                var order = new Order
                {
                    UserID = SessionManager.CurrentUser.UserID,
                };
                orderBLL.PlaceOrder(order, CartItems);
                CartItems.Clear();
                TotalCost = 0;
                SuccessMessage = $"Comanda {order.OrderCode} a fost plasata! " +
                                 $"Total: {order.TotalCost:F2} lei";
                ErrorMessage = "";
            }
            catch (RestaurantException ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
