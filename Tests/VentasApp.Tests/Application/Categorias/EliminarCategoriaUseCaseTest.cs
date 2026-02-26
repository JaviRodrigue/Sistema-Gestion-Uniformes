using Xunit;
using Moq;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Application.DTOs.Categoria;
using VentasApp.Application.CasoDeUso.Categoria;
using VentasApp.Domain.Modelo.Categoria;
using FluentAssertions;



public class EliminarCategoriaUseCaseTests
{
    private readonly Mock<ICategoriaRepository> _repo = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    [Fact]
    public async Task Ejecutar_CategoriaExiste_DeberiaDesactivar()
    {
        var categoria = new Categoria("Accesorios");

        _repo.Setup(r => r.ObtenerCategoriaPorId(1))
             .ReturnsAsync(categoria);

        var useCase = new EliminarCategoriaUseCase(_repo.Object, _uow.Object);

        await useCase.EjecutarAsync(1);

        categoria.Activa.Should().BeFalse();
        _uow.Verify(u => u.SaveChanges(), Times.Once);
    }
}
