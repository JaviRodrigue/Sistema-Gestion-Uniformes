namespace VentasApp.Application.CasoDeUso.Venta;

using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Enum;
using VentasApp.Domain.Modelo.Venta;

//Caso de uso para crar una venta vacia
public class CrearVentaUseCase
{
    private readonly IVentaRepository _ventaRepository;

    public CrearVentaUseCase(IVentaRepository ventaRepository)
    {
        _ventaRepository = ventaRepository;
    }

    public async Task<int> EjecutarAsync(TipoVenta tipoVenta)
    {
        var venta = new Venta(tipoVenta);
        await _ventaRepository.Agregar(venta);
        return venta.Id;
    }
}