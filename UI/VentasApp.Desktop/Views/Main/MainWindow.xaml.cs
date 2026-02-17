using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;
using VentasApp.Desktop.Views.Cliente;
using VentasApp.Desktop.Views.Productos;
using VentasApp.Desktop.Views.Ventas;

namespace VentasApp.Desktop.Views.Main;

public partial class MainWindow : Window
{
    private readonly IServiceProvider _provider;

    public MainWindow(IServiceProvider provider)
    {
        InitializeComponent();

        _provider = provider;

        // Vista inicial (evita pantalla vacía)
        ContentHost.Content = _provider.GetRequiredService<VentaView>(); // Initialize with VentaView
    }

    private void OnBtnVentasClick(object sender, RoutedEventArgs e)
    {
        ContentHost.Content = _provider.GetRequiredService<VentaView>(); // Load VentaView
    }

    private void OnBtnClientesClick(object sender, RoutedEventArgs e)
    {
        ContentHost.Content = _provider.GetRequiredService<ClienteView>(); // Load ClienteView
    }

    private void OnBtnProductosClick(object sender, RoutedEventArgs e)
    {
        ContentHost.Content = _provider.GetRequiredService<ProductoView>(); // Load ProductoView
    }

    private void OnBtnConfigClick(object sender, RoutedEventArgs e)
    {
        ContentHost.Content = new TextBlock
        {
            Text = "Configuración (próximamente)",
            FontSize = 24
        };
    }
}
