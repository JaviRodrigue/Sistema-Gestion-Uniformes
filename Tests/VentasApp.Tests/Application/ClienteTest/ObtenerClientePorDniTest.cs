using FluentAssertions;
using Moq;
using System;
using System.Reflection;
using System.Threading.Tasks;
using VentasApp.Application.CasoDeUso.Cliente;
using VentasApp.Application.DTOs.Cliente;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Cliente;
using Xunit;

namespace VentasApp.Tests.Application.ClienteTest;

public class ObtenerClientePorDniTest
{
    [Fact]
    public async Task ObtenerClientePorDni_test()
    {
        // Arrange
        var repoMock = new Mock<IClienteRepository>();
        var cliente = new Cliente("Juan", "44048885");

        repoMock.Setup(r => r.ObtenerClientePorDni("44048885"))
                .ReturnsAsync(cliente);

        var obtenerClientePorDniCasoDeUso = new ObtenerClientePorDniCasoDeUso(repoMock.Object);

        // Act
        var resultado = await obtenerClientePorDniCasoDeUso.EjecutarAsync("44048885");

        // Assert
        resultado.Nombre.Should().Be("Juan");
        resultado.Dni.Should().Be("44048885");
    }

    [Fact]
    public async Task ObtenerClientePorDni_NoExiste_DeberiaLanzar()
    {
        var repoMock = new Mock<IClienteRepository>();
        repoMock.Setup(r => r.ObtenerClientePorDni("00000000")).ReturnsAsync((Cliente?)null);

        var obtenerClientePorDniCasoDeUso = new ObtenerClientePorDniCasoDeUso(repoMock.Object);

        await Assert.ThrowsAsync<Exception>(async () => await obtenerClientePorDniCasoDeUso.EjecutarAsync("00000000"));
    }
}
