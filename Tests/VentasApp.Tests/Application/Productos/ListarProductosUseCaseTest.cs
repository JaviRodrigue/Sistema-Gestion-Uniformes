using Xunit;
using Moq;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Productos;
using VentasApp.Application.CasoDeUso.Productos;
using VentasApp.Application.DTOs.Productos;
using FluentAssertions;

public class ListarProductoUseCaseTests
{
    [Fact]
    public async Task EjecutarAsync_DeberiaListarSoloProductosActivos()
    {
        var productos = new List<Producto>
        {
            new Producto(1, "A", 10, 20),
            new Producto(1, "B", 20, 30)
        };

        productos[1].Desactivar();

        var repo = new Mock<IProductoRepository>();
        repo.Setup(r => r.ListarProductos())
            .ReturnsAsync(productos);

        var useCase = new ListarProductoUseCase(repo.Object);

        var result = await useCase.EjecutarAsync();

        result.Should().HaveCount(1);
        result.First().Nombre.Should().Be("A");
    }
}
