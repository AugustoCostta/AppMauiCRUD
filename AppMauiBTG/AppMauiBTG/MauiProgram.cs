using AppMauiBTG.Services;
using AppMauiBTG.ViewModels;
using AppMauiBTG.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace AppMauiBTG;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        string arquivoExe = Assembly.GetEntryAssembly()?.Location;
        string PathBase = Path.Combine(Path.GetDirectoryName(arquivoExe), "Data");
        Directory.CreateDirectory(PathBase);

        var PathDB = Path.Combine(PathBase, "customers.db3");
        builder.Services.AddSingleton<ICustomerService>(s => new CustomerService(PathDB));

        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddTransient<CustomerDetailViewModel>();

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<CustomerDetailPage>();



#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}

