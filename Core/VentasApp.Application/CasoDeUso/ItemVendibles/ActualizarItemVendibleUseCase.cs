namespace VentasApp.Application.CasoDeUso.ItemVendibles;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Application.DTOs.ItemVendible;

public class ActualizarItemVendibleUseCase
{
    public readonly IItemVendibleRepository _repository;
    public readonly IUnitOfWork _unit;

    public ActualizarItemVendibleUseCase(IItemVendibleRepository repo, IUnitOfWork unit)
    {
        _repository = repo;
        _unit = unit;
    }

    public async Task EjecutarAsync(int idItem, ActualizarItemVendibleDto dto)
    {
        //Verifico si el item existe
        var item = await _repository.ObtenerItem(idItem) ?? throw new Exception("No se encontro el Item Vendible");
        item.CambiarNombre(dto.Nombre);
        item.CambiarCodigoBarras(dto.CodigoBarra);
        item.CambiarTalle(dto.Talle);
        //guardo los cambios hechos
        await _unit.SaveChanges();
    }
}