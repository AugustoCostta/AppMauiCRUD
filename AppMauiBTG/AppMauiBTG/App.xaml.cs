using Microsoft.Extensions.DependencyInjection;
using AppMauiBTG.ViewModels;
using AppMauiBTG.Views;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;
#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using WinRT.Interop;
#endif


namespace AppMauiBTG
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var mainPage = IPlatformApplication.Current.Services.GetService<MainPage>();
            var window = new Window(mainPage);

#if WINDOWS
    window.Created += (s, e) =>
    {
        var mauiWin = (Window)s;
        if (mauiWin.Handler?.PlatformView is Microsoft.UI.Xaml.Window nativeWin)
        {
            nativeWin.Closed += (sender, args) =>
            {
                Environment.Exit(0);
            };

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(nativeWin);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);

            if (appWindow?.Presenter is Microsoft.UI.Windowing.OverlappedPresenter presenter)
            {
                presenter.Maximize();
            }
        }
    };
#endif

            return window;
        }

    }
}

