namespace VentasApp.Application.CasoDeUso.Pago;
using VentasApp.Application.DTOs.Pago;
using VentasApp.Application.Interfaces.Repositorios;

public class ListarPagosVentaUseCase(IPagoRepository repo)
{
    private readonly IPagoRepository _repositoryPago=repo;
    

    public async Task<List<PagoDto>> EjecutarAsync(int idVenta)
    {
        var pagos = await _repositoryPago.ObtenerPorVenta(idVenta);


        //Dobleo mapeo en la dto
        return [.. pagos.Select(p => new PagoDto
        {
            Id = p.Id,
            IdVenta = p.IdVenta,
            FechaPago = p.FechaPago,
            Total = p.Total,
            Metodos = [.. p.Metodos.Select(m => new PagoMetodoDetalleDto
            {
                MedioPago = m.MedioPago.Nombre,
                Monto = m.Monto
            })]
        })];
    }
}