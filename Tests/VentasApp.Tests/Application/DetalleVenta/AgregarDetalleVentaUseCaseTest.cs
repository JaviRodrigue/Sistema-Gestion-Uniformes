using Xunit;
using Moq;
using VentasApp.Application.CasoDeUso.DetalleVenta;
using VentasApp.Application.DTOs.DetalleVenta;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Venta;
using VentasApp.Domain.Modelo.Productos;
using VentasApp.Domain.Enum;
using VentasApp.Domain.Base;

public class AgregarDetalleVentaUseCaseTests
{
    [Fact]
    public async Task VentaNormal_DescuentaStockDisponible()
    {
        // Arrange
        var venta = new Venta(TipoVenta.Presencial);

        var stock = new Stock(
            IdItemVendible: 1,
            cantidadInicial: 10,
            stockMinimo: 2
        );

        var ventaRepo = new Mock<IVentaRepository>();
        ventaRepo.Setup(r => r.ObtenerPorId(1)).ReturnsAsync(venta);

        var stockRepo = new Mock<IStockRepository>();
        stockRepo
            .Setup(r => r.ObtenerPorItemVendible(1))
            .ReturnsAsync(stock);

        var uow = new Mock<IUnitOfWork>();

        var useCase = new AgregarDetalleVentaUseCase(
            ventaRepo.Object,
            uow.Object,
            stockRepo.Object
        );

        var dto = new AgregarDetalleVentaDto
        {
            IdItemVendible = 1,
            Cantidad = 3,
            PrecioUnitario = 100
        };

        // Act
        await useCase.EjecutarAsync(1, dto);

        // Assert
        Assert.Equal(7, stock.CantidadDisponible);
        Assert.Equal(0, stock.CantidadReservada);
        Assert.Single(venta.Detalles);

        uow.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
public async Task VentaPedido_ReservaStock()
{
    // Arrange
    var venta = new Venta(TipoVenta.Pedido);

    var stock = new Stock(
        IdItemVendible: 1,
        cantidadInicial: 10,
        stockMinimo: 2
    );

    var ventaRepo = new Mock<IVentaRepository>();
    ventaRepo.Setup(r => r.ObtenerPorId(1)).ReturnsAsync(venta);

    var stockRepo = new Mock<IStockRepository>();
    stockRepo
        .Setup(r => r.ObtenerPorItemVendible(1))
        .ReturnsAsync(stock);

    var uow = new Mock<IUnitOfWork>();

    var useCase = new AgregarDetalleVentaUseCase(
        ventaRepo.Object,
        uow.Object,
        stockRepo.Object
    );

    var dto = new AgregarDetalleVentaDto
    {
        IdItemVendible = 1,
        Cantidad = 4,
        PrecioUnitario = 200
    };

    // Act
    await useCase.EjecutarAsync(1, dto);

    // Assert
    Assert.Equal(6, stock.CantidadDisponible);
    Assert.Equal(4, stock.CantidadReservada);
}
[Fact]
public async Task NoPermiteAgregarDetalle_SiStockInsuficiente()
{
    var venta = new Venta(TipoVenta.Presencial);

    var stock = new Stock(
        IdItemVendible: 1,
        cantidadInicial: 2,
        stockMinimo: 1
    );

    var ventaRepo = new Mock<IVentaRepository>();
    ventaRepo.Setup(r => r.ObtenerPorId(1)).ReturnsAsync(venta);

    var stockRepo = new Mock<IStockRepository>();
    stockRepo.Setup(r => r.ObtenerPorItemVendible(1)).ReturnsAsync(stock);

    var uow = new Mock<IUnitOfWork>();

    var useCase = new AgregarDetalleVentaUseCase(
        ventaRepo.Object,
        uow.Object,
        stockRepo.Object
    );

    var dto = new AgregarDetalleVentaDto
    {
        IdItemVendible = 1,
        Cantidad = 5,
        PrecioUnitario = 100
    };

    await Assert.ThrowsAsync<ExcepcionDominio>(() =>
        useCase.EjecutarAsync(1, dto)
    );
}

}
