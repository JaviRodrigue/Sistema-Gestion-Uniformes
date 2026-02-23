using VentasApp.Application.DTOs.DetalleVenta;
using VentasApp.Application.Interfaces.Repositorios;

namespace VentasApp.Application.CasoDeUso.DetalleVenta;

public class AgregarDetalleVentaUseCase
{
    private readonly IVentaRepository _repo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStockRepository _repositoryStock;

    public AgregarDetalleVentaUseCase(IVentaRepository repo, IUnitOfWork unit, IStockRepository repoStock)
    {
        _repo = repo;
        _unitOfWork = unit;
        _repositoryStock = repoStock;
    }

    public async Task EjecutarAsync(int idVenta, AgregarDetalleVentaDto dto)
    {
        //Verifico que venta exista
        var venta = await _repo.ObtenerPorId(idVenta) ?? throw new Exception("La venta no existe");

        //Veridico que se encuentre el stock
        var stock = await _repositoryStock.ObtenerPorItemVendible(dto.IdItemVendible) ?? throw new Exception("No se encontro el stock");

        //valido que la venta acepte el detalle
        //dentro de venta, se verificara el stock
        venta.AgregarDetalle(dto.IdItemVendible,dto.Cantidad,dto.PrecioUnitario);
        
        //Verifico el tipo de venta que se realizo, y a partir de ahi, descuento el stock
        if (venta.EsPedido)
        {
            stock.Reservar(dto.Cantidad);
        }
        else
        {
            stock.Descontar(dto.Cantidad);
        }

        
        await _unitOfWork.SaveChanges();
    }
}