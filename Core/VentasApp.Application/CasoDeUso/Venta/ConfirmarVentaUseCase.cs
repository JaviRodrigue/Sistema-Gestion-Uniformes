namespace VentasApp.Application.CasoDeUso.Venta;
using VentasApp.Application.Interfaces.Repositorios;

public class ConfirmarVentaUseCase
{
    private readonly IVentaRepository _ventaRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStockRepository _repositoryStock;

    public ConfirmarVentaUseCase(IVentaRepository ventaRepository, IUnitOfWork unit, IStockRepository repo)
    {
        _ventaRepository = ventaRepository;
        _unitOfWork = unit;
        _repositoryStock = repo;
    }

    public async Task EjecutarAsync(int id)
    {
        //Verifico que la venta exista
        var venta = await _ventaRepository.ObtenerPorId(id) ?? throw new Exception("La venta no existe");

        if (venta.EsPedido)
        {
            foreach(var detalle in venta.Detalles)
            {
                var stock = await _repositoryStock.ObtenerPorItemVendible(detalle.IdItemVendible) ?? throw new Exception("No se encontro el stock");
                stock.ConfirmarReserva(detalle.Cantidad);
            }
        }
        venta.Confirmar();
        await _unitOfWork.SaveChanges();
    }
}