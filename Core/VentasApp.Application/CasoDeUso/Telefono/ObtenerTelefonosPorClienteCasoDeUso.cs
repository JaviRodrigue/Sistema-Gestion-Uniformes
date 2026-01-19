using System.Collections.Generic;
using System.Threading.Tasks;
using VentasApp.Application.Interfaces.Repositorios;

namespace VentasApp.Application.CasoDeUso.Telefono;

public class ObtenerTelefonosPorClienteCasoDeUso
{
    public readonly ITelefonoRepository _repository;

    public ObtenerTelefonosPorClienteCasoDeUso(ITelefonoRepository repo)
    {
        _repository = repo;
    }

    public async Task<List<string>> EjecutarAsync(int clienteId)
    {
        return await _repository.ObtenerTelefonosPorClienteId(clienteId);
    }
}
