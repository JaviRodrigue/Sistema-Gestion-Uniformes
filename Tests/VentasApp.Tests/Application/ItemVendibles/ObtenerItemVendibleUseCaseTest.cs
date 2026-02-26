using Xunit;
using Moq;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Productos;
using VentasApp.Application.CasoDeUso.ItemVendibles;
using VentasApp.Application.DTOs.ItemVendible;
using FluentAssertions;

public class ObtenerItemVendibleUseCaseTests
{
    [Fact]
    public async Task EjecutarAsync_ItemExiste_DeberiaRetornarDto()
    {
        var item = new ItemVendible(1,"Remera","123","M");

        var repo = new Mock<IItemVendibleRepository>();
        repo.Setup(r => r.ObtenerItem(1))
            .ReturnsAsync(item);

        var useCase = new ObtenerItemVendibleUseCase(repo.Object);

        var dto = await useCase.EjecutarAsync(1);

        dto.Should().NotBeNull();
        dto!.Nombre.Should().Be("remera");
    }

    [Fact]
    public async Task EjecutarAsync_ItemNoExiste_DeberiaRetornarNull()
    {
        var repo = new Mock<IItemVendibleRepository>();
        repo.Setup(r => r.ObtenerItem(1))
            .ReturnsAsync((ItemVendible?)null);

        var useCase = new ObtenerItemVendibleUseCase(repo.Object);

        var dto = await useCase.EjecutarAsync(1);

        dto.Should().BeNull();
    }
}
