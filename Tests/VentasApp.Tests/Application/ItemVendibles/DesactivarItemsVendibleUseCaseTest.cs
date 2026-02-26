using Xunit;
using Moq;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Productos;
using VentasApp.Application.CasoDeUso.ItemVendibles;
using VentasApp.Application.DTOs.ItemVendible;
using FluentAssertions;


public class DesactivarItemVendibleUseCaseTests
{
    private readonly Mock<IItemVendibleRepository> _repo = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    [Fact]
    public async Task EjecutarAsync_ItemExiste_DeberiaDesactivar()
    {
        var item = new ItemVendible(1,"Remera","123",null);

        _repo.Setup(r => r.ObtenerItem(1))
             .ReturnsAsync(item);

        var useCase = new DesactivarItemVendibleUseCase(_repo.Object, _uow.Object);

        await useCase.EjecutarAsync(1);

        item.Activado.Should().BeFalse();
        _uow.Verify(u => u.SaveChanges(), Times.Once);
    }
}
