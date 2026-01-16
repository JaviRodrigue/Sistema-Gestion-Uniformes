using System;
using VentasApp.Application.DTOs.Cliente;
using VentasApp.Application.Interfaces.Repositorios;

namespace VentasApp.Application.CasoDeUso.Telefono;

public class CrearTelefonoCasoDeUso
{
    public readonly ITelefonoRepository _repository;
    public readonly IUnitOfWork _unit;

    public CrearTelefonoCasoDeUso(ITelefonoRepository repo, IUnitOfWork unit)
    {
        _repository = repo;
        _unit = unit;
    }

    public async Task EjecutarAsync(AgregarTelefonoClienteDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Numero))
            throw new ArgumentException("El número de teléfono es obligatorio", nameof(dto.Numero));

        await _repository.AgregarTelefonos(dto.IdCliente, new List<string> { dto.Numero });
        await _unit.SaveChanges();
    }
}
