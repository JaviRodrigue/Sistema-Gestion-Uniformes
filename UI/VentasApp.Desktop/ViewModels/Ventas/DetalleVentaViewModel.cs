using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VentasApp.Desktop.ViewModels.DTOs;

namespace VentasApp.Desktop.ViewModels.Ventas;

public partial class DetalleVentaViewModel : ObservableObject
{
    public VentaDetalleDto Venta { get; }

    public DetalleVentaViewModel(VentaDetalleDto venta)
    {
        Venta = venta;
    }

    // ================= ITEMS =================

    [RelayCommand]
    private void AgregarItem()
    {
        Venta.Items.Add(new VentaItemDto
        {
            Producto = "Nuevo producto",
            Cantidad = 1,
            PrecioUnitario = 0
        });
    }

    [RelayCommand]
    private void EliminarItem(VentaItemDto? item)
    {
        if (item is null) return;
        Venta.Items.Remove(item);
    }

    // ================= PAGOS =================

    [RelayCommand]
    private void AgregarPago()
    {
        Venta.Pagos.Add(new PagoDto
        {
            Fecha = System.DateTime.Today,
            Monto = 0,
            MedioPago = "Efectivo"
        });
    }

    [RelayCommand]
    private void EliminarPago(PagoDto? pago)
    {
        if (pago is null) return;
        Venta.Pagos.Remove(pago);
    }
}
