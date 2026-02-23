using Xunit;
using Moq;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Application.CasoDeUso.Stocks;
using VentasApp.Application.DTOs.Stocks;
using VentasApp.Domain.Modelo.Productos;
using FluentAssertions;



public class ReservarStockUseCaseTests
{
    private readonly Mock<IStockRepository> _repo = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    [Fact]
    public async Task EjecutarAsync_StockSuficiente_DeberiaReservar()
    {
        var stock = new Stock(1, 10, 2);

        _repo.Setup(r => r.ObtenerPorItemVendible(1))
             .ReturnsAsync(stock);

        var useCase = new ReservarStockUseCase(_repo.Object, _uow.Object);

        var dto = new ReservarStockDto
        {
            Cantidad = 3
        };

        await useCase.EjecutarAsync(1, dto);

        stock.CantidadDisponible.Should().Be(7);
        stock.CantidadReservada.Should().Be(3);
        _uow.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task EjecutarAsync_StockNoExiste_DeberiaFallar()
    {
        _repo.Setup(r => r.ObtenerPorItemVendible(1))
             .ReturnsAsync((Stock?)null);

        var useCase = new ReservarStockUseCase(_repo.Object, _uow.Object);

        var dto = new ReservarStockDto
        {
            Cantidad = 1
        };
        await Assert.ThrowsAsync<Exception>(() =>
            useCase.EjecutarAsync(1, dto));
    }
}
