using System.Threading.Tasks;
using Moq;
using VentasApp.Application.CasoDeUso.Telefono;
using VentasApp.Application.Interfaces.Repositorios;
using Xunit;

namespace VentasApp.Tests.Application.TelefonoTest;

public class EliminarTelefonosDeClienteTest
{
    [Fact]
    public async Task EliminarTelefonosDeCliente_DeberiaDesactivarYGuardar()
    {
        var repoMock = new Mock<ITelefonoRepository>();
        var unitMock = new Mock<IUnitOfWork>();

        repoMock.Setup(r => r.DesactivarTelefonosPorClienteId(1)).Returns(Task.CompletedTask);
        unitMock.Setup(u => u.SaveChanges()).ReturnsAsync(1);

        var useCase = new EliminarTelefonosDeClienteCasoDeUso(repoMock.Object, unitMock.Object);

        await useCase.EjecutarAsync(1);

        repoMock.Verify(r => r.DesactivarTelefonosPorClienteId(1), Times.Once);
        unitMock.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task EliminarTelefonosDeCliente_SinTelefonos_NoFalla()
    {
        var repoMock = new Mock<ITelefonoRepository>();
        var unitMock = new Mock<IUnitOfWork>();

        repoMock.Setup(r => r.DesactivarTelefonosPorClienteId(2)).Returns(Task.CompletedTask);
        unitMock.Setup(u => u.SaveChanges()).ReturnsAsync(1);

        var useCase = new EliminarTelefonosDeClienteCasoDeUso(repoMock.Object, unitMock.Object);

        await useCase.EjecutarAsync(2);

        repoMock.Verify(r => r.DesactivarTelefonosPorClienteId(2), Times.Once);
        unitMock.Verify(u => u.SaveChanges(), Times.Once);
    }
}
