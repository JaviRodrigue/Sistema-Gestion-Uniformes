namespace VentasApp.Application.CasoDeUso.Venta;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Venta;

public class ObtenerVentaUseCase{
    
    private readonly IVentaRepository _ventaRepository;


    public ObtenerVentaUseCase(IVentaRepository ventaRepository)
    {
        _ventaRepository = ventaRepository;
    }

    public async Task<Venta?> EjecutarAsync(int id)
    {
        return await _ventaRepository.ObtenerPorId(id);
    }
}