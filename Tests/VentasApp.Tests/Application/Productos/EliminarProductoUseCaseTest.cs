using Xunit;
using Moq;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Productos;
using VentasApp.Application.CasoDeUso.Productos;
using VentasApp.Application.DTOs.Productos;
using FluentAssertions;

public class EliminarProductoUseCaseTests
{
    private readonly Mock<IProductoRepository> _repo = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    [Fact]
    public async Task EjecutarAsync_ProductoExiste_DeberiaDesactivar()
    {
        var producto = new Producto(1, "Pantalón", 100, 150);

        _repo.Setup(r => r.ObtenerProducto(1))
             .ReturnsAsync(producto);

        var useCase = new EliminarProductoUseCase(_repo.Object, _uow.Object);

        await useCase.EjecutarAsync(1);

        producto.Activo.Should().BeFalse();
        _uow.Verify(u => u.SaveChanges(), Times.Once);
    }
}
