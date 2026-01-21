using FluentAssertions;
using FluentAssertions;
using System;
using Xunit;
using VentasApp.Domain.Modelo.Cliente;

namespace VentasApp.Tests.Domain.ClienteTest;

public class TelefonoTest
{
    // =============================
    // CREACIÓN
    // =============================

    [Fact]
    public void CrearTelefono_ConstructorManual_DeberiaInicializarPropiedades()
    {
        // Arrange
        var cliente = new Cliente("Maria", "12345678");
        int idClienteManual = 99;
        string numero = "555-1234";

        // Act
        // Probamos el constructor: Telefono(int idCliente, string numero, Cliente cliente)
        var telefono = new Telefono(idClienteManual, numero, cliente);

        // Assert
        telefono.Numero.Should().Be(numero);
        telefono.IdCliente.Should().Be(idClienteManual);
        telefono.Cliente.Should().BeSameAs(cliente);
        telefono.Activado.Should().BeTrue();
    }

    [Fact]
    public void CrearTelefono_DesdeCliente_DeberiaHeredarIdDelCliente()
    {
        // Arrange
        var cliente = new Cliente("Maria", "12345678");
        string numero = "555-9876";

        // Nota: Como 'cliente' es nuevo, su Id probablemente sea 0 (dependiendo de tu clase Entidad).
        // Verificamos que el teléfono copie exactamente ese ID.

        // Act
        // Probamos el constructor: Telefono(Cliente cliente, string numero)
        var telefono = new Telefono(cliente, numero);

        // Assert
        telefono.Numero.Should().Be(numero);
        telefono.Cliente.Should().BeSameAs(cliente);
        telefono.IdCliente.Should().Be(cliente.Id); // Debe coincidir con el ID del objeto cliente pasado
        telefono.Activado.Should().BeTrue();
    }

    // =============================
    // ESTADOS (Activar / Desactivar)
    // =============================

    [Fact]
    public void Desactivar_DeberiaCambiarEstadoAFalso()
    {
        // Arrange
        var telefono = TelefonoValido();

        // Act
        telefono.Desactivar();

        // Assert
        telefono.Activado.Should().BeFalse();
    }

    [Fact]
    public void Activar_PreviamenteDesactivado_DeberiaCambiarEstadoATrue()
    {
        // Arrange
        var telefono = TelefonoValido();
        telefono.Desactivar(); // Lo ponemos en false primero

        // Act
        telefono.Activar();

        // Assert
        telefono.Activado.Should().BeTrue();
    }

    // =============================
    // HELPERS
    // =============================

    private static Telefono TelefonoValido()
    {
        var cliente = new Cliente("Test", "111");
        return new Telefono(cliente, "123-456");
    }
}