namespace VentasApp.Application.CasoDeUso.ItemVendible;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Application.DTOs.ItemVendible;

public class ListarItemVendibleUseCase
{
    public IItemVendibleRepository _repository;

    public ListarItemVendibleUseCase(IItemVendibleRepository repo)
    {
        _repository = repo;
    }

    public async Task<List<ListadoItemVendibleDto>> EjecutarAsync(int idProducto)
    {
        //Traigo todos los items vendibles de un producto
        var items = await _repository.ListarItem(idProducto);
        //Mapeo la Dto de ItemVendible
        return items
                .Where(i => i.Activado)
                .Select(i => new ListadoItemVendibleDto
                {
                    Id = i.Id,
                    Nombre = i.Nombre,
                    CodigoBarra = i.CodigoBarra
                })
                .ToList();
    }
}