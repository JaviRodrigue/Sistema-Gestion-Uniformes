using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;
using VentasApp.Desktop.ViewModels;
using VentasApp.Desktop.Views.Cliente;
using VentasApp.Desktop.Views.Config;
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
        NavigateTo(_provider.GetRequiredService<VentaView>());
    }

    private void OnBtnVentasClick(object sender, RoutedEventArgs e) =>
        NavigateTo(_provider.GetRequiredService<VentaView>());

    private void OnBtnClientesClick(object sender, RoutedEventArgs e) =>
        NavigateTo(_provider.GetRequiredService<ClienteView>());

    private void OnBtnProductosClick(object sender, RoutedEventArgs e) =>
        NavigateTo(_provider.GetRequiredService<ProductoView>());

    private void OnBtnConfigClick(object sender, RoutedEventArgs e) =>
        NavigateTo(_provider.GetRequiredService<ConfigView>());

    private void NavigateTo(UIElement view)
    {
        TxtBuscar.Text = string.Empty;
        ContentHost.Content = view;
    }

    private async void OnBuscarTextChanged(object sender, TextChangedEventArgs e)
    {
        var texto = TxtBuscar.Text?.Trim() ?? string.Empty;
        if ((ContentHost.Content as FrameworkElement)?.DataContext is IBuscable buscable)
        {
            if (string.IsNullOrWhiteSpace(texto))
                await buscable.RestablecerAsync();
            else
                await buscable.BuscarAsync(texto);
        }
    }

    private void OnLimpiarBusquedaClick(object sender, RoutedEventArgs e) =>
        TxtBuscar.Text = string.Empty;
}
