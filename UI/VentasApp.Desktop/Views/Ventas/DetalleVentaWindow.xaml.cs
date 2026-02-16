using System.Windows;
using VentasApp.Application.DTOs.Venta;
using VentasApp.Desktop.ViewModels.Ventas;

namespace VentasApp.Desktop.Views.Ventas;

public partial class DetalleVentaWindow : Window
{
    public DetalleVentaWindow(VentaDetalleDto dto)
    {
        InitializeComponent();
        DataContext = dto;
    }

    private void Guardar_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }

    private void Cancelar_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
