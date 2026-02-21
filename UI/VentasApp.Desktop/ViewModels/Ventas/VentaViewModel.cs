using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
using VentasApp.Infrastructure.Persistencia.Contexto;

namespace VentasApp.Desktop.ViewModels.Ventas;

public partial class VentaViewModel : ObservableObject, IBuscable
{
    private readonly ListarVentasUseCase _listar;
    private readonly ObtenerVentaUseCase _obtener;
    private readonly CrearVentaUseCase _crear;
    private readonly AnularVentaUseCase _anular;
    private readonly VentasApp.Application.CasoDeUso.DetalleVenta.GuardarDetalleVentaUseCase _guardarDetalle;

    private List<VentaCardDto> _todasLasVentas = new();

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

        _todasLasVentas = lista.Select(v => new VentaCardDto
        {
            Id = v.Id,
            Codigo = v.Codigo,
            Fecha = v.Fecha,
            EstadoVenta = v.EstadoVenta,
            EstadoPago = v.EstadoPago,
            Total = v.Total,
            Restante = v.Restante
        }).ToList();

        Ventas = new ObservableCollection<VentaCardDto>(_todasLasVentas);
    }

    // ================= IBuscable =================

    public async Task BuscarAsync(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto)) { await RestablecerAsync(); return; }

        var esNumero = texto.All(char.IsDigit);

        if (esNumero)
        {
            if (texto.Length < 5)
            {
                var id = int.Parse(texto);
                Ventas = new ObservableCollection<VentaCardDto>(_todasLasVentas.Where(v => v.Id == id));
            }
            else
            {
                var ids = await ObtenerIdsVentasPorClienteAsync(async repo =>
                {
                    var porDni = await repo.ObtenerClientePorDni(texto);
                    var porTel = await repo.ObtenerClientePorTelefono(texto);
                    return new[] { porDni, porTel }
                        .Where(c => c is not null)
                        .DistinctBy(c => c!.Id)
                        .Select(c => c!.Id)
                        .ToList();
                });
                Ventas = new ObservableCollection<VentaCardDto>(_todasLasVentas.Where(v => ids.Contains(v.Id)));
            }
        }
        else
        {
            var ids = await ObtenerIdsVentasPorClienteAsync(async repo =>
            {
                var clientes = await repo.BuscarPorNombre(texto);
                return clientes.Select(c => c.Id).ToList();
            });
            Ventas = new ObservableCollection<VentaCardDto>(_todasLasVentas.Where(v => ids.Contains(v.Id)));
        }
    }

    public Task RestablecerAsync()
    {
        Ventas = new ObservableCollection<VentaCardDto>(_todasLasVentas);
        return Task.CompletedTask;
    }

    private static async Task<List<int>> ObtenerIdsVentasPorClienteAsync(
        Func<IClienteRepository, Task<List<int>>> obtenerClienteIds)
    {
        using var scope = App.AppHost!.Services.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IClienteRepository>();
        var clienteIds = await obtenerClienteIds(repo);
        if (!clienteIds.Any()) return [];

        var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        return db.Compras
            .Where(c => clienteIds.Contains(c.IdCliente))
            .Select(c => c.IdVenta)
            .Distinct()
            .ToList();
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

    [RelayCommand]
    private async Task EliminarVenta(VentaCardDto card)
    {
        var result = System.Windows.MessageBox.Show($"¿Anular la venta {card.Codigo}?", "Confirmar anulación", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
        if (result != System.Windows.MessageBoxResult.Yes) return;

        await _anular.EjecutarAsync(card.Id);
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
