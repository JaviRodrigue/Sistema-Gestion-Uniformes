namespace VentasApp.Application.CasoDeUso.Venta;

using VentasApp.Application.DTOs.Venta;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Base;

public class AgregarItemAVentaUseCase
{
    private readonly IVentaRepository _ventaRepository;
    private readonly IStockRepository _stockRepository;
    private readonly IItemVendibleRepository _itemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AgregarItemAVentaUseCase(IVentaRepository ventaRepository, IStockRepository stockRepository, IItemVendibleRepository itemRepository, IUnitOfWork unit)
    {
        _ventaRepository = ventaRepository;
        _stockRepository = stockRepository;
        _itemRepository = itemRepository;
        _unitOfWork = unit;
    }

    public async Task EjecutarAsync(int idVenta, AgregarDetalleDto dto)
    {
        //Primero busco si la venta existe, para poder agregar un item
        var venta = await _ventaRepository.ObtenerPorId(idVenta) ?? throw new Exception("La venta no existe");
        
        // Verificar stock disponible
        var stock = await _stockRepository.ObtenerPorItemVendible(dto.IdItemVendible);
        if (stock == null)
        {
            throw new ExcepcionDominio($"No se encontró stock para el producto seleccionado");
        }
        
        if (stock.CantidadDisponible < dto.Cantidad)
        {
            // Obtener nombre del producto para mensaje más claro
            var itemVendible = await _itemRepository.ObtenerItem(dto.IdItemVendible);
            var nombreProducto = itemVendible?.Nombre ?? "el producto";
            if (!string.IsNullOrWhiteSpace(itemVendible?.Talle))
            {
                nombreProducto = $"{nombreProducto} - Talle {itemVendible.Talle}";
            }
            
            throw new ExcepcionDominio($"Stock insuficiente para '{nombreProducto}'.\n\nDisponible: {stock.CantidadDisponible} unidades\nSolicitado: {dto.Cantidad} unidades\n\nPor favor, reduzca la cantidad o verifique el stock disponible.");
        }
        
        var cantidad = dto.Cantidad;
        var idItemVendible = dto.IdItemVendible;
        var precioUnitario = dto.PrecioUnitario;
        
        try
        {
            venta.AgregarDetalle(idItemVendible, cantidad, precioUnitario);
            // Descontar del stock
            stock.Descontar(cantidad);
            await _stockRepository.Actualizar(stock);
        }
        catch (ExcepcionDominio ex)
        {
            throw new ExcepcionDominio($"No se puede agregar el producto: {ex.Message}");
        }

        await _unitOfWork.SaveChanges();
    }
}