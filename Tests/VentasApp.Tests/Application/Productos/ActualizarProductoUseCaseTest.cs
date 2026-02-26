using Xunit;
using Moq;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Productos;
using VentasApp.Application.CasoDeUso.Productos;
using VentasApp.Application.DTOs.Productos;
using FluentAssertions;

public class ActualizarProductoUseCaseTests
{
    private readonly Mock<IProductoRepository> _repo = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    [Fact]
    public async Task EjecutarAsync_ProductoExiste_DeberiaActualizar()
    {
        var producto = new Producto(1, "Camisa", 50, 80);

        _repo.Setup(r => r.ObtenerProducto(1))
             .ReturnsAsync(producto);

        var useCase = new ActualizarProductoUseCase(_repo.Object, _uow.Object);

        await useCase.EjecutarAsync(1, new ActualizarProductoDto
        {
            Nombre = "Camisa nueva",
            Costo = 60,
            PrecioVenta = 90
        });

        producto.Nombre.Should().Be("Camisa nueva");
        producto.Costo.Should().Be(60);
        producto.PrecioVenta.Should().Be(90);

        _uow.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task EjecutarAsync_ProductoNoExiste_DeberiaFallar()
    {
        _repo.Setup(r => r.ObtenerProducto(1))
             .ReturnsAsync((Producto?)null);

        var useCase = new ActualizarProductoUseCase(_repo.Object, _uow.Object);

        await Assert.ThrowsAsync<Exception>(() =>
            useCase.EjecutarAsync(1, new ActualizarProductoDto()));
    }
}
