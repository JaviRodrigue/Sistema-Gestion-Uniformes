using System;
using System.Linq;
using System.Collections.Generic;
using VentasApp.Application.DTOs.Cliente;
using VentasApp.Application.Interfaces.Repositorios;

namespace VentasApp.Application.CasoDeUso.Cliente;

public class ObtenerTodosClientesCasoDeUso(IClienteRepository repo)
{
    public readonly IClienteRepository _repository=repo;


    public async Task<List<BuscarClienteDTO>> EjecutarAsync()
    {
        var clientes = await _repository.ListarClientes();

        var listaDto = clientes
            .Select(c =>
            {
                var dto = new BuscarClienteDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Instagram = c.Instagram,
                    Telefonos = c.Telefonos?.Select(t => t.Numero).ToList(),
                    FechaAlta = c.FechaAlta
                };

                return dto;
            })
            .ToList();

        return listaDto;
    }
}
