using Xunit;
using Moq;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Application.CasoDeUso.Stocks;
using VentasApp.Application.DTOs.Stocks;
using VentasApp.Domain.Modelo.Productos;
using FluentAssertions;



public class ConfirmarReservaStockUseCaseTests
{
    private readonly Mock<IStockRepository> _repo = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    [Fact]
    public async Task EjecutarAsync_ReservaValida_DeberiaConfirmar()
    {
        var stock = new Stock(1, 10, 2);
        stock.Reservar(5);

        _repo.Setup(r => r.ObtenerPorItemVendible(1))
             .ReturnsAsync(stock);

        var useCase = new ReservarStockUseCase(_repo.Object, _uow.Object);
        var dto = new ReservarStockDto
        {
            Cantidad = 3
        };
        await useCase.EjecutarAsync(1,dto);
        
        stock.CantidadReservada.Should().Be(8);
        stock.CantidadDisponible.Should().Be(2);
        _uow.Verify(u => u.SaveChanges(), Times.Once);
    }
}
