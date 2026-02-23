using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using VentasApp.Application.CasoDeUso.Cliente;
using VentasApp.Application.DTOs.Cliente;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Cliente;
using Xunit;

namespace VentasApp.Tests.Application.ClienteTest;

public class ObtenerClientePorTelefonoTest
{
    [Fact]
    public async Task ObtenerClientePorTelefono_test()
    {
        // Arrange
        var repoMock = new Mock<IClienteRepository>();
        var cliente = new Cliente("Juan", "44048885");
        cliente.ReemplazarTelefonos(["1234567890"]);

        repoMock.Setup(r => r.ObtenerClientePorTelefono("1234567890"))
                .ReturnsAsync(cliente);

        var obtenerClientePorTelefonoCasoDeUso = new ObtenerClientePorTelefonoCasoDeUso(repoMock.Object);

        // Act
        var resultado = await obtenerClientePorTelefonoCasoDeUso.EjecutarAsync("1234567890");

        // Assert
        resultado.Nombre.Should().Be("Juan");
        resultado.Telefonos.Should().NotBeNull();
        resultado.Telefonos.Should().ContainSingle().Which.Should().Be("1234567890");
    }

    [Fact]
    public async Task ObtenerClientePorTelefono_NoExiste_DeberiaLanzar()
    {
        var repoMock = new Mock<IClienteRepository>();
        repoMock.Setup(r => r.ObtenerClientePorTelefono("000")).ReturnsAsync((Cliente?)null);

        var obtenerClientePorTelefonoCasoDeUso = new ObtenerClientePorTelefonoCasoDeUso(repoMock.Object);

        await Assert.ThrowsAsync<Exception>(async () => await obtenerClientePorTelefonoCasoDeUso.EjecutarAsync("000"));
    }
}
