using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using VentasApp.Desktop;


public partial class App : Application
{
    private IServiceProvider _serviceProvider = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();
        ConfigureServices(services);

        _serviceProvider = services.BuildServiceProvider();

        var mainWindow = new MainWindow
        {
            DataContext = _serviceProvider.GetRequiredService<MainViewModel>()
        };

        mainWindow.Show();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<INavigationService, NavigationService>();

        services.AddTransient<VentaViewModel>();
        services.AddTransient<ProductoViewModel>();
        services.AddSingleton<SidebarViewModel>();
        services.AddTransient<CategoriaViewModel>();
        services.AddTransient<StockViewModel>();
        services.AddTransient<PagoViewModel>();


    }
}
