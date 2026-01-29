using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using VentasApp.Desktop.ViewModels.Productos;
using VentasApp.Desktop.Views.Productos;
using System;
using System.Diagnostics;
// Asegúrate de importar tus otros namespaces si los necesitas
// using VentasApp.Infrastructure.Persistence; 
// using VentasApp.Application.UseCases...

namespace VentasApp.Desktop;

public partial class App : System.Windows.Application
{
    // El "Host" es el contenedor que guarda todos tus servicios y ViewModels
    public static IHost? AppHost { get; private set; }

    public App()
    {
        // Capturar excepciones no manejadas
        this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                // 1. REGISTRAR VENTANAS Y VISTAS
                services.AddSingleton<MainWindow>(); // Ventana Principal
                // (Opcional si usas inyección en vistas, por ahora MainWindow es suficiente)

                // 2. REGISTRAR VIEWMODELS
                services.AddTransient<ProductoViewModel>();

                // 3. REGISTRAR SERVICIOS Y CASOS DE USO (Aquí irán los de tu backend luego)
                // services.AddDbContext<VentasContext>();
                // services.AddScoped<IVentaRepository, VentaRepository>();

            })
            .Build();
    }

    private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
        MessageBox.Show($"ERROR NO MANEJADO:\n\n{e.Exception.Message}\n\nStackTrace:\n{e.Exception.StackTrace}", 
            "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        Debug.WriteLine($"ERROR: {e.Exception}");
        e.Handled = true;
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var ex = (Exception)e.ExceptionObject;
        MessageBox.Show($"ERROR CRITICO:\n\n{ex.Message}\n\nStackTrace:\n{ex.StackTrace}", 
            "Error Crítico", MessageBoxButton.OK, MessageBoxImage.Error);
        Debug.WriteLine($"ERROR CRITICO: {ex}");
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        try
        {
            // 1. Iniciar el Host
            await AppHost!.StartAsync();

            // 2. Pedirle al contenedor que cree la MainWindow
            // (Esto inyectará automáticamente todo lo que la MainWindow necesite)
            var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
            
            // 3. Mostrar la ventana
            startupForm.Show();

            base.OnStartup(e);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"ERROR AL INICIAR:\n\n{ex.Message}\n\nStackTrace:\n{ex.StackTrace}", 
                "Error de Inicio", MessageBoxButton.OK, MessageBoxImage.Error);
            Debug.WriteLine($"ERROR AL INICIAR: {ex}");
            Shutdown();
        }
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        // Limpiar recursos al cerrar
        await AppHost!.StopAsync();
        base.OnExit(e);
    }
}