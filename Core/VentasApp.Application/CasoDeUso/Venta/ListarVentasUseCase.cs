namespace VentasApp.Application.CasoDeUso.Venta;
using VentasApp.Application.Interfaces;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Venta;

public class ListarVentasUseCase
{
    private readonly IVentaRepository _ventaRepository;

    public ListarVentasUseCase(IVentaRepository ventaRepository)
    {
        _ventaRepository = ventaRepository;
    }

    public async Task<List<Venta>> EjecutarAsync()
    {
        return await _ventaRepository.ObtenerTodas();
    }
}