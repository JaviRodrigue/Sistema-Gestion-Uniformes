using CommunityToolkit.Mvvm.ComponentModel;
using VentasApp.Domain.Base;

namespace VentasApp.Desktop.ViewModels.DTOs;

public partial class VentaItemDto : ObservableObject
{
    [ObservableProperty]
    private int _idDetalle;

    [ObservableProperty]
    private int _productId;

    partial void OnProductIdChanged(int _)
    {
        // When ProductId changes from UI, try to update Producto name and PrecioUnitario if a products list is available
        // The ViewModel will subscribe to PropertyChanged and handle price lookup; this partial is kept for potential future use.
    }

    [ObservableProperty]
    private string _producto = "";

    [ObservableProperty]
    private decimal _precioUnitario;

    [ObservableProperty]
    private int _cantidad;

    [ObservableProperty]
    private bool _entregado;

    public decimal Subtotal => PrecioUnitario * Cantidad;


    partial void OnPrecioUnitarioChanged(decimal _) => OnPropertyChanged(nameof(Subtotal));
    partial void OnCantidadChanged(int _) => OnPropertyChanged(nameof(Subtotal));
}
