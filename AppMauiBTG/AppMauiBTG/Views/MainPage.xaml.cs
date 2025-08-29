using AppMauiBTG.ViewModels;
using Microsoft.Maui.Controls;

namespace AppMauiBTG.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ((MainViewModel)BindingContext).LoadCustomersCommand.ExecuteAsync(null);
        }
    }
}

