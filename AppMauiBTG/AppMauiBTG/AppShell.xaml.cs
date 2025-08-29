namespace AppMauiBTG
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(Views.CustomerDetailPage), typeof(Views.CustomerDetailPage));
        }
    }
}

