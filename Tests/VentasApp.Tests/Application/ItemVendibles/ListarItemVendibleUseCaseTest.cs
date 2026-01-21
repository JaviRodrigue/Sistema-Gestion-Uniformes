using Xunit;
using Moq;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Productos;
using VentasApp.Application.CasoDeUso.ItemVendibles;
using VentasApp.Application.DTOs.ItemVendible;
using FluentAssertions;


public class ListarItemVendibleUseCaseTests
{
    [Fact]
    public async Task EjecutarAsync_DeberiaListarSoloActivos()
    {
        var items = new List<ItemVendible>
        {
            new ItemVendible(1,"A","1",null),
            new ItemVendible(1,"B","2",null)
        };

        items[1].Desactivar();

        var repo = new Mock<IItemVendibleRepository>();
        repo.Setup(r => r.ListarItem(1))
            .ReturnsAsync(items);

        var useCase = new ListarItemVendibleUseCase(repo.Object);

        var result = await useCase.EjecutarAsync(1);

        result.Should().HaveCount(1);
    }
}
