using CommunityToolkit.Mvvm.ComponentModel;
using VentasApp.Domain.Base;

namespace VentasApp.Desktop.ViewModels.DTOs;

public partial class VentaItemDto : ObservableObject
{
    [ObservableProperty]
    private int _idDetalle;

    [ObservableProperty]
    private string _producto = "";

    [ObservableProperty]
    private decimal _precioUnitario;

    [ObservableProperty]
    private int _cantidad;

    public decimal Subtotal => PrecioUnitario * Cantidad;

    partial void OnPrecioUnitarioChanged(decimal _) => OnPropertyChanged(nameof(Subtotal));
    partial void OnCantidadChanged(int _) => OnPropertyChanged(nameof(Subtotal));
}
