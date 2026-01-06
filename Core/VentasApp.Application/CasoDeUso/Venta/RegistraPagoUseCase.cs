namespace VentasApp.Application.CasoDeUso.Venta;
using VentasApp.Application.Interfaces;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Venta;

public class RegistrarPagoUseCase
{
    private readonly IVentaRepository _ventaRepository;

    public RegistrarPagoUseCase(IVentaRepository ventaRepository)
    {
        _ventaRepository = ventaRepository;
    }

    public async Task EjecutarAsync(int idVenta, decimal monto)
    {
        //Verifico que la venta exista
        var venta = await _ventaRepository.ObtenerPorId(idVenta) ?? throw new Exception("La venta no existe");
        
    }
}