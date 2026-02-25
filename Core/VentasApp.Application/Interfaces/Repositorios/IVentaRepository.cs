namespace VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Venta;
public interface IVentaRepository
{
    Task<Venta?> ObtenerPorId(int id);
    Task<List<Venta>> ObtenerTodas();
    Task Agregar(Venta venta);
    Task Actualizar(Venta venta);
    Task VincularClienteVenta(int idVenta, int idCliente);
    Task<List<int>> ObtenerIdsVentasPorClientes(List<int> clientesIds);
    Task<string> ObtenerUltimoCodigoVenta();
    Task<bool> ExisteCodigoVenta(string codigo);
}