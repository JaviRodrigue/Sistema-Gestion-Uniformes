using System.Collections.Generic;
using System.Threading.Tasks;
using VentasApp.Domain.Modelo.Cliente;

namespace VentasApp.Application.Interfaces.Repositorios;

public interface IClienteRepository
{
    Task Agregar(Cliente cliente);
    Task<Cliente?> ObtenerClientePorId(int id);
    Task<Cliente?> ObtenerClientePorDni(long dni);
    Task<Cliente?> ObtenerClientePorTelefono(string telefono);
    Task<List<Cliente>> BuscarPorNombre(string nombre);
    Task<List<Cliente>> ListarClientes();
    Task Actualizar(Cliente cliente);
    Task Eliminar(int id);
}

