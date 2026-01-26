using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using VentasApp.Desktop;

public partial class App : Application
{
    private IServiceProvider _serviceProvider;

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
        
    }
}
