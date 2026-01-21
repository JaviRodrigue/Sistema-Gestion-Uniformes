using Xunit;
using Moq;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Application.CasoDeUso.Stocks;
using VentasApp.Application.DTOs.Stocks;
using VentasApp.Domain.Modelo.Productos;
using FluentAssertions;



public class ObtenerStockUseCaseTests
{
    private readonly Mock<IStockRepository> _repo = new();

    [Fact]
    public async Task EjecutarAsync_StockExiste_DeberiaRetornarDto()
    {
        var stock = new Stock(1, 10, 2);

        _repo.Setup(r => r.ObtenerPorItemVendible(1))
             .ReturnsAsync(stock);

        var useCase = new ObtenerStockUseCase(_repo.Object);

        var dto = await useCase.EjecutarAsync(1);

        dto.Should().NotBeNull();
        dto!.CantidadDisponible.Should().Be(10);
    }

    [Fact]
    public async Task EjecutarAsync_StockNoExiste_DeberiaRetornarNull()
    {
        _repo.Setup(r => r.ObtenerPorItemVendible(1))
             .ReturnsAsync((Stock?)null);

        var useCase = new ObtenerStockUseCase(_repo.Object);

        var dto = await useCase.EjecutarAsync(1);

        dto.Should().BeNull();
    }
}
