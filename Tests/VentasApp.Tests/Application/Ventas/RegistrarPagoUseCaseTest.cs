using Xunit;
using Moq;
using VentasApp.Application.CasoDeUso.Venta;
using VentasApp.Application.DTOs.Venta;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Venta;
using VentasApp.Domain.Enum;
using VentasApp.Domain.Modelo.Pago;
using VentasApp.Application.DTOs.Pago;
using FluentAssertions;

public class RegistrarPagoUseCaseTest
{
    private readonly Mock<IVentaRepository> _venta = new();
    private readonly Mock<IUnitOfWork> _unit = new ();


    [Fact]
public async Task RegistrarPago_DeberiaActualizarVentaYCrearPago()
{
    var venta = new Venta(TipoVenta.Presencial);
    venta.AgregarDetalle(1, 1, 100);

    var medio = new MedioPago("Efectivo", false);

    var repoVenta = new Mock<IVentaRepository>();
    var repoPago = new Mock<IPagoRepository>();
    var repoMedio = new Mock<IMedioPagoRepository>();
    var uow = new Mock<IUnitOfWork>();

    repoVenta.Setup(r => r.ObtenerPorId(1)).ReturnsAsync(venta);
    repoMedio.Setup(r => r.ObtenerPorId(1)).ReturnsAsync(medio);

    var useCase = new RegistrarPagoUseCase(
        repoVenta.Object,
        uow.Object,
        repoMedio.Object,
        repoPago.Object
    );

    var dto = new RegistrarPagoDto
    {
        EsSenia = false,
        Monto = 100,
        Metodos = new List<PagoMetodoDto>
        {
            new PagoMetodoDto { IdMedioPago = 1, Monto = 100 }
        }
    };

    await useCase.EjecutarAsync(1, dto);

    venta.MontoPagado.Should().Be(100);
    repoPago.Verify(r => r.Agregar(It.IsAny<Pago>()), Times.Once);
}


}