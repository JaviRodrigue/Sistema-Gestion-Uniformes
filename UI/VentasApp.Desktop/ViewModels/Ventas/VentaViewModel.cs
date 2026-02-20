using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VentasApp.Application.CasoDeUso.Venta;
using VentasApp.Application.DTOs.Venta;
using VentasApp.Desktop.ViewModels.DTOs;
using VentasApp.Desktop.Views.Ventas;

namespace VentasApp.Desktop.ViewModels.Ventas;

public partial class VentaViewModel : ObservableObject
{
    private readonly ListarVentasUseCase _listar;
    private readonly ObtenerVentaUseCase _obtener;
    private readonly CrearVentaUseCase _crear;
    private readonly AnularVentaUseCase _anular;
    private readonly VentasApp.Application.CasoDeUso.DetalleVenta.GuardarDetalleVentaUseCase _guardarDetalle;

    [ObservableProperty]
    private ObservableCollection<VentaCardDto> _ventas = new();

    public VentaViewModel(
        ListarVentasUseCase listar,
        ObtenerVentaUseCase obtener,
        CrearVentaUseCase crear,
        AnularVentaUseCase anular,
        VentasApp.Application.CasoDeUso.DetalleVenta.GuardarDetalleVentaUseCase guardarDetalle)
    {
        _listar = listar;
        _obtener = obtener;
        _crear = crear;
        _anular = anular;
        _guardarDetalle = guardarDetalle;

        _ = CargarAsync();
    }

    private async Task CargarAsync()
    {
        var lista = await _listar.EjecutarAsync();

        Ventas = new ObservableCollection<VentaCardDto>(
            lista.Select(v => new VentaCardDto
            {
                Id = v.Id,
                Codigo = v.Codigo,
                Fecha = v.Fecha,
                EstadoVenta = v.EstadoVenta,
                EstadoPago = v.EstadoPago,
                Total = v.Total,
                Restante = v.Restante
             }));
    }

    [RelayCommand]
    private async Task VerDetalle(VentaCardDto card)
    {
        var detalle = await _obtener.EjecutarAsync(card.Id);
        if (detalle is null) return;

        var uiDetalle = MapDetalle(detalle);
        uiDetalle.Id = detalle.Id;
        var win = new DetalleVentaWindow(uiDetalle, _guardarDetalle);
        var result = win.ShowDialog();

        // Si el usuario guardó cambios, recargar la lista para actualizar totales en el listado
        if (result == true)
        {
            await CargarAsync();
        }
    }

    private static VentasApp.Desktop.ViewModels.DTOs.VentaDetalleDto MapDetalle(VentasApp.Application.DTOs.Venta.VentaDetalleDto src)
    {
        return new VentasApp.Desktop.ViewModels.DTOs.VentaDetalleDto
        {
            Codigo = src.Codigo,
            Cliente = src.Cliente,
            // FechaEstimada no existe en el DTO de aplicación
            Items = new ObservableCollection<VentasApp.Desktop.ViewModels.DTOs.VentaItemDto>(
                src.Items.Select(i => new VentasApp.Desktop.ViewModels.DTOs.VentaItemDto
                {
                    IdDetalle = i.IdDetalle,
                    ProductId = i.IdItemVendible,
                    Producto = i.Descripcion,
                    PrecioUnitario = i.PrecioUnitario,
                    Cantidad = i.Cantidad
                })),
            Pagos = new ObservableCollection<VentasApp.Desktop.ViewModels.DTOs.PagoDto>(
                src.Pagos.Select(p =>
                    {
                        var metodo = p.Metodos.FirstOrDefault();
                        return new VentasApp.Desktop.ViewModels.DTOs.PagoDto
                        {
                            Id = p.Id,
                            Fecha = p.FechaPago,
                            Monto = p.Total,
                            MedioPago = metodo?.MedioPago ?? string.Empty,
                            MedioPagoId = metodo?.IdMedioPago ?? 0
                        };
                    }))
        };
    }

    [RelayCommand]
    private async Task AgregarVenta()
    {
        // 1. Crear venta
        var id = await _crear.EjecutarAsync(new CrearVentaDto());

        // 2. Obtener detalle recién creado (evita listar todo)
        var detalle = await _obtener.EjecutarAsync(id);
        if (detalle is null) return;

        // 3. Mapear
        var uiDetalle = MapDetalle(detalle);
        uiDetalle.Id = detalle.Id;

        // 4. Abrir ventana directamente
        var win = new DetalleVentaWindow(uiDetalle, _guardarDetalle);
        var result = win.ShowDialog();

        // 5. Solo si guardó, refrescar listado
        if (result == true)
        {
            await CargarAsync();
        }
    }
}
