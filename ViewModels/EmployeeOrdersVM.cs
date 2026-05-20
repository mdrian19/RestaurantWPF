using RestaurantApp.BusinessLogicLayer;
using RestaurantApp.Exceptions;
using RestaurantApp.Helpers;
using RestaurantApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace RestaurantApp.ViewModels
{
    public class EmployeeOrdersVM : BasePropertyChanged
    {
        private readonly OrderBLL orderBLL = new();
        public ObservableCollection<Order> Orders { get; set; }

        private Order? selectedOrder;
        public Order SelectedOrder
        {
            get => selectedOrder!;
            set
            {
                selectedOrder = value;
                NotifyPropertyChanged();
            }
        }

        public string[] Statuses { get; } = {
            "Se pregateste", "A plecat la client", "Livrata", "Anulata"
        };

        private string selectedStatus = "Se pregateste";
        public string SelectedStatus
        {
            get => selectedStatus;
            set
            {
                selectedStatus = value;
                NotifyPropertyChanged();
            }
        }

        private bool showOnlyActive = true;
        public bool ShowOnlyActive
        {
            get => showOnlyActive;
            set
            {
                showOnlyActive = value;
                NotifyPropertyChanged();
                LoadOrders();
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

        public EmployeeOrdersVM()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
                return;

            LoadOrders();
        }

        private void LoadOrders()
        {
            Orders = ShowOnlyActive ? orderBLL.GetAllActiveOrders() : orderBLL.GetAllOrders();
            NotifyPropertyChanged(nameof(Orders));
        }


        private ICommand _changeStatusCmd;
        public ICommand ChangeStatusCommand => _changeStatusCmd ??=
            new RelayCommand<object>(_ => DoChangeStatus());

        private void DoChangeStatus()
        {
            try
            {
                orderBLL.ChangeStatus(SelectedOrder, SelectedStatus);
                LoadOrders();
                ErrorMessage = "";
            }
            catch (RestaurantException ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
