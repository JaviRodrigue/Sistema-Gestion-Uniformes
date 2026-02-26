using Xunit;
using Moq;
using VentasApp.Application.CasoDeUso.Venta;
using VentasApp.Application.DTOs.Venta;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Venta;
using VentasApp.Domain.Enum;
public class CrearVentaUseCaseTest
{
    private readonly Mock<IVentaRepository> _venta = new();
    private readonly Mock<IUnitOfWork> _unit = new ();
    [Fact]
    public async Task CrearVenta_DeberiaCrearVentaPendiente()
    {
        var repo = new Mock<IVentaRepository>();
        var uow = new Mock<IUnitOfWork>();

        repo.Setup(r => r.Agregar(It.IsAny<Venta>()))
            .Returns(Task.CompletedTask);

        uow.Setup(u => u.SaveChanges()).ReturnsAsync(1);

        var useCase = new CrearVentaUseCase(repo.Object, uow.Object);

        var id = await useCase.EjecutarAsync(new CrearVentaDto
        {
            TipoVenta = TipoVenta.Presencial
        });

        repo.Verify(r => r.Agregar(It.IsAny<Venta>()), Times.Once);
        uow.Verify(u => u.SaveChanges(), Times.Once);
    }
}
