using FluentAssertions;
using Moq;
using System;
using System.Reflection;
using System.Threading.Tasks;
using VentasApp.Application.CasoDeUso.Cliente;
using VentasApp.Application.DTOs.Cliente;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Cliente;
using VentasApp.Domain.Base;
using Xunit;

namespace VentasApp.Tests.Application.ClienteTest;

public class ObtenerClientePorIdTest
{
    [Fact]
    public async Task ObtenerClientePorId_test()
    {
        // Arrange
        var repoMock = new Mock<IClienteRepository>();
        var cliente = new Cliente("Juan", "44048885");
        typeof(Entidad)
            .GetProperty("Id", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
            .SetValue(cliente, 1);
        
        repoMock.Setup(r => r.ObtenerClientePorId(cliente.Id))
                .ReturnsAsync(cliente);
        var obtenerClientePorIdCasoDeUso = new ObtenerClientePorIdCasoDeUso(repoMock.Object);

        // Act
        var resultado = await obtenerClientePorIdCasoDeUso.EjecutarAsync(1);

        // Assert
        resultado.Nombre.Should().Be("Juan");
        resultado.Id.Should().Be(1);
    }

    [Fact]
    public async Task ObtenerClientePorId_NoExiste_DeberiaLanzar()
    {
        // Arrange
        var repoMock = new Mock<IClienteRepository>();
        repoMock.Setup(r => r.ObtenerClientePorId(99)).ReturnsAsync((Cliente?)null);

        var obtenerClientePorIdCasoDeUso = new ObtenerClientePorIdCasoDeUso(repoMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(async () => await obtenerClientePorIdCasoDeUso.EjecutarAsync(99));
    }
}
