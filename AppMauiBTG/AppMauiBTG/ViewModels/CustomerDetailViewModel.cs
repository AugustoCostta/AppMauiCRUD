using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppMauiBTG.Models;
using AppMauiBTG.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
#if WINDOWS
using Microsoft.Maui.Controls;
using Microsoft.UI.Windowing;
using WinRT.Interop;
#endif

namespace AppMauiBTG.ViewModels
{
    [QueryProperty(nameof(CustomerId), nameof(CustomerId))]
    public partial class CustomerDetailViewModel : ObservableObject
    {
        private readonly ICustomerService _customerService;

        [ObservableProperty]
        Cliente customer;

        [ObservableProperty]
        int customerId;

        public ICommand SaveCustomerCommand { get; }
        public ICommand CancelCommand { get; }
        public MainViewModel MainVm { get; set; }

#if WINDOWS
        public Window Window { get; set; }
#endif

        public CustomerDetailViewModel(ICustomerService customerService)
        {
            _customerService = customerService;
            Customer = new Cliente();
            SaveCustomerCommand = new AsyncRelayCommand(SaveCustomerAsync);
            CancelCommand = new AsyncRelayCommand(CancelAsync);
        }

        private async Task CloseWindowAsync()
        {
        #if WINDOWS
            if (Window?.Handler?.PlatformView is Microsoft.UI.Xaml.Window nativeWin)
            {
                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(nativeWin);
                var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
                var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
                appWindow?.Hide();
            }
        #endif
        }

        partial void OnCustomerIdChanged(int oldValue, int newValue)
        {
            if (newValue != 0)
            {
                LoadCustomer(newValue);
            }
        }

        private async void LoadCustomer(int id)
        {
            Customer = await _customerService.GetCustomerAsync(id);
        }

        private async Task SaveCustomerAsync()
        {
            if (ValidateCustomer())
            {
                await _customerService.SaveCustomerAsync(Customer);
               
                if (MainVm != null)
                    await MainVm.LoadCustomersCommand.ExecuteAsync(null);

                await CloseWindowAsync();
            }
        }

        private async Task CancelAsync()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Confirmar Cancelamento",
                $"Deseja realmente cancelar esta ação?",
                "Sim", "Não");

            if (confirm)
                await CloseWindowAsync();
        }

        private bool ValidateCustomer()
        {
            if (string.IsNullOrWhiteSpace(Customer.Name))
            {
                Application.Current.MainPage.DisplayAlert("Erro de Validação", "O campo Nome é obrigatório.", "OK");
                return false;
            }
            if (string.IsNullOrWhiteSpace(Customer.Lastname))
            {
                Application.Current.MainPage.DisplayAlert("Erro de Validação", "O campo Sobrenome é obrigatório.", "OK");
                return false;
            }
            if (Customer.Age <= 0)
            {
                Application.Current.MainPage.DisplayAlert("Erro de Validação", "A idade deve ser um número positivo.", "OK");
                return false;
            }
            if (string.IsNullOrWhiteSpace(Customer.Address))
            {
                Application.Current.MainPage.DisplayAlert("Erro de Validação", "O campo Endereço é obrigatório.", "OK");
                return false;
            }
            return true;
        }
    }
}

