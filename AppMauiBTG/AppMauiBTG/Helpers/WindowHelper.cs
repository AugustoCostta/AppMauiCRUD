using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using WinRT.Interop;
#endif


namespace AppMauiBTG.Helpers
{
    public static class WindowHelper
    {
#if WINDOWS
    public static void CenterOnScreen(Window window, int width = 800, int height = 600)
    {
        window.Created += (s, e) =>
        {
            var mauiWindow = (Window)s;
            var nativeWin = mauiWindow.Handler?.PlatformView as Microsoft.UI.Xaml.Window;
            if (nativeWin == null)
                return;

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(nativeWin);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
            if (appWindow == null)
                return;

            appWindow.Resize(new Windows.Graphics.SizeInt32(width, height));

            var displayArea = Microsoft.UI.Windowing.DisplayArea.GetFromWindowId(windowId, Microsoft.UI.Windowing.DisplayAreaFallback.Primary);
            int centerX = (displayArea.WorkArea.Width - width) / 2;
            int centerY = (displayArea.WorkArea.Height - height) / 2;

            appWindow.Move(new Windows.Graphics.PointInt32(centerX, centerY));
        };
        }
#endif
    }
    public static class WindowExtensions
    {
#if WINDOWS
        public static void CloseWindow(this Window window)
        {
            if (window.Handler?.PlatformView is not Microsoft.UI.Xaml.Window nativeWin)
                return;

            var hwnd = WindowNative.GetWindowHandle(nativeWin);
            var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);

            if (appWindow != null)
            {
                appWindow.Hide();
            }
        }
#endif
    }
}

