using Xunit;
using Moq;
using FluentAssertions;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Productos;
using VentasApp.Application.CasoDeUso.ItemVendibles;
using VentasApp.Application.DTOs.ItemVendible;


public class CrearItemVendibleUseCaseTests
{
    private readonly Mock<IItemVendibleRepository> _repo = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    [Fact]
    public async Task EjecutarAsync_DatosValidos_DeberiaCrearItem()
    {
        _repo.Setup(r => r.ObtenerItemPorCodigoBarra("123"))
             .ReturnsAsync((ItemVendible?)null);
        _repo.Setup(r => r.ExisteConNombreYTalle("Remera", "M"))
             .ReturnsAsync(false);

        var useCase = new CrearItemVendibleUseCase(_repo.Object, _uow.Object);

        var dto = new CrearItemVendibleDto
        {
            IdProducto = 1,
            nombre = "Remera",
            CodigoBarra = "123",
            Talle = "M"
        };

        var id = await useCase.EjecutarAsync(dto);

        _repo.Verify(r => r.Agregar(It.IsAny<ItemVendible>()), Times.Once);
        _uow.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task EjecutarAsync_CodigoDuplicado_DeberiaLanzarException()
    {
        _repo.Setup(r => r.ObtenerItemPorCodigoBarra("123"))
             .ReturnsAsync(new ItemVendible(1, "x", "123", null));

        var useCase = new CrearItemVendibleUseCase(_repo.Object, _uow.Object);

        await Assert.ThrowsAsync<Exception>(() =>
            useCase.EjecutarAsync(new CrearItemVendibleDto
            {
                IdProducto = 1,
                nombre = "Remera",
                CodigoBarra = "123"
            }));
    }

    [Fact]
    public async Task EjecutarAsync_NombreYTalleDuplicado_DeberiaLanzarException()
    {
        _repo.Setup(r => r.ObtenerItemPorCodigoBarra(It.IsAny<string>()))
             .ReturnsAsync((ItemVendible?)null);
        _repo.Setup(r => r.ExisteConNombreYTalle("Remera", "M"))
             .ReturnsAsync(true);

        var useCase = new CrearItemVendibleUseCase(_repo.Object, _uow.Object);
        var dto = new CrearItemVendibleDto
        {
            IdProducto = 1,
            nombre = "Remera",
            CodigoBarra = "456",
            Talle = "M"
        };

        var act = () => useCase.EjecutarAsync(dto);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("*Remera*M*");
    }

    [Fact]
    public async Task EjecutarAsync_MismoNombreTalleDiferente_DeberiaCrearItem()
    {
        _repo.Setup(r => r.ObtenerItemPorCodigoBarra(It.IsAny<string>()))
             .ReturnsAsync((ItemVendible?)null);
        _repo.Setup(r => r.ExisteConNombreYTalle("Remera", "L"))
             .ReturnsAsync(false);

        var useCase = new CrearItemVendibleUseCase(_repo.Object, _uow.Object);
        var dto = new CrearItemVendibleDto
        {
            IdProducto = 1,
            nombre = "Remera",
            CodigoBarra = "789",
            Talle = "L"
        };

        await useCase.EjecutarAsync(dto);

        _repo.Verify(r => r.Agregar(It.IsAny<ItemVendible>()), Times.Once);
        _uow.Verify(u => u.SaveChanges(), Times.Once);
    }
}

