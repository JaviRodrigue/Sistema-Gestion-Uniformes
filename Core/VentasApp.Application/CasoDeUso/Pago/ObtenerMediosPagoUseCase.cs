using VentasApp.Application.DTOs.Productos;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Pago;

namespace VentasApp.Application.CasoDeUso.Pago;

public class ObtenerMediosPagoUseCase
{
    private readonly IMedioPagoRepository _medioPagoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ObtenerMediosPagoUseCase(IMedioPagoRepository medioPagoRepository, IUnitOfWork unitOfWork)
    {
        _medioPagoRepository = medioPagoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<ListadoProductoDto>> EjecutarAsync()
    {
        var medios = await _medioPagoRepository.ObtenerTodos();
        
        var needSave = false;
        if (!medios.Any(m => m.Nombre == "Efectivo")) 
        { 
            await _medioPagoRepository.Agregar(new MedioPago("Efectivo", false)); 
            needSave = true; 
        }
        if (!medios.Any(m => m.Nombre == "Tarjeta")) 
        { 
            await _medioPagoRepository.Agregar(new MedioPago("Tarjeta", true)); 
            needSave = true; 
        }
        if (!medios.Any(m => m.Nombre == "Transferencia")) 
        { 
            await _medioPagoRepository.Agregar(new MedioPago("Transferencia", false)); 
            needSave = true; 
        }
        
        if (needSave) 
        {
            await _unitOfWork.SaveChanges();
            medios = await _medioPagoRepository.ObtenerTodos();
        }

        return medios.Select(m => new ListadoProductoDto 
        { 
            Id = m.Id, 
            Nombre = m.Nombre 
        }).ToList();
    }
}
