using Xunit;
using Moq;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Application.DTOs.Categoria;
using VentasApp.Application.CasoDeUso.Categoria;
using VentasApp.Domain.Modelo.Categoria;




public class AgregarCategoriaUseCaseTests
{
    private readonly Mock<ICategoriaRepository> _repo = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    [Fact]
    public async Task Ejecutar_CategoriaValida_DeberiaPersistir()
    {
        var useCase = new AgregarCategoriaUseCase(_repo.Object, _uow.Object);

        var dto = new CrearCategoriaDto { Nombre = "Ropa" };

        await useCase.Ejecutar(dto);

        _repo.Verify(r => r.Agregar(It.IsAny<Categoria>()), Times.Once);
        _uow.Verify(u => u.SaveChanges(), Times.Once);
    }
}
