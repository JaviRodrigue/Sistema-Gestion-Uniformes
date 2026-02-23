using FluentAssertions;
using VentasApp.Domain.Modelo.Productos;
using VentasApp.Infrastructure.Persistencia.Contexto;
using VentasApp.Infrastructure.Persistencia.Repositorios;
using Xunit;

namespace VentasApp.Tests.Infraestructura;

public class ItemVendibleRepositoryTest
{
    private static async Task<ItemVendible> SeedItemAsync(
        DatabaseContext context, string nombre, string codigo, string? talle, bool desactivar = false)
    {
        var producto = new Producto(1, "Producto Test", 100, 200);
        var item = new ItemVendible(1, nombre, codigo, talle);
        if (desactivar) item.Desactivar();
        producto.AgregarItem(item);
        context.Producto.Add(producto);
        await context.SaveChangesAsync();
        return item;
    }

    [Fact]
    public async Task ExisteConNombreYTalle_CuandoItemActivoCoincide_RetornaTrue()
    {
        using var context = DatabaseContextTestFactory.Create();
        await SeedItemAsync(context, "Remera", "COD-001", "M");
        var repo = new ItemVendibleRepository(context);

        var resultado = await repo.ExisteConNombreYTalle("Remera", "M");

        resultado.Should().BeTrue();
    }

    [Fact]
    public async Task ExisteConNombreYTalle_CuandoNoExisteNingunItem_RetornaFalse()
    {
        using var context = DatabaseContextTestFactory.Create();
        var repo = new ItemVendibleRepository(context);

        var resultado = await repo.ExisteConNombreYTalle("Remera", "M");

        resultado.Should().BeFalse();
    }

    [Fact]
    public async Task ExisteConNombreYTalle_CuandoMismoNombreTalleDiferente_RetornaFalse()
    {
        using var context = DatabaseContextTestFactory.Create();
        await SeedItemAsync(context, "Remera", "COD-002", "L");
        var repo = new ItemVendibleRepository(context);

        var resultado = await repo.ExisteConNombreYTalle("Remera", "M");

        resultado.Should().BeFalse();
    }

    [Fact]
    public async Task ExisteConNombreYTalle_CuandoItemDesactivado_RetornaFalse()
    {
        using var context = DatabaseContextTestFactory.Create();
        await SeedItemAsync(context, "Remera", "COD-003", "M", desactivar: true);
        var repo = new ItemVendibleRepository(context);

        var resultado = await repo.ExisteConNombreYTalle("Remera", "M");

        resultado.Should().BeFalse();
    }

    [Fact]
    public async Task ExisteConNombreYTalle_NombreConMayusculas_NormalizaYRetornaTrue()
    {
        using var context = DatabaseContextTestFactory.Create();
        await SeedItemAsync(context, "remera", "COD-004", "M");
        var repo = new ItemVendibleRepository(context);

        var resultado = await repo.ExisteConNombreYTalle("  REMERA  ", "M");

        resultado.Should().BeTrue();
    }
}

