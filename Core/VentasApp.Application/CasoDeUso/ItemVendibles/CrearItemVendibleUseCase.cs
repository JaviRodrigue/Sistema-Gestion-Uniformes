using VentasApp.Domain.Modelo.Productos;
using VentasApp.Application.DTOs.ItemVendible;
using VentasApp.Application.Interfaces.Repositorios;
namespace VentasApp.Application.CasoDeUso.ItemVendibles;

public class CrearItemVendibleUseCase
{
    public readonly IItemVendibleRepository _repository;
    public readonly IUnitOfWork _unit;

    public CrearItemVendibleUseCase(IItemVendibleRepository repo, IUnitOfWork unit)
    {
        _repository = repo;
        _unit = unit;
    }

    public async Task<int> EjecutarAsync(CrearItemVendibleDto dto)
    {
        var existe = await _repository.ObtenerItemPorCodigoBarra(dto.CodigoBarra);
        if(existe != null)
        {
            throw new Exception("Ya existe un item con ese codigo de barra");
        }

        var item = new ItemVendible(
            dto.IdProducto,
            dto.nombre,
            dto.CodigoBarra,
            dto.Talle
        );

        await _repository.Agregar(item);
        await _unit.SaveChanges();
        return item.Id;
    }
}