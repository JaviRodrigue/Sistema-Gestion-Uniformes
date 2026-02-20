using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VentasApp.Desktop.ViewModels.DTOs;

namespace VentasApp.Desktop.ViewModels.Ventas;

public partial class DetalleVentaViewModel : ObservableObject
{
    public VentaDetalleDto Venta { get; }

        public System.Collections.ObjectModel.ObservableCollection<VentasApp.Application.DTOs.Productos.ListadoProductoDto> Productos { get; set; } = new();
        public System.Collections.ObjectModel.ObservableCollection<VentasApp.Application.DTOs.Productos.ListadoProductoDto> MediosPago { get; set; } = new();

    public string Cliente
    {
        get => Venta.Cliente;
        set
        {
            Venta.Cliente = value;
            OnPropertyChanged();
        }
    }

    public int IdCliente
    {
        get => Venta.IdCliente;
        set
        {
            Venta.IdCliente = value;
            OnPropertyChanged();
        }
    }

    public DateTime? FechaEstimada
    {
        get => Venta.FechaEstimada;
        set
        {
            Venta.FechaEstimada = value;
            OnPropertyChanged();
        }
    }

    public DetalleVentaViewModel(VentaDetalleDto venta)
    {
        Venta = venta ?? new VentaDetalleDto();

        // subscribe to collection changes so totals update and manage item subscriptions
        Venta.Items.CollectionChanged += Items_CollectionChanged;
        Venta.Pagos.CollectionChanged += Pagos_CollectionChanged;

        foreach (var item in Venta.Items)
        {
            item.PropertyChanged += Item_PropertyChanged;
            // if the item already has a product selected, update price from Productos
            if (item.ProductId != 0)
            {
                var prod = Productos.FirstOrDefault(p => p.Id == item.ProductId);
                if (prod is not null)
                {
                    item.PrecioUnitario = prod.PrecioVenta;
                    item.Producto = prod.Nombre;
                }
            }
        }

        foreach (var pago in Venta.Pagos)
            pago.PropertyChanged += Pago_PropertyChanged;
    }

    private void RaiseTotalsChanged()
    {
        OnPropertyChanged(nameof(Total));
        OnPropertyChanged(nameof(Pagado));
        OnPropertyChanged(nameof(Restante));
    }

    public decimal Total => Venta.Total;
    public decimal Pagado => Venta.Pagado;
    public decimal Restante => Venta.Restante;

    // ================= ITEMS =================

    [RelayCommand]
    private void AgregarItem()
    {
        var it = new VentaItemDto
        {
            IdDetalle = 0,
            ProductId = 0,
            Producto = string.Empty,
            Cantidad = 1,
            PrecioUnitario = 0
        };
        it.PropertyChanged += Item_PropertyChanged;
        Venta.Items.Add(it);
    }

    [RelayCommand]
    private void EliminarItem(VentaItemDto? item)
    {
        if (item is null) return;
        item.PropertyChanged -= Item_PropertyChanged;
        Venta.Items.Remove(item);
    }

    // ================= PAGOS =================

    [RelayCommand]
    private void AgregarPago()
    {
        var p = new PagoDto
        {
            Fecha = System.DateTime.Today,
            Monto = 0,
            MedioPago = "Efectivo"
        };
        p.PropertyChanged += Pago_PropertyChanged;
        Venta.Pagos.Add(p);
    }

    [RelayCommand]
    private void EliminarPago(PagoDto? pago)
    {
        if (pago is null) return;
        pago.PropertyChanged -= Pago_PropertyChanged;
        Venta.Pagos.Remove(pago);
    }

    private void Items_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems != null)
        {
            foreach (VentaItemDto it in e.OldItems)
                it.PropertyChanged -= Item_PropertyChanged;
        }
        if (e.NewItems != null)
        {
            foreach (VentaItemDto it in e.NewItems)
                it.PropertyChanged += Item_PropertyChanged;
        }
        RaiseTotalsChanged();
    }

    private void Pagos_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems != null)
        {
            foreach (PagoDto p in e.OldItems)
                p.PropertyChanged -= Pago_PropertyChanged;
        }
        if (e.NewItems != null)
        {
            foreach (PagoDto p in e.NewItems)
                p.PropertyChanged += Pago_PropertyChanged;
        }
        RaiseTotalsChanged();
    }

    private void Item_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        // when price or quantity change, subtotal changes
        if (e.PropertyName is nameof(VentaItemDto.Subtotal) or nameof(VentaItemDto.PrecioUnitario) or nameof(VentaItemDto.Cantidad))
        {
            RaiseTotalsChanged();
        }
        // react to product selection changes
        if (e.PropertyName == nameof(VentaItemDto.ProductId))
        {
            var item = sender as VentaItemDto;
            if (item is null) return;
            var prod = Productos.FirstOrDefault(p => p.Id == item.ProductId);
            if (prod is not null)
            {
                item.PrecioUnitario = prod.PrecioVenta;
                item.Producto = prod.Nombre;
            }
            RaiseTotalsChanged();
        }
    }

    private void Pago_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(PagoDto.Monto))
        {
            RaiseTotalsChanged();
        }
    }
}
