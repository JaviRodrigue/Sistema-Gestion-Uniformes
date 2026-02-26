using Xunit;
using Moq;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Application.DTOs.Categoria;
using VentasApp.Application.CasoDeUso.Categoria;
using VentasApp.Domain.Modelo.Categoria;
using FluentAssertions;



public class ListarCategoriaUseCaseTests
{
    private readonly Mock<ICategoriaRepository> _repo = new();

    [Fact]
    public async Task EjecutarAsync_DeberiaRetornarSoloCategoriasActivas()
    {
        _repo.Setup(r => r.ObtenerTodasCategorias())
             .ReturnsAsync(new List<Categoria>
             {
                 new("Ropa"),
                 new("Calzado") { }
             });

        var useCase = new ListarCategoriaUseCase(_repo.Object, Mock.Of<IUnitOfWork>());

        var result = await useCase.EjecutarAsync();

        result.Should().HaveCount(2);
    }
}
