using System.Collections.Generic;
using System.Threading.Tasks;
using VentasApp.Domain.Modelo.Cliente;

namespace VentasApp.Application.Interfaces.Repositorios;

public interface IClienteRepository
{
    Task Agregar(Cliente cliente);
    Task<Cliente?> ObtenerClientePorVenta(int idVenta);
    Task<Cliente?> ObtenerClientePorId(int id);
    Task<Cliente?> ObtenerClientePorDni(string instagram);
    Task<Cliente?> ObtenerClientePorTelefono(string telefono);
    Task<List<Cliente>> BuscarPorNombre(string nombre);
    Task<List<Cliente>> BuscarPorInstagram(string instagram);
    Task<List<Cliente>> ListarClientes();
    Task Actualizar(Cliente cliente);
    Task Eliminar(int id);
    Task Desactivar(int id);
}

