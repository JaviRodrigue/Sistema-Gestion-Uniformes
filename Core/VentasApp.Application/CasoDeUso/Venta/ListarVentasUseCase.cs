namespace VentasApp.Application.CasoDeUso.Venta;
using VentasApp.Application.DTOs.Venta;
using VentasApp.Application.Interfaces;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Venta;

public class ListarVentasUseCase
{
    private readonly IVentaRepository _ventaRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IPagoRepository _pagoRepository;

    public ListarVentasUseCase(IVentaRepository ventaRepository, IClienteRepository clienteRepository, IPagoRepository pagoRepository)
    {
        _ventaRepository = ventaRepository;
        _clienteRepository = clienteRepository;
        _pagoRepository = pagoRepository;
    }

    public async Task<List<VentaResumenDto>> EjecutarAsync()
    {
        var ventas = await _ventaRepository.ObtenerTodas();
        var resultado = new List<VentaResumenDto>();

        foreach (var v in ventas)
        {
            var cliente = await _clienteRepository.ObtenerClientePorVenta(v.Id);
            var pagos = await _pagoRepository.ObtenerPorVenta(v.Id);
            var todosVerificados = pagos.Any() && pagos.All(p => p.Verificado);
            
            resultado.Add(new VentaResumenDto
            {
                Id = v.Id,
                Codigo = v.CodigoVenta,
                Fecha = v.FechaVenta,
                Cliente = cliente?.Nombre ?? "Sin cliente",
                Total = v.MontoTotal,
                Restante = v.SaldoPendiente,
                EstadoVenta = v.Estado.ToString(),
                EstadoPago = v.SaldoPendiente <= 0.01m ? "Pagada" : "Pendiente",
                TodosLosPagosVerificados = todosVerificados
            });
        }

        // Ordenar por codigo (ID) descendente - las mas recientes primero
        return resultado.OrderByDescending(v => v.Id).ToList();
    }

}