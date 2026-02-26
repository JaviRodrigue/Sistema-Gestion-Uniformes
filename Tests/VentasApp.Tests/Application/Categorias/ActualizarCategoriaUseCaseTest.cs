using Xunit;
using Moq;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Application.DTOs.Categoria;
using VentasApp.Application.CasoDeUso.Categoria;
using VentasApp.Domain.Modelo.Categoria;
using FluentAssertions;


public class ActualizarCategoriaUseCaseTests
{
    private readonly Mock<ICategoriaRepository> _repo = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    [Fact]
    public async Task Ejecutar_CategoriaExiste_DeberiaActualizarNombre()
    {
        var categoria = new Categoria("Viejo");

        _repo.Setup(r => r.ObtenerCategoriaPorId(1))
             .ReturnsAsync(categoria);

        var useCase = new ActualizarCategoriaUseCase(_repo.Object, _uow.Object);

        await useCase.EjecutarAsync(1, new ActualizarCategoriaDto { Nombre = "Nuevo" });

        categoria.Nombre.Should().Be("Nuevo");
        _uow.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task Ejecutar_CategoriaNoExiste_DeberiaLanzarException()
    {
        _repo.Setup(r => r.ObtenerCategoriaPorId(1))
             .ReturnsAsync((Categoria?)null);

        var useCase = new ActualizarCategoriaUseCase(_repo.Object, _uow.Object);

        await Assert.ThrowsAsync<Exception>(() =>
            useCase.EjecutarAsync(1, new ActualizarCategoriaDto { Nombre = "Test" }));
    }
}
