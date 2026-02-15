using System.Windows;
using VentasApp.Desktop.ViewModels.DTOs;

namespace VentasApp.Desktop.Views.Ventas;

public partial class DetalleVentaWindow : Window
{
    public DetalleVentaWindow(VentaDetalleDto dto)
    {
        InitializeComponent();
        DataContext = dto;
    }
}
