using System;
using System.Collections.ObjectModel;
using System.Linq;
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

    [RelayCommand]
    private void VerDetalle(VentaCardDto venta)
    {
        var detalle = CrearDetalleMock(venta);

        var window = new Views.Ventas.DetalleVentaWindow(detalle)
        {
            Owner = System.Windows.Application.Current.MainWindow
        };

        window.ShowDialog();
    }

    [RelayCommand]
    private void EliminarVenta(VentaCardDto venta)
    {
        Ventas.Remove(venta);
    }

    // ================= MOCK =================

    private void CargarMock()
    {
        Ventas =
        [
            new()
            {
                Id = 1,
                Codigo = "V-0001",
                Fecha = DateTime.Today,
                Total = 15000,
                Restante = 0,
                EstadoVenta = "Completada",
                EstadoPago = "Pagado",
                Cliente = "María González"
            },
            new()
            {
                Id = 2,
                Codigo = "V-0002",
                Fecha = DateTime.Today.AddDays(-2),
                Total = 25000,
                Restante = 8000,
                EstadoVenta = "Pendiente",
                EstadoPago = "Parcial",
                Cliente = "Carlos Pérez"
            }
        ];
    }

    private VentaDetalleDto CrearDetalleMock(VentaCardDto v)
    {
        return new VentaDetalleDto
        {
            Codigo = v.Codigo,
            Cliente = v.Cliente ?? "",
            Total = v.Total,
            Restante = v.Restante,
            Items = new()
            {
                new VentaItemDto { Producto="Galletitas", Cantidad=2, PrecioUnitario=1500 },
                new VentaItemDto { Producto="Leche", Cantidad=5, PrecioUnitario=900 }
            },
            Pagos = new()
            {
                new PagoDto { Fecha=DateTime.Today, Monto=10000, MedioPago="Efectivo" }
            }
        };
    }
}
