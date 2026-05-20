using System.Collections.ObjectModel;
using System.Windows.Input;
using RestaurantApp.BusinessLogicLayer;
using RestaurantApp.DataAccessLayer;
using RestaurantApp.Exceptions;
using RestaurantApp.Helpers;
using RestaurantApp.Models;

namespace RestaurantApp.ViewModels
{
    public class ClientOrdersListVM : BasePropertyChanged
    {
        private readonly OrderBLL orderBLL = new();
        private readonly OrderItemDAL itemDAL = new();

        private ObservableCollection<Order> orders = new();
        public ObservableCollection<Order> Orders
        {
            get => orders;
            set
            {
                orders = value;
                NotifyPropertyChanged();
            }
        }

        private Order? selectedOrder;
        public Order SelectedOrder
        {
            get => selectedOrder!;
            set
            {
                selectedOrder = value;
                NotifyPropertyChanged();
                if (selectedOrder != null)
                    LoadItemsForOrder(selectedOrder);
            }
        }

        private ObservableCollection<OrderItem> selectedOrderItems = new();
        public ObservableCollection<OrderItem> SelectedOrderItems
        {
            get => selectedOrderItems;
            set
            {
                selectedOrderItems = value;
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

        public ClientOrdersListVM()
        {
            LoadOrders();
        }

        private void LoadOrders()
        {
            if (SessionManager.CurrentUser == null) return;
            Orders = orderBLL.GetClientOrders(SessionManager.CurrentUser.UserID ?? 0);
        }

        private void LoadItemsForOrder(Order order)
        {
            SelectedOrderItems = itemDAL.GetItemsForOrder(order.OrderID ?? 0);
        }


        private ICommand? _cancelCmd;
        public ICommand CancelCommand => _cancelCmd ??= new RelayCommand<object>(_ => DoCancelOrder());

        private ICommand? _refreshCmd;
        public ICommand RefreshCommand => _refreshCmd ??= new RelayCommand<object>(_ => LoadOrders());

        private void DoCancelOrder()
        {
            try
            {
                if (SelectedOrder == null)
                {
                    ErrorMessage = "Selectati o comanda din lista.";
                    return;
                }
                if (!SelectedOrder.IsActive)
                {
                    ErrorMessage = "Comanda nu mai poate fi anulata (este livrata sau deja anulata).";
                    return;
                }

                orderBLL.CancelOrder(SelectedOrder);
                LoadOrders();
                SuccessMessage = "Comanda a fost anulata cu succes.";
                ErrorMessage = "";
            }
            catch (RestaurantException ex)
            {
                ErrorMessage = ex.Message;
                SuccessMessage = "";
            }
        }
    }
}
