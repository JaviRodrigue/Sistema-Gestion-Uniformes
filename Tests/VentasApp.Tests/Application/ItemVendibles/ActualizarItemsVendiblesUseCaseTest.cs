using Xunit;
using Moq;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Productos;
using VentasApp.Application.CasoDeUso.ItemVendibles;
using VentasApp.Application.DTOs.ItemVendible;
using FluentAssertions;


public class ActualizarItemVendibleUseCaseTests
{
    private readonly Mock<IItemVendibleRepository> _repo = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    [Fact]
    public async Task EjecutarAsync_ItemExiste_DeberiaActualizar()
    {
        var item = new ItemVendible(1, "Remera", "123", "M");

        _repo.Setup(r => r.ObtenerItem(1))
             .ReturnsAsync(item);

        var useCase = new ActualizarItemVendibleUseCase(_repo.Object, _uow.Object);

        await useCase.EjecutarAsync(1, new ActualizarItemVendibleDto
        {
            Nombre = "Remera nueva",
            CodigoBarra = "456",
            Talle = "L"
        });

        item.Nombre.Should().Be("remera nueva");
        _uow.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task EjecutarAsync_ItemNoExiste_DeberiaFallar()
    {
        _repo.Setup(r => r.ObtenerItem(1))
             .ReturnsAsync((ItemVendible?)null);

        var useCase = new ActualizarItemVendibleUseCase(_repo.Object, _uow.Object);

        await Assert.ThrowsAsync<Exception>(() =>
            useCase.EjecutarAsync(1, new ActualizarItemVendibleDto()));
    }
}
