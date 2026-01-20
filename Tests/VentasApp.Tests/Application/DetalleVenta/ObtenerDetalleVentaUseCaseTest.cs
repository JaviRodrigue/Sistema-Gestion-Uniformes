using Xunit;
using Moq;
using VentasApp.Application.CasoDeUso.DetalleVenta;
using VentasApp.Application.DTOs.DetalleVenta;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Venta;
using VentasApp.Domain.Modelo.Producto;
using VentasApp.Domain.Enum;

public class ObtenerDetallesUseCaseTests
{
    [Fact]
    public async Task ObtenerDetalles_RetornaListaDto()
    {
        var venta = new Venta(TipoVenta.Presencial);
        venta.AgregarDetalle(1, 2, 100);

        var repo = new Mock<IVentaRepository>();
        repo.Setup(r => r.ObtenerPorId(1)).ReturnsAsync(venta);

        var useCase = new ObtenerDetallesUseCase(repo.Object);

        var result = await useCase.Ejecutar(1);

        Assert.Single(result);
        Assert.Equal(200, result.First().SubTotal);
    }
}
