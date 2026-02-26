using Xunit;
using Moq;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Application.CasoDeUso.Stocks;
using VentasApp.Application.DTOs.Stocks;
using VentasApp.Domain.Modelo.Productos;
using FluentAssertions;



public class AumentarStockUseCaseTests
{
    private readonly Mock<IStockRepository> _repo = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    [Fact]
    public async Task EjecutarAsync_StockExiste_DeberiaAumentarCantidad()
    {
        var stock = new Stock(1, 10, 2);

        _repo.Setup(r => r.ObtenerPorItemVendible(1))
             .ReturnsAsync(stock);

        var useCase = new AumentarStockUseCase(_repo.Object, _uow.Object);

        var dto = new ActualizarStockDto
        {
            Cantidad = 5
        };
        await useCase.EjecutarAsync(1, dto);

        stock.CantidadDisponible.Should().Be(15);
        _uow.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task EjecutarAsync_StockNoExiste_DeberiaLanzarException()
    {
        _repo.Setup(r => r.ObtenerPorItemVendible(1))
             .ReturnsAsync((Stock?)null);

        var useCase = new AumentarStockUseCase(_repo.Object, _uow.Object);

        var dto = new ActualizarStockDto
        {
            Cantidad = 5
        };

        await Assert.ThrowsAsync<Exception>(() =>
            useCase.EjecutarAsync(1, dto));
    }
}
