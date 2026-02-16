using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VentasApp.Application.CasoDeUso.Venta;
using VentasApp.Application.DTOs.Venta;
using VentasApp.Desktop.ViewModels.DTOs;
using VentasApp.Desktop.Views.Ventas;

namespace VentasApp.Desktop.ViewModels.Ventas.VentaViewModel;

public partial class VentaViewModel : ObservableObject
{
    private readonly ListarVentasUseCase _listar;
    private readonly ObtenerVentaUseCase _obtener;
    private readonly CrearVentaUseCase _crear;
    private readonly AnularVentaUseCase _anular;

    [ObservableProperty]
    private ObservableCollection<VentaCardDto> _ventas = new();

    public VentaViewModel(
        ListarVentasUseCase listar,
        ObtenerVentaUseCase obtener,
        CrearVentaUseCase crear,
        AnularVentaUseCase anular)
    {
        _listar = listar;
        _obtener = obtener;
        _crear = crear;
        _anular = anular;

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
                EstadoPago = v.EstadoPago
             }));
    }

    [RelayCommand]
    private async Task VerDetalle(VentaCardDto card)
    {
        var detalle = await _obtener.EjecutarAsync(card.Id);
        if (detalle is null) return;

        var win = new DetalleVentaWindow(detalle);
        win.ShowDialog();
    }

    [RelayCommand]
    private async Task AgregarVenta()
    {
        var id = await _crear.EjecutarAsync(new CrearVentaDto());

        await CargarAsync();
    }

    [RelayCommand]
    private async Task EliminarVenta(VentaCardDto card)
    {
        await _anular.EjecutarAsync(card.Id);
        await CargarAsync();
    }
}
