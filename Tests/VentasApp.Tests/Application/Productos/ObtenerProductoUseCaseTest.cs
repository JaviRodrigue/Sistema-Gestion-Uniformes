using Xunit;
using Moq;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Productos;
using VentasApp.Application.CasoDeUso.Productos;
using VentasApp.Application.DTOs.Productos;
using FluentAssertions;

public class ObtenerProductoUseCaseTests
{
    [Fact]
    public async Task EjecutarAsync_ProductoExiste_DeberiaRetornarDto()
    {
        var producto = new Producto(1, "Gorra", 30, 50);

        var repo = new Mock<IProductoRepository>();
        repo.Setup(r => r.ObtenerProducto(1))
            .ReturnsAsync(producto);

        var useCase = new ObtenerProductoUseCase(repo.Object);

        var dto = await useCase.EjecutarAsync(1);

        dto.Should().NotBeNull();
        dto!.Ganancia.Should().Be(20);
    }

    [Fact]
    public async Task EjecutarAsync_ProductoNoExiste_DeberiaRetornarNull()
    {
        var repo = new Mock<IProductoRepository>();
        repo.Setup(r => r.ObtenerProducto(1))
            .ReturnsAsync((Producto?)null);

        var useCase = new ObtenerProductoUseCase(repo.Object);

        var dto = await useCase.EjecutarAsync(1);

        dto.Should().BeNull();
    }
}
