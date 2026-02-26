using Xunit;
using Moq;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Application.CasoDeUso.Stocks;
using VentasApp.Application.DTOs.Stocks;
using VentasApp.Domain.Modelo.Productos;
using FluentAssertions;



public class CrearStockUseCaseTests
{
    private readonly Mock<IStockRepository> _repo = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    [Fact]
    public async Task EjecutarAsync_DatosValidos_DeberiaCrearStock()
    {
        var useCase = new CrearStockUseCase(_repo.Object, _uow.Object);

        var dto = new CrearStockDto
        {
            IdItemVendible = 1,
            CantidadInicial = 10,
            StockMinimo = 2
        };

        await useCase.EjecutarAsync(dto);

        _repo.Verify(r => r.Agregar(It.IsAny<Stock>()), Times.Once);
        _uow.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task EjecutarAsync_StockDuplicado_DeberiaLanzarException()
    {
        _repo.Setup(r => r.ObtenerPorItemVendible(1))
             .ReturnsAsync(new Stock(1, 10, 2));

        var useCase = new CrearStockUseCase(_repo.Object, _uow.Object);

        await Assert.ThrowsAsync<Exception>(() =>
            useCase.EjecutarAsync(new CrearStockDto
            {
                IdItemVendible = 1,
                CantidadInicial = 5,
                StockMinimo = 1
            }));
    }
}
