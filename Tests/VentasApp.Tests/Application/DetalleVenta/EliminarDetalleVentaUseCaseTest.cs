using Xunit;
using Moq;
using VentasApp.Application.CasoDeUso.DetalleVenta;
using VentasApp.Application.DTOs.DetalleVenta;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Venta;
using VentasApp.Domain.Modelo.Productos;
using VentasApp.Domain.Enum;

public class EliminarDetalleVentaUseCaseTests
{
    [Fact]
    public async Task EliminarDetalle_RemueveDetalle()
    {
        var venta = new Venta(TipoVenta.Presencial);
        venta.AgregarDetalle(1, 2, 100);

        var detalleId = venta.Detalles.First().Id;

        var repo = new Mock<IVentaRepository>();
        repo.Setup(r => r.ObtenerPorId(1)).ReturnsAsync(venta);

        var uow = new Mock<IUnitOfWork>();

        var useCase = new EliminarDetalleVentaUseCase(repo.Object, uow.Object);

        await useCase.Ejecutar(1, detalleId);

        Assert.Empty(venta.Detalles);
        uow.Verify(u => u.SaveChanges(), Times.Once);
    }
}
