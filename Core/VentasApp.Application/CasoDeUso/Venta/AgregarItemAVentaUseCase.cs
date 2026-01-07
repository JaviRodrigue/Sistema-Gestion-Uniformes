namespace VentasApp.Application.CasoDeUso.Venta;

using VentasApp.Application.DTOs.Venta;
using VentasApp.Application.Interfaces.Repositorios;

public class AgregarItemAVentaUseCase
{
    private readonly IVentaRepository _ventaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AgregarItemAVentaUseCase(IVentaRepository ventaRepository, IUnitOfWork unit)
    {
        _ventaRepository = ventaRepository;
        _unitOfWork = unit;
    }

    public async Task EjecutarAsync(int idVenta, AgregarDetalleDto dto)
    {
        //Primero busco si la venta existe, para poder agregar un item
        var venta = await _ventaRepository.ObtenerPorId(idVenta) ?? throw new Exception("La venta no existe");
        var cantidad = dto.Cantidad;
        var idItemVendible = dto.IdItemVendible;
        var precioUnitario = dto.PrecioUnitario;
        venta.AgregarDetalle(idItemVendible,cantidad,precioUnitario);

        await _unitOfWork.SaveChanges();
    }
}