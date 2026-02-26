using CommunityToolkit.Mvvm.ComponentModel;
using VentasApp.Domain.Base;
using System.Windows;

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
    private int _stockMaximo;

    [ObservableProperty]
    private bool _entregado;

    public decimal Subtotal => PrecioUnitario * Cantidad;


    partial void OnPrecioUnitarioChanged(decimal _) => OnPropertyChanged(nameof(Subtotal));
    
    partial void OnCantidadChanged(int value)
    {
        if (StockMaximo > 0 && value > StockMaximo)
        {
            var cantidadOriginal = value;
            Cantidad = StockMaximo;
            
            // Notificar al usuario usando Dispatcher para estar en el thread de UI
            if (!string.IsNullOrEmpty(Producto))
            {
                System.Windows.Application.Current?.Dispatcher?.InvokeAsync(() =>
                {
                    MessageBox.Show(
                        $"La cantidad solicitada ({cantidadOriginal}) excede el stock disponible.\n\n" +
                        $"Producto: {Producto}\n" +
                        $"Stock disponible: {StockMaximo} unidades\n\n" +
                        $"La cantidad se ajustó automáticamente a {StockMaximo}.",
                        "Stock Ajustado",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                });
            }
        }
        OnPropertyChanged(nameof(Subtotal));
    }
}
