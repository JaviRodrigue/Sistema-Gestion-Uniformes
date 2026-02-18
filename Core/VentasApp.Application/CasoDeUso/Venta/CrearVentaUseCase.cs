namespace VentasApp.Application.CasoDeUso.Venta;

using VentasApp.Application.DTOs.Venta;
using VentasApp.Application.Interfaces.Repositorios;

using VentasApp.Domain.Modelo.Venta;

//Caso de uso para crar una venta vacia
public class CrearVentaUseCase
{
    private readonly IVentaRepository _ventaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CrearVentaUseCase(IVentaRepository ventaRepository, IUnitOfWork unitOfWork)
    {
        _ventaRepository = ventaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> EjecutarAsync(CrearVentaDto VentaDto)
    {
        var venta = new Venta(VentaDto.TipoVenta);
        await _ventaRepository.Agregar(venta);
        try
        {
            await _unitOfWork.SaveChanges();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            throw;
        }
        return venta.Id;
    }
}