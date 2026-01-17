using System;
using System.Linq;
using System.Collections.Generic;
using VentasApp.Application.DTOs.Cliente;
using VentasApp.Application.Interfaces.Repositorios;

namespace VentasApp.Application.CasoDeUso.Cliente;

public class ObtenerTodosClientesCasoDeUso
{
    public readonly IClienteRepository _repository;

    public ObtenerTodosClientesCasoDeUso(IClienteRepository repo)
    {
        _repository = repo;
    }

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
                    Telefono = c.Telefonos?.FirstOrDefault()?.Numero
                };

                if (long.TryParse(c.DNI, out var dni))
                    dto.Dni = dni;
                else
                    dto.Dni = null;

                return dto;
            })
            .ToList();

        return listaDto;
    }
}
