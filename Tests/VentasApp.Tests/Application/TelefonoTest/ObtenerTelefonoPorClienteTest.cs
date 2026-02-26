using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using VentasApp.Application.CasoDeUso.Telefono;
using VentasApp.Application.Interfaces.Repositorios;
using Xunit;

namespace VentasApp.Tests.Application.TelefonoTest;

public class ObtenerTelefonoPorClienteTest
{
    [Fact]
    public async Task ObtenerTelefonosPorCliente_DeberiaRetornarLista()
    {
        var repoMock = new Mock<ITelefonoRepository>();
        repoMock.Setup(r => r.ObtenerTelefonosPorClienteId(1)).ReturnsAsync(["123"]);

        var useCase = new ObtenerTelefonosPorClienteCasoDeUso(repoMock.Object);

        var resultado = await useCase.EjecutarAsync(1);

        resultado.Should().ContainSingle().Which.Should().Be("123");
    }

    [Fact]
    public async Task ObtenerTelefonosPorCliente_SinTelefonos_RetornaVacio()
    {
        var repoMock = new Mock<ITelefonoRepository>();
        repoMock.Setup(r => r.ObtenerTelefonosPorClienteId(2)).ReturnsAsync([]);

        var useCase = new ObtenerTelefonosPorClienteCasoDeUso(repoMock.Object);

        var resultado = await useCase.EjecutarAsync(2);

        resultado.Should().BeEmpty();
    }
}
