using Xunit;
using Moq;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Application.DTOs.Categoria;
using VentasApp.Application.CasoDeUso.Categoria;
using VentasApp.Domain.Modelo.Categoria;
using FluentAssertions;


public class ObtenerCategoriaUseCaseTests
{
    private readonly Mock<ICategoriaRepository> _repo = new();

    [Fact]
    public async Task EjecutarAsync_CategoriaExiste_DeberiaRetornarDto()
    {
        var categoria = new Categoria("Ropa");

        _repo.Setup(r => r.ObtenerCategoriaPorId(1))
             .ReturnsAsync(categoria);

        var useCase = new ObtenerCategoriaUseCase(_repo.Object);

        var dto = await useCase.EjecutarAsync(1);

        dto.Should().NotBeNull();
        dto!.Nombre.Should().Be("Ropa");
    }

    [Fact]
    public async Task EjecutarAsync_CategoriaNoExiste_DeberiaRetornarNull()
    {
        _repo.Setup(r => r.ObtenerCategoriaPorId(1))
             .ReturnsAsync((Categoria?)null);

        var useCase = new ObtenerCategoriaUseCase(_repo.Object);

        var dto = await useCase.EjecutarAsync(1);

        dto.Should().BeNull();
    }
}
