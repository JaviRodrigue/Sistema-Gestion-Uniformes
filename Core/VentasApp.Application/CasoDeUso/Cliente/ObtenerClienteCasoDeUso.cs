using System;
using System.Linq;
using VentasApp.Application.DTOs.Cliente;
using VentasApp.Application.Interfaces.Repositorios;

namespace VentasApp.Application.CasoDeUso.Cliente;

public class ObtenerClientePorIdCasoDeUso
{
    public readonly IClienteRepository _repository;

    public ObtenerClientePorIdCasoDeUso(IClienteRepository repo)
    {
        _repository = repo;
    }

    public async Task<BuscarClienteDTO> EjecutarAsync(int id)
    {
        var cliente = await _repository.ObtenerClientePorId(id)
            ?? throw new Exception("No se encontró el cliente");

        var dto = new BuscarClienteDTO
        {
            Id = cliente.Id,
            Nombre = cliente.Nombre,
            Telefono = cliente.Telefonos?.FirstOrDefault()?.Numero,
        };

        if (long.TryParse(cliente.DNI, out var dni))
            dto.Dni = dni;
        else
            dto.Dni = null;

        return dto;
    }
}