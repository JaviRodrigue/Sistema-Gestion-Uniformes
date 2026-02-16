namespace VentasApp.Application.CasoDeUso.Venta;
using VentasApp.Application.Interfaces;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Venta;

public class ListarVentasUseCase
{
    private readonly IVentaRepository _ventaRepository;

    public ListarVentasUseCase(IVentaRepository ventaRepository)
    {
        _ventaRepository = ventaRepository;
    }

    public async Task<List<VentaResumenDto>> EjecutarAsync()
    {
        var ventas = await _ventaRepository.ObtenerTodas();

        return ventas.Select(v => new VentaResumenDto
        {
            Id = v.Id,
            Codigo = $"V-{v.Id:0000}",
            Fecha = v.FechaVenta,
            Total = v.MontoTotal,
            Restante = v.SaldoPendiente,
            EstadoVenta = v.Estado.ToString(),
            EstadoPago = v.SaldoPendiente == 0 ? "Pagado" : "Pendiente"
        }).ToList();
    }

}