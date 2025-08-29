using AppMauiBTG.ViewModels;
using Microsoft.Maui.Controls;


namespace AppMauiBTG.Views
{
    public partial class CustomerDetailPage : ContentPage
    {
        private Window? _window;

        public CustomerDetailPage(CustomerDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        public void SetWindow(Window window)
        {
            _window = window;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        private void AgeEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry entry && !string.IsNullOrEmpty(entry.Text))
            {
                entry.Text = new string(entry.Text.Where(char.IsDigit).ToArray());
            }
        }
    }
}

