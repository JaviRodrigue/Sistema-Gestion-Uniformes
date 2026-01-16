using VentasApp.Domain.Modelo.Pago;

namespace VentasApp.Application.Interfaces.Repositorios;

public interface IMedioPagoRepository
{
    public Task<MedioPago?> ObtenerPorId(int id);
}