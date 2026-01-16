using System.Collections.Generic;
using System.Threading.Tasks;
using VentasApp.Domain.Modelo.Cliente;

namespace VentasApp.Application.Interfaces.Repositorios;

public interface ITelefonoRepository
{
    Task<List<string>> ObtenerTelefonosPorClienteId(int clienteId);
    Task AgregarTelefonos(int clienteId, List<string> telefonos);
    Task EliminarTelefonosPorClienteId(int clienteId);
}
