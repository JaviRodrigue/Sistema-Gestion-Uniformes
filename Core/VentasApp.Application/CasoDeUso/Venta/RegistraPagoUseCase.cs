namespace VentasApp.Application.CasoDeUso.Venta;

using VentasApp.Application.DTOs.Pago;
using VentasApp.Application.Interfaces;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Venta;
using VentasApp.Domain.Modelo.Pago;

public class RegistrarPagoUseCase
{
    private readonly IVentaRepository _ventaRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMedioPagoRepository _medioPagoRepository;
    private readonly IPagoRepository _pagoRepository;

    public RegistrarPagoUseCase(IVentaRepository ventaRepository, IUnitOfWork unit, IMedioPagoRepository pago, IPagoRepository pag)
    {
        _ventaRepository = ventaRepository;
        _unitOfWork = unit;
        _medioPagoRepository = pago;
        _pagoRepository = pag;
    }

    public async Task EjecutarAsync(int idVenta, RegistrarPagoDto dto)
    {
        //Verifico que la venta exista
        var venta = await _ventaRepository.ObtenerPorId(idVenta) ?? throw new Exception("La venta no existe");

        if (!dto.Metodos.Any())
        {
            throw new Exception("Debe de especificar el metodo");
        }

        var pago = new Pago(venta.Id,dto.EsSenia);

        foreach(var metodo in dto.Metodos)
        {

            var medio = await _medioPagoRepository.ObtenerPorId(metodo.IdMedioPago) ?? throw new Exception("Medio de pago inexistente");
            if (!medio.Activo)
            {
                throw new Exception("El medio de pago no esta disponible");
            }

            pago.AgregarPago(medio.Id,metodo.Monto);
        }
        pago.ValidarPago();



        venta.RegistrarPago(dto.Monto);
        await _pagoRepository.Agregar(pago);
        await _unitOfWork.SaveChanges();
    }
}