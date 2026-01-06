namespace VentasApp.Application.CasoDeUso.Venta;
using VentasApp.Application.Interfaces;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Venta;

public class ConfirmarVentaUseCase
{
    private readonly IVentaRepository _ventaRepository;

    public ConfirmarVentaUseCase(IVentaRepository ventaRepository)
    {
        _ventaRepository = ventaRepository;
    }

    public async Task EjecutarAsync(int id)
    {
        //Verifico que la venta exista
        var venta = await _ventaRepository.ObtenerPorId(id) ?? throw new Exception("La venta no existe");

        venta.Confirmar();
        await _ventaRepository.Actualizar(venta);
    }
}