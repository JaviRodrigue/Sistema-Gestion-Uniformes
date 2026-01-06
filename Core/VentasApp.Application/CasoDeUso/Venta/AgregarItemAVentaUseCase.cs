namespace VentasApp.Application.CasoDeUso.Venta;
using VentasApp.Application.Interfaces;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Venta;

public class AgregarItemAVentaUseCase
{
    private readonly IVentaRepository _ventaRepository;

    public AgregarItemAVentaUseCase(IVentaRepository ventaRepository)
    {
        _ventaRepository = ventaRepository;
    }

    public async Task EjecutarAsync(int ventaId, int itemVendible, int cantidad, decimal precioUnitario)
    {
        //Primero busco si la venta existe, para poder agregar un item
        var venta = await _ventaRepository.ObtenerPorId(ventaId) ?? throw new Exception("La venta no existe");

        venta.AgregarDetalle(itemVendible,cantidad,precioUnitario);

        await _ventaRepository.Agregar(venta);
    }
}