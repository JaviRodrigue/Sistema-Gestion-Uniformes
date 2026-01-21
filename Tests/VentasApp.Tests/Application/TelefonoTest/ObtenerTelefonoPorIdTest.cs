using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using VentasApp.Application.CasoDeUso.Telefono;
using VentasApp.Application.Interfaces.Repositorios;
using Xunit;

namespace VentasApp.Tests.Application.TelefonoTest;

public class ObtenerTelefonoPorIdTest
{
    [Fact]
    public async Task ObtenerTelefonoPorId_DeberiaRetornarNumero()
    {
        var repoMock = new Mock<ITelefonoRepository>();
        repoMock.Setup(r => r.ObtenerTelefonoPorId(1)).ReturnsAsync("123");

        var useCase = new ObtenerTelefonoPorIdCasoDeUso(repoMock.Object);

        var resultado = await useCase.EjecutarAsync(1);

        resultado.Should().Be("123");
    }

    [Fact]
    public async Task ObtenerTelefonoPorId_NoExiste_DeberiaLanzar()
    {
        var repoMock = new Mock<ITelefonoRepository>();
        repoMock.Setup(r => r.ObtenerTelefonoPorId(2)).ReturnsAsync((string?)null);

        var useCase = new ObtenerTelefonoPorIdCasoDeUso(repoMock.Object);

        await Assert.ThrowsAsync<Exception>(async () => await useCase.EjecutarAsync(2));
    }
}
