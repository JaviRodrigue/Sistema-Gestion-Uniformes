using Xunit;
using Moq;
using VentasApp.Application.CasoDeUso.DetalleVenta;
using VentasApp.Application.DTOs.DetalleVenta;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Venta;
using VentasApp.Domain.Modelo.Producto;
using VentasApp.Domain.Enum;
using VentasApp.Application.DTOs.Venta;

public class ModificarDetalleUseCaseTests
{
    [Fact]
    public async Task ModificarDetalle_ActualizaCantidadYPrecio()
    {
        var venta = new Venta(TipoVenta.Presencial);
        venta.AgregarDetalle(1, 1, 100);

        var detalle = venta.Detalles.First();

        var repo = new Mock<IVentaRepository>();
        repo.Setup(r => r.ObtenerPorId(1)).ReturnsAsync(venta);

        var uow = new Mock<IUnitOfWork>();

        var useCase = new ModificarDetalleUseCase(repo.Object, uow.Object);

        var dto = new ModificarDetalleVentaDto
        {
            Cantidad = 3,
            PrecioUnitario = 150
        };

        await useCase.Ejecutar(1, detalle.Id, dto);

        Assert.Equal(450, venta.MontoTotal);
    }
}
