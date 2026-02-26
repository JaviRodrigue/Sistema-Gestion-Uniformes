using VentasApp.Domain.Modelo.Pago;

namespace VentasApp.Application.Interfaces.Repositorios;

public interface IMedioPagoRepository
{
    Task<MedioPago?> ObtenerPorId(int id);
    Task<List<MedioPago>> ObtenerTodos();
    Task Agregar(MedioPago medioPago);
}