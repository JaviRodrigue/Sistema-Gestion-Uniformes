using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;

using System.Collections.Generic;

using System.Collections.ObjectModel;
using System.Linq;
using VentasApp.Application.CasoDeUso.Venta;
using VentasApp.Application.DTOs.Venta;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Desktop.ViewModels;
using VentasApp.Desktop.ViewModels.DTOs;
using VentasApp.Desktop.Views.Ventas;
using VentasApp.Desktop.Messages;

namespace VentasApp.Desktop.ViewModels.Ventas;

public partial class VentaViewModel : ObservableObject, IBuscable
{
    private readonly IServiceProvider _provider;

    // note: use-cases are resolved per-operation from a scoped provider to avoid capturing
    // a scoped DbContext inside a long-lived singleton ViewModel which causes stale data.

    private List<VentaCardDto> _todasLasVentas = new();

    [ObservableProperty]
    private ObservableCollection<VentaCardDto> _ventas = new();

    [ObservableProperty]
    private string _filtroEstadoVenta = "Todas";

    [ObservableProperty]
    private string _filtroEstadoPago = "Todos";

    public List<string> FiltrosEstadoVenta { get; } = new() { "Todas", "Pendiente", "Confirmada", "Completada", "Cancelada" };
    public List<string> FiltrosEstadoPago { get; } = new() { "Todos", "Pendiente", "Pagada" };

    partial void OnFiltroEstadoVentaChanged(string value) => AplicarFiltros();
    partial void OnFiltroEstadoPagoChanged(string value) => AplicarFiltros();

    private List<VentaCardDto> _ventasBusqueda = new();

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

        _todasLasVentas = lista.Select(v => new VentaCardDto
        {
            Id = v.Id,
            Codigo = v.Codigo,
            Fecha = v.Fecha,
            Cliente = v.Cliente,
            EstadoVenta = v.EstadoVenta,
            EstadoPago = v.EstadoPago,
            Total = v.Total,
            Restante = v.Restante
        }).ToList();

        _ventasBusqueda = _todasLasVentas.ToList();
        AplicarFiltros();
    }

    private void AplicarFiltros()
    {
        var filtrados = _ventasBusqueda.AsEnumerable();
        
        if (FiltroEstadoVenta != "Todas")
            filtrados = filtrados.Where(v => v.EstadoVenta == FiltroEstadoVenta);
            
        if (FiltroEstadoPago != "Todos")
            filtrados = filtrados.Where(v => v.EstadoPago == FiltroEstadoPago);

        Ventas = new ObservableCollection<VentaCardDto>(filtrados);
    }

    // ================= IBuscable =================

    public async Task BuscarAsync(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto)) { await RestablecerAsync(); return; }

        var textoLower = texto.ToLowerInvariant();
        var esNumero = texto.All(char.IsDigit);
        var resultados = new HashSet<VentaCardDto>();

        if (esNumero)
        {
            // Buscar por ID de venta
            if (int.TryParse(texto, out var id))
            {
                var porId = _todasLasVentas.FirstOrDefault(v => v.Id == id);
                if (porId != null) resultados.Add(porId);
            }
            
            // Buscar por DNI o Teléfono del cliente
            var idsPorCliente = await ObtenerIdsVentasPorClienteAsync(async repo =>
            {
                var porDni = await repo.ObtenerClientePorDni(texto);
                var porTel = await repo.ObtenerClientePorTelefono(texto);
                return new[] { porDni, porTel }
                    .Where(c => c is not null)
                    .DistinctBy(c => c!.Id)
                    .Select(c => c!.Id)
                    .ToList();
            });
            
            foreach (var v in _todasLasVentas.Where(v => idsPorCliente.Contains(v.Id)))
            {
                resultados.Add(v);
            }
        }
        
        // Buscar por código de venta, nombre de cliente, estado de venta o estado de pago
        var porTexto = _todasLasVentas.Where(v => 
            v.Codigo.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
            v.Cliente.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
            v.EstadoVenta.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
            v.EstadoPago.Contains(texto, StringComparison.OrdinalIgnoreCase)
        );
        
        foreach (var v in porTexto)
        {
            resultados.Add(v);
        }
        
        // Buscar por nombre de cliente en la base de datos (por si el nombre en la tarjeta está incompleto)
        var idsPorNombreCliente = await ObtenerIdsVentasPorClienteAsync(async repo =>
        {
            var clientes = await repo.BuscarPorNombre(texto);
            return clientes.Select(c => c.Id).ToList();
        });
        
        foreach (var v in _todasLasVentas.Where(v => idsPorNombreCliente.Contains(v.Id)))
        {
            resultados.Add(v);
        }

        _ventasBusqueda = resultados.ToList();
        AplicarFiltros();
    }

    public Task RestablecerAsync()
    {
        _ventasBusqueda = _todasLasVentas.ToList();
        AplicarFiltros();
        return Task.CompletedTask;
    }

    private static async Task<List<int>> ObtenerIdsVentasPorClienteAsync(
        Func<IClienteRepository, Task<List<int>>> obtenerClienteIds)
    {
        using var scope = App.AppHost!.Services.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IClienteRepository>();
        var clienteIds = await obtenerClienteIds(repo);
        if (!clienteIds.Any()) return [];

        var useCase = scope.ServiceProvider.GetRequiredService<ObtenerIdsVentasPorClientesUseCase>();
        return await useCase.EjecutarAsync(clienteIds);
    }

    [RelayCommand]
    private async Task VerDetalle(VentaCardDto card)
    {
        using var scope = _provider.CreateScope();
        var obtener = scope.ServiceProvider.GetRequiredService<ObtenerVentaUseCase>();
        var detalle = await obtener.EjecutarAsync(card.Id);
        if (detalle is null) return;

        var uiDetalle = MapDetalle(detalle);
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
        
        WeakReferenceMessenger.Default.Send(new StockChangedMessage());
        
        await CargarAsync();
    }

    private static VentasApp.Desktop.ViewModels.DTOs.VentaDetalleDto MapDetalle(VentasApp.Application.DTOs.Venta.VentaDetalleDto src)
    {
        return new VentasApp.Desktop.ViewModels.DTOs.VentaDetalleDto
        {
            Id = src.Id,
            Codigo = src.Codigo,
            Fecha = src.Fecha,
            Cliente = src.Cliente,
            IdCliente = src.IdCliente,
            Estado = src.Estado,
            // FechaEstimada no existe en el DTO de aplicación
            Items = new ObservableCollection<VentasApp.Desktop.ViewModels.DTOs.VentaItemDto>(
                src.Items.Select(i => new VentasApp.Desktop.ViewModels.DTOs.VentaItemDto
                {
                    IdDetalle = i.IdDetalle,
                    ProductId = i.IdItemVendible,
                    Producto = i.Descripcion,
                    PrecioUnitario = i.PrecioUnitario,
                    Cantidad = i.Cantidad,
                    Entregado = i.Entregado
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
                            Verificado = p.Verificado,
                            MedioPago = metodo?.MedioPago ?? string.Empty,
                            MedioPagoId = metodo?.IdMedioPago ?? 0
                        };
                    }))
        };
    }

    [RelayCommand]
    private async Task AgregarVenta()
    {
        int idVentaCreada = 0;
        
        try
        {
            // 1. Crear venta
            using var scope = _provider.CreateScope();
            var crear = scope.ServiceProvider.GetRequiredService<CrearVentaUseCase>();
            idVentaCreada = await crear.EjecutarAsync(new CrearVentaDto());

            // 2. Obtener detalle recién creado (evita listar todo)
            var obtener = scope.ServiceProvider.GetRequiredService<ObtenerVentaUseCase>();
            var detalle = await obtener.EjecutarAsync(idVentaCreada);
            if (detalle is null) return;

            // 3. Mapear
            var uiDetalle = MapDetalle(detalle);

            // 4. Abrir ventana directamente
            var guardar = scope.ServiceProvider.GetRequiredService<VentasApp.Application.CasoDeUso.DetalleVenta.GuardarDetalleVentaUseCase>();
            var win = new DetalleVentaWindow(uiDetalle, guardar);
            var result = win.ShowDialog();

            // 5. Solo si guardó, refrescar listado
            if (result == true)
            {
                await CargarAsync();
            }
            else
            {
                // Si el usuario canceló, eliminar la venta vacía recién creada
                var anular = scope.ServiceProvider.GetRequiredService<AnularVentaUseCase>();
                await anular.EjecutarAsync(idVentaCreada);
            }
        }
        catch (Exception ex)
        {
            // Si hay error y se creó una venta, intentar limpiarla
            if (idVentaCreada > 0)
            {
                try
                {
                    using var scope = _provider.CreateScope();
                    var anular = scope.ServiceProvider.GetRequiredService<AnularVentaUseCase>();
                    await anular.EjecutarAsync(idVentaCreada);
                }
                catch
                {
                    // Ignorar errores al limpiar
                }
            }
            
            System.Windows.MessageBox.Show($"Error al crear venta: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }
    }
}
