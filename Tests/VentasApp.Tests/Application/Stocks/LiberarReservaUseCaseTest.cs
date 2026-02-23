using Xunit;
using Moq;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Application.CasoDeUso.Stocks;
using VentasApp.Application.DTOs.Stocks;
using VentasApp.Domain.Modelo.Productos;
using FluentAssertions;


public class LiberarReservaStockUseCaseTests
{
    private readonly Mock<IStockRepository> _repo = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    [Fact]
    public async Task EjecutarAsync_ReservaExistente_DeberiaLiberar()
    {
        var stock = new Stock(1, 10, 2);
        stock.Reservar(4);

        _repo.Setup(r => r.ObtenerPorItemVendible(1))
             .ReturnsAsync(stock);

        var useCase = new LiberarReservaUseCase(_repo.Object, _uow.Object);

        await useCase.EjecutarAsync(1, 2);

        stock.CantidadReservada.Should().Be(2);
        stock.CantidadDisponible.Should().Be(8);
        _uow.Verify(u => u.SaveChanges(), Times.Once);
    }
}

