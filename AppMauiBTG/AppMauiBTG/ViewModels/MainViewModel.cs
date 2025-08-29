using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppMauiBTG.Models;
using AppMauiBTG.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using AppMauiBTG.Helpers;
using AppMauiBTG.Views;

namespace AppMauiBTG.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ICustomerService _customerService;

        [ObservableProperty]
        ObservableCollection<Cliente> customers;

        [ObservableProperty]
        Cliente selectedCustomer;

        public IAsyncRelayCommand LoadCustomersCommand { get; }
        public IAsyncRelayCommand AddCustomerCommand { get; }
        public IAsyncRelayCommand<Cliente> EditCustomerCommand { get; }
        public IAsyncRelayCommand<Cliente> DeleteCustomerCommand { get; }

        public MainViewModel(ICustomerService customerService)
        {
            _customerService = customerService;
            customers = new ObservableCollection<Cliente>();

            LoadCustomersCommand = new AsyncRelayCommand(LoadCustomersAsync);
            AddCustomerCommand = new AsyncRelayCommand(AddCustomerAsync);
            EditCustomerCommand = new AsyncRelayCommand<Cliente>(EditCustomerAsync);
            DeleteCustomerCommand = new AsyncRelayCommand<Cliente>(DeleteCustomerAsync);
        }

        private async Task EditCustomerAsync(Cliente customer)
        {
            if (customer == null)
                return;

        #if WINDOWS
            var vm = new CustomerDetailViewModel(_customerService);
            vm.Customer = customer;
            vm.MainVm = this;

        
            var detailPage = new CustomerDetailPage(vm);
            var newWindow = new Window(detailPage)
            {
                Title = $"Edição Cliente - {customer.Name}"
            };
            vm.Window = newWindow;
        
            newWindow.Created += (s, e) =>
            {
                if (newWindow.Handler?.PlatformView is Microsoft.UI.Xaml.Window nativeWin)
                {
                    var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(nativeWin);
                    var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
                    var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
        
                    if (appWindow != null)
                    {
                        var displayArea = Microsoft.UI.Windowing.DisplayArea.GetFromWindowId(
                            windowId,
                            Microsoft.UI.Windowing.DisplayAreaFallback.Primary
                        );
        
                        int centerX = displayArea.WorkArea.Width / 2 - 400;
                        int centerY = displayArea.WorkArea.Height / 2 - 300; 
        
                        appWindow.MoveAndResize(new Windows.Graphics.RectInt32(centerX, centerY, 1020, 610));
                    }
                }
            };
        
            Application.Current.OpenWindow(newWindow);
#else
            await Shell.Current.GoToAsync($"{nameof(CustomerDetailPage)}?CustomerId={customer.Id}");
        #endif
        }


        private async Task DeleteCustomerAsync(Cliente customer)
        {
            if (customer == null)
                return;

            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Confirmar exclusão",
                $"Deseja realmente excluir {customer.Name} {customer.Lastname}?",
                "Sim", "Não");

            if (confirm)
            {
                await _customerService.DeleteCustomerAsync(customer);
                await LoadCustomersAsync();
            }
        }

        private async Task LoadCustomersAsync()
        {
            var customerList = await _customerService.GetCustomersAsync();
            customers.Clear();
            foreach (var customer in customerList)
            {
                customers.Add(customer);
            }
        }

        private async Task AddCustomerAsync()
        {
#if WINDOWS
            var customerVm = new CustomerDetailViewModel(_customerService);
            var window = new Window(new CustomerDetailPage(customerVm));
            customerVm.Window = window;
            customerVm.MainVm = this;
        
            window.Created += (s, e) =>
            {
                if (window.Handler?.PlatformView is Microsoft.UI.Xaml.Window nativeWin)
                {
                    var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(nativeWin);
                    var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
                    var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
        
                    if (appWindow != null)
                    {
                        var displayArea = Microsoft.UI.Windowing.DisplayArea.GetFromWindowId(
                            windowId,
                            Microsoft.UI.Windowing.DisplayAreaFallback.Primary
                        );
        
                        int centerX = displayArea.WorkArea.Width / 2 - 400;
                        int centerY = displayArea.WorkArea.Height / 2 - 300; 
        
                        appWindow.MoveAndResize(new Windows.Graphics.RectInt32(centerX, centerY, 1020, 610));
                    }
                }
            };
        
            Application.Current.OpenWindow(window);
#else
            await Shell.Current.GoToAsync(nameof(CustomerDetailPage));
        #endif
        }
    }
}

