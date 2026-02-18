using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using VentasApp.Desktop.ViewModels.Productos;
using VentasApp.Desktop.ViewModels.Ventas;
using VentasApp.Desktop.ViewModels.Cliente;
using VentasApp.Desktop.Views.Productos;
using VentasApp.Desktop.Views.Ventas;
using VentasApp.Desktop.Views.Cliente;
using VentasApp.Desktop.Views.Main;
using Microsoft.EntityFrameworkCore;
using VentasApp.Infrastructure.Persistencia.Contexto;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Infrastructure.Persistencia.Repositorios;
using VentasApp.Application.CasoDeUso.Venta;
using VentasApp.Application.CasoDeUso.Productos;
using VentasApp.Application.CasoDeUso.Cliente;
using VentasApp.Application.CasoDeUso.Stocks;
using VentasApp.Application.CasoDeUso.ItemVendibles;
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
                services.AddSingleton<MainWindow>(sp =>
                    new MainWindow(sp));
                // Ventana Principal
                // (Opcional si usas inyección en vistas, por ahora MainWindow es suficiente)

                // 2. REGISTRAR VIEWMODELS (Singleton para no perder estado al cambiar de vista)
                services.AddSingleton<ProductoViewModel>();
                services.AddSingleton<ClienteViewModel>();
                services.AddSingleton<VentaViewModel>();

                // 3. REGISTRAR SERVICIOS, DB CONTEXT, REPOS Y CASOS DE USO
                services.AddDbContext<DatabaseContext>(opt =>
                {
                    var folder = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "VentasApp");
                    Directory.CreateDirectory(folder);
                    var dbPath = Path.Combine(folder, "Database.sqlite");
                    opt.UseSqlite($"Data Source={dbPath}");
                });

                services.AddScoped<IVentaRepository, VentaRepository>();
                services.AddScoped<IUnitOfWork, UnitOfWork>();
                services.AddScoped<IStockRepository, StockRepository>();
                services.AddScoped<VentasApp.Application.Interfaces.Repositorios.IClienteRepository, VentasApp.Infrastructure.Persistencia.Repositorios.ClienteRepository>();
                services.AddScoped<VentasApp.Application.Interfaces.Repositorios.IProductoRepository, VentasApp.Infrastructure.Persistencia.Repositorios.ProductoRepository>();
                services.AddScoped<VentasApp.Application.Interfaces.Repositorios.IItemVendibleRepository, VentasApp.Infrastructure.Persistencia.Repositorios.ItemVendibleRepository>();

                services.AddTransient<ListarVentasUseCase>();
                services.AddTransient<ObtenerVentaUseCase>();
                services.AddTransient<CrearVentaUseCase>();
                services.AddTransient<VentasApp.Application.CasoDeUso.DetalleVenta.GuardarDetalleVentaUseCase>();
                services.AddTransient<AnularVentaUseCase>();
                
                // Productos UseCases
                services.AddTransient<CrearProductoUseCase>();
                services.AddTransient<ActualizarProductoUseCase>();
                services.AddTransient<CrearItemVendibleUseCase>();

                // Cliente UseCases
                services.AddTransient<ActualizarClienteCasoDeUso>();

                // Stocks UseCases
                services.AddTransient<CrearStockUseCase>();
                services.AddTransient<ObtenerStockUseCase>();
                services.AddTransient<AumentarStockUseCase>();
                services.AddTransient<DescontarStockUseCase>();
                services.AddTransient<ActualizarStockMinimoUseCase>();

                services.AddTransient<VentaView>();
                services.AddTransient<ProductoView>();
                services.AddTransient<ClienteView>();


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

            // 2. Asegurar creación de la base de datos
            using (var scope = AppHost.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await db.Database.EnsureCreatedAsync();
            }

            // 3. Pedirle al contenedor que cree la MainWindow
            // (Esto inyectará automáticamente todo lo que la MainWindow necesite)
            var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
            
            // 4. Mostrar la ventana
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