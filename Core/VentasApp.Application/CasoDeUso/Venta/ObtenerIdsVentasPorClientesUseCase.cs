using VentasApp.Application.Interfaces.Repositorios;

namespace VentasApp.Application.CasoDeUso.Venta;

public class ObtenerIdsVentasPorClientesUseCase
{
    private readonly IVentaRepository _ventaRepository;

    public ObtenerIdsVentasPorClientesUseCase(IVentaRepository ventaRepository)
    {
        _ventaRepository = ventaRepository;
    }

    public async Task<List<int>> EjecutarAsync(List<int> clientesIds)
    {
        return await _ventaRepository.ObtenerIdsVentasPorClientes(clientesIds);
    }
}
