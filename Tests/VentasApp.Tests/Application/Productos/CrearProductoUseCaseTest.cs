using Xunit;
using Moq;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Productos;
using VentasApp.Application.CasoDeUso.Productos;
using VentasApp.Application.DTOs.Productos;
using FluentAssertions;


public class CrearProductoUseCaseTests
{
    private readonly Mock<IProductoRepository> _repo = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    [Fact]
    public async Task EjecutarAsync_DatosValidos_DeberiaCrearProducto()
    {
        var useCase = new CrearProductoUseCase(_repo.Object, _uow.Object);

        var dto = new CrearProductoDto
        {
            IdCategoria = 1,
            Nombre = "Zapatilla",
            Costo = 100,
            PrecioVenta = 150
        };

        var id = await useCase.EjecutarAsync(dto);

        _repo.Verify(r => r.Agregar(It.IsAny<Producto>()), Times.Once);
        _uow.Verify(u => u.SaveChanges(), Times.Once);
    }
}
