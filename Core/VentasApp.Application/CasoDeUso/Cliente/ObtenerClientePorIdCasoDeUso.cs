using System;
using System.Linq;
using VentasApp.Application.DTOs.Cliente;
using VentasApp.Application.Interfaces.Repositorios;

namespace VentasApp.Application.CasoDeUso.Cliente;

public class ObtenerClientePorIdCasoDeUso(IClienteRepository repo)
{
    public readonly IClienteRepository _repository=repo;


    public async Task<BuscarClienteDTO> EjecutarAsync(int id)
    {
        var cliente = await _repository.ObtenerClientePorId(id)
            ?? throw new Exception("No se encontró el cliente");

        var dto = new BuscarClienteDTO
        {
            Id = cliente.Id,
            Nombre = cliente.Nombre,
            Instagram = cliente.Instagram,
            Telefonos = cliente.Telefonos?.Select(t => t.Numero).ToList(),
            FechaAlta = cliente.FechaAlta
        };

        return dto;
    }
}