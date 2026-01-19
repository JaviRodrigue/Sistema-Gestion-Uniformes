using System;
using VentasApp.Application.DTOs.Cliente;
using VentasApp.Application.Interfaces.Repositorios;


namespace VentasApp.Application.CasoDeUso.Telefono;
public class EliminarTelefonoCasoDeUso
{
    public readonly ITelefonoRepository _repository;
    public readonly IUnitOfWork _unit;

    public EliminarTelefonoCasoDeUso(ITelefonoRepository repo, IUnitOfWork unit)
    {
        _repository = repo;
        _unit = unit;
    }

    public async Task EjecutarAsync(int idTelefono)
    {
        var existe = await _repository.ObtenerTelefonoPorId(idTelefono)
            ?? throw new Exception("No se encontró el teléfono");

        // Eliminación lógica: desactivar el teléfono en vez de borrarlo físicamente
        await _repository.DesactivarTelefono(idTelefono);
        await _unit.SaveChanges();
    }
}
