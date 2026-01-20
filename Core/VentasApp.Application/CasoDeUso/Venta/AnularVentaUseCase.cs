namespace VentasApp.Application.CasoDeUso.Venta;
using VentasApp.Application.Interfaces.Repositorios;


public class AnularVentaUseCase
{
    private readonly IVentaRepository _ventaRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStockRepository _repositoryStock;

    public AnularVentaUseCase(IVentaRepository ventaRepository, IUnitOfWork unit, IStockRepository repo)
    {
        _ventaRepository = ventaRepository;
        _unitOfWork = unit;
        _repositoryStock = repo;
    }

    public async Task EjecutarAsync(int idVenta)
    {
        //Verifico que venta exista
        var venta = await _ventaRepository.ObtenerPorId(idVenta) ?? throw new Exception("La venta no existe");
        //Realizo un delete logico en venta
        venta.AnularVenta();
        foreach(var detalle in venta.Detalles)
        {
            var stock = await _repositoryStock.ObtenerPorItemVendible(detalle.IdItemVendible) ?? throw new Exception("Stock no encontrado");

            //Verifico si la venta era pedido
            if (venta.EsPedido)
            {
                stock.LiberarReserva(detalle.Cantidad);
            }
            else
            {
                stock.Aumentar(detalle.Cantidad);
            }
        }
        await _unitOfWork.SaveChanges();
    }
}