namespace VentasApp.Application.CasoDeUso.ItemVendible;


using VentasApp.Application.DTOs.ItemVendible;
using VentasApp.Application.Interfaces.Repositorios;

public class ObtenerItemVendibleUseCase
{
    public readonly IItemVendibleRepository _repository;

    public ObtenerItemVendibleUseCase(IItemVendibleRepository repo)
    {
        _repository = repo;
    }

    public async Task<ItemVendibleDto?> EjecutarAsync(int id)
    {
        var item = await _repository.ObtenerItem(id);
        if(item == null) return null;

        return new ItemVendibleDto
        {
            Id=item.Id,
            IdProducto = item.IdProducto,
            Nombre = item.Nombre,
            CodigoBarra = item.CodigoBarra,
            Talle = item.Talle,
            Activo = item.Activado
        };
        
    }
}