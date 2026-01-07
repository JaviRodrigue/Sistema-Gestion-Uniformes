using VentasApp.Application.DTOs.DetalleVenta;
using VentasApp.Application.Interfaces.Repositorios;

namespace VentasApp.Application.CasoDeUso.DetalleVenta;

public class ObtenerDetallesUseCase
{
    private readonly IVentaRepository _repo;

    public ObtenerDetallesUseCase(IVentaRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<DetalleVentaDto>> Ejecutar(int idVenta)
    {
        //Vetifico que venta exista
        var venta = await _repo.ObtenerPorId(idVenta) ?? throw new Exception("Venta no encontada");

        //Mapeando la Dto de DetalleVenta
        return venta.Detalles.Select(d => new DetalleVentaDto
        {
            Id = d.Id,
            IdItemVendible = d.IdItemVendible,
            Cantidad = d.Cantidad,
            PrecioUnitario = d.PrecioUnitario,
            SubTotal = d.SubTotal
        }).ToList();
    }
}