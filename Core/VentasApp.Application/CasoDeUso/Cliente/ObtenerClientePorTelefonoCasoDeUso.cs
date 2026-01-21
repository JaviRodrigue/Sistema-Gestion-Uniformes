using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasApp.Application.DTOs.Cliente;
using VentasApp.Application.Interfaces.Repositorios;

namespace VentasApp.Application.CasoDeUso.Cliente;
public class ObtenerClientePorTelefonoCasoDeUso(IClienteRepository repo)
{
    public readonly IClienteRepository _repository = repo;
    public async Task<BuscarClienteDTO> EjecutarAsync(string telefono)
    {
        var cliente = await _repository.ObtenerClientePorTelefono(telefono)
            ?? throw new Exception("No se encontró el cliente, telefono inexistente");

        var dto = new BuscarClienteDTO
        {
            Id = cliente.Id,
            Nombre = cliente.Nombre,
            Dni = cliente.DNI,
            Telefonos = cliente.Telefonos?.Select(t => t.Numero).ToList(),
            FechaAlta = cliente.FechaAlta
        };

        return dto;
    }
}
