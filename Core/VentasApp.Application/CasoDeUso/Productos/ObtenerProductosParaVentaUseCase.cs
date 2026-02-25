using VentasApp.Application.DTOs.Productos;
using VentasApp.Application.Interfaces.Repositorios;

namespace VentasApp.Application.CasoDeUso.Productos;

public class ObtenerProductosParaVentaUseCase
{
    private readonly IItemVendibleRepository _itemVendibleRepository;

    public ObtenerProductosParaVentaUseCase(IItemVendibleRepository itemVendibleRepository)
    {
        _itemVendibleRepository = itemVendibleRepository;
    }

    public async Task<List<ListadoProductoDto>> EjecutarAsync()
    {
        var items = await _itemVendibleRepository.ObtenerTodosConProductoYStock();
        
        var productos = items.Select(iv => new ListadoProductoDto
        {
            Id = iv.Id,
            Nombre = iv.Producto != null 
                ? (string.IsNullOrWhiteSpace(iv.Talle) 
                    ? iv.Producto.Nombre 
                    : $"{iv.Producto.Nombre} - Talle {iv.Talle}")
                : iv.CodigoBarra,
            CodigoBarra = iv.CodigoBarra,
            PrecioVenta = iv.Producto != null ? iv.Producto.PrecioVenta : 0m,
            StockDisponible = iv.Stock?.CantidadDisponible ?? 0
        }).ToList();

        return productos;
    }
}
