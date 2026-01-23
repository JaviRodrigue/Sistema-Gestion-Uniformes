using Xunit;
using Moq;
using VentasApp.Application.CasoDeUso.Venta;
using VentasApp.Application.DTOs.Venta;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Venta;
using VentasApp.Domain.Enum;
using VentasApp.Domain.Modelo.Productos;
using FluentAssertions;

public class AnularVentaUseCaseTest
{
    private readonly Mock<IVentaRepository> _venta = new();
    private readonly Mock<IUnitOfWork> _unit = new ();

    [Fact]
    public async Task AnularVenta_Pedido_DeberiaLiberarReserva()
    {
        var venta = new Venta(TipoVenta.Pedido);
        venta.AgregarDetalle(1, 3, 100);

        var stock = new Stock(1, 10, 1);
        stock.Reservar(3);

        var repo = new Mock<IVentaRepository>();
        var stockRepo = new Mock<IStockRepository>();
        var uow = new Mock<IUnitOfWork>();

        repo.Setup(r => r.ObtenerPorId(1)).ReturnsAsync(venta);
        stockRepo.Setup(s => s.ObtenerPorItemVendible(1)).ReturnsAsync(stock);

        var useCase = new AnularVentaUseCase(repo.Object, uow.Object, stockRepo.Object);

        await useCase.EjecutarAsync(1);

        stock.CantidadDisponible.Should().Be(10);
        stock.CantidadReservada.Should().Be(0);
        venta.Estado.Should().Be(EstadoVenta.Cancelada);
    }

}