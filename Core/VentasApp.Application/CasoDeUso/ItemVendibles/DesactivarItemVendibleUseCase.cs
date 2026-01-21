namespace VentasApp.Application.CasoDeUso.ItemVendibles;
using VentasApp.Application.Interfaces.Repositorios;

public class DesactivarItemVendibleUseCase
{
    public readonly IItemVendibleRepository _repository;
    public readonly IUnitOfWork _unit;

    public DesactivarItemVendibleUseCase(IItemVendibleRepository repo, IUnitOfWork unit)
    {
        _repository = repo;
        _unit = unit;
    }

    public async Task EjecutarAsync(int id)
    {
        //verifico si existe el item vendible
        var item = await _repository.ObtenerItem(id) ?? throw new Exception("No se encontro el Item Vendible");
        //hago un delete logico a item
        item.Desactivar();
        await _unit.SaveChanges();
    }
}