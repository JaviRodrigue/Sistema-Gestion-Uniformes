using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using VentasApp.Application.CasoDeUso.Cliente;
using VentasApp.Application.DTOs.Cliente;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Cliente;
using Xunit;
namespace VentasApp.Tests.Application.ClienteTest;

public class ObtenerTodosClientesTest
{
    [Fact]
    public async Task ObtenerTodosClientes_DeberiaRetornarListaDeClientes()
    {
        // Arrange
        var repoMock = new Mock<IClienteRepository>();
        var clientes = new List<Cliente>
        {
            new("Juan", "44048885"),
            new("Pedro", "55055999")
        };
        repoMock.Setup(r => r.ListarClientes())
                .ReturnsAsync(clientes);
        var obtenerTodosClientesCasoDeUso = new ObtenerTodosClientesCasoDeUso(repoMock.Object);
        // Act
        var resultado = await obtenerTodosClientesCasoDeUso.EjecutarAsync();
        // Assert
        resultado.Should().HaveCount(2);
        resultado[0].Nombre.Should().Be("Juan");
        resultado[1].Nombre.Should().Be("Pedro");
    }
}
