using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VentasApp.Desktop.ViewModels.DTOs;

namespace VentasApp.Desktop.ViewModels.Ventas;

public partial class VentaViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<VentaCardDto> _ventas = new();

    public VentaViewModel()
    {
        CargarMock();
    }

    // =====================================================
    // VER DETALLE
    // =====================================================

    [RelayCommand]
    private void VerDetalle(VentaCardDto? venta)
    {
        if (venta is null)
            return;

        var window = new Views.Ventas.DetalleVentaWindow(venta.Detalle)
        {
            Owner = System.Windows.Application.Current.MainWindow
        };

        window.ShowDialog();

        // 🔥 notificar refresco visual
        OnPropertyChanged(nameof(Ventas));
    }

    // =====================================================
    // ELIMINAR
    // =====================================================

    [RelayCommand]
    private void EliminarVenta(VentaCardDto? venta)
    {
        if (venta is null)
            return;

        Ventas.Remove(venta);
    }

    // =====================================================
    // NUEVA VENTA
    // =====================================================

    [RelayCommand]
    private void AgregarVenta()
    {
        var detalle = new VentaDetalleDto
        {
            Codigo = $"V-{Ventas.Count + 1:0000}"
        };

        var window = new Views.Ventas.DetalleVentaWindow(detalle)
        {
            Owner = System.Windows.Application.Current.MainWindow
        };

        var ok = window.ShowDialog();

        if (ok != true)
            return;

        Ventas.Add(new VentaCardDto
        {
            Id = Ventas.Count + 1,
            Codigo = detalle.Codigo,
            Fecha = DateTime.Today,
            EstadoVenta = "Nueva",
            EstadoPago = detalle.Restante > 0 ? "Pendiente" : "Pagado",
            Cliente = detalle.Cliente,
            Detalle = detalle
        });
    }

    // =====================================================
    // MOCK
    // =====================================================

    private void CargarMock()
    {
        var d1 = new VentaDetalleDto
        {
            Codigo = "V-0001",
            Cliente = "María González"
        };

        d1.Items.Add(new VentaItemDto { Producto = "Leche", Cantidad = 2, PrecioUnitario = 900 });
        d1.Pagos.Add(new PagoDto { Fecha = DateTime.Today, MedioPago = "Efectivo", Monto = 1800 });

        Ventas.Add(new VentaCardDto
        {
            Id = 1,
            Codigo = d1.Codigo,
            Fecha = DateTime.Today,
            EstadoVenta = "Completada",
            EstadoPago = "Pagado",
            Cliente = d1.Cliente,
            Detalle = d1
        });
    }
}
