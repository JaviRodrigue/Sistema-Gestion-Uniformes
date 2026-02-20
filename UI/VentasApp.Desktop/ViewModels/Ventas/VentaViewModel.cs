using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using VentasApp.Application.CasoDeUso.Venta;
using VentasApp.Application.DTOs.Venta;
using VentasApp.Desktop.ViewModels.DTOs;
using VentasApp.Desktop.Views.Ventas;

namespace VentasApp.Desktop.ViewModels.Ventas;

public partial class VentaViewModel : ObservableObject
{
    private readonly IServiceProvider _provider;

    // note: use-cases are resolved per-operation from a scoped provider to avoid capturing
    // a scoped DbContext inside a long-lived singleton ViewModel which causes stale data.

    [ObservableProperty]
    private ObservableCollection<VentaCardDto> _ventas = new();

    public VentaViewModel(IServiceProvider provider)
    {
        _provider = provider;

        _ = CargarAsync();
    }

    // Public helper to allow external callers (e.g. detail window) to request a refresh
    public Task RefreshAsync()
    {
        return CargarAsync();
    }

    private async Task CargarAsync()
    {
        // Resolve use case from a scoped provider to ensure a fresh DbContext is used
        using var scope = _provider.CreateScope();
        var listar = scope.ServiceProvider.GetRequiredService<ListarVentasUseCase>();
        var lista = await listar.EjecutarAsync();

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
        using var scope = _provider.CreateScope();
        var obtener = scope.ServiceProvider.GetRequiredService<ObtenerVentaUseCase>();
        var detalle = await obtener.EjecutarAsync(card.Id);
        if (detalle is null) return;

        var uiDetalle = MapDetalle(detalle);
        uiDetalle.Id = detalle.Id;
        // Resolve the GuardarDetalle usecase from transient scope and pass it to the window
        var guardar = scope.ServiceProvider.GetRequiredService<VentasApp.Application.CasoDeUso.DetalleVenta.GuardarDetalleVentaUseCase>();
        var win = new DetalleVentaWindow(uiDetalle, guardar);
        var result = win.ShowDialog();

        // Si el usuario guardó cambios, recargar la lista para actualizar totales en el listado
        if (result == true)
        {
            await CargarAsync();
        }
    }

    [RelayCommand]
    private async Task EliminarVenta(VentaCardDto card)
    {
        var result = System.Windows.MessageBox.Show($"¿Anular la venta {card.Codigo}?", "Confirmar anulación", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
        if (result != System.Windows.MessageBoxResult.Yes) return;

        using var scope = _provider.CreateScope();
        var anular = scope.ServiceProvider.GetRequiredService<AnularVentaUseCase>();
        await anular.EjecutarAsync(card.Id);
        await CargarAsync();
    }

    private static VentasApp.Desktop.ViewModels.DTOs.VentaDetalleDto MapDetalle(VentasApp.Application.DTOs.Venta.VentaDetalleDto src)
    {
        return new VentasApp.Desktop.ViewModels.DTOs.VentaDetalleDto
        {
            Codigo = src.Codigo,
            Cliente = src.Cliente,
            IdCliente = src.IdCliente,
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
        using var scope = _provider.CreateScope();
        var crear = scope.ServiceProvider.GetRequiredService<CrearVentaUseCase>();
        var id = await crear.EjecutarAsync(new CrearVentaDto());

        // 2. Obtener detalle recién creado (evita listar todo)
        var obtener = scope.ServiceProvider.GetRequiredService<ObtenerVentaUseCase>();
        var detalle = await obtener.EjecutarAsync(id);
        if (detalle is null) return;

        // 3. Mapear
        var uiDetalle = MapDetalle(detalle);
        uiDetalle.Id = detalle.Id;

        // 4. Abrir ventana directamente
        var guardar = scope.ServiceProvider.GetRequiredService<VentasApp.Application.CasoDeUso.DetalleVenta.GuardarDetalleVentaUseCase>();
        var win = new DetalleVentaWindow(uiDetalle, guardar);
        var result = win.ShowDialog();

        // 5. Solo si guardó, refrescar listado
        if (result == true)
        {
            await CargarAsync();
        }
    }
}
