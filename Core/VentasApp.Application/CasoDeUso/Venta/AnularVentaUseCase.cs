namespace VentasApp.Application.CasoDeUso.Venta;
using VentasApp.Application.Interfaces;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Venta;

public class AnularVentaUseCase
{
    private readonly IVentaRepository _ventaRepository;

    public AnularVentaUseCase(IVentaRepository ventaRepository)
    {
        _ventaRepository = ventaRepository;
    }

    public async Task EjecutarAsync(int idVenta)
    {
        var venta = await _ventaRepository.ObtenerPorId(idVenta) ?? throw new Exception("La venta no existe");
        venta.AnularVenta();
        await _ventaRepository.Actualizar(venta);
    }
}