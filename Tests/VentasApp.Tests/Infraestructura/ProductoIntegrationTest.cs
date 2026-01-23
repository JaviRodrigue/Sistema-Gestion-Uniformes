using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using VentasApp.Domain.Modelo.Productos;
using VentasApp.Infrastructure.Persistencia.Repositorios;
using VentasApp.Domain.Modelo.Categoria;

public class ProductoItemStockIntegrationTests
{
    [Fact]
    public async Task Producto_DeberiaPersistirseConItemsVendibles()
    {
        using var context = DatabaseContextTestFactory.Create();
        var productoRepo = new ProductoRepository(context);

        var categoria = new Categoria("Ropa");

        var producto = new Producto(categoria.Id,"Remera", 100, 200);

        var item1 = new ItemVendible(1,"Remera M", "RBM", "M");
        var item2 = new ItemVendible(1,"Remera L", "RBL", "L");
        producto.AgregarItem(item1);
        producto.AgregarItem(item2);

        await productoRepo.Agregar(producto);
        await context.SaveChangesAsync();

        var productoDb = await productoRepo.ObtenerProducto(producto.Id);

        productoDb.Should().NotBeNull();
        productoDb!.ItemsVendibles.Should().HaveCount(2);
    }

    [Fact]
    public async Task ItemVendible_CodigoBarraDuplicado_DeberiaLanzarExcepcion()
    {
        using var context = DatabaseContextTestFactory.Create();

        var categoria = new Categoria("Ropa");
        var producto = new Producto(categoria.Id,"Pantalon", 300, 600);
        var item = new ItemVendible(1,"Pantalon M", "COD123", "M");
        producto.AgregarItem(item);

        context.Producto.Add(producto);
        await context.SaveChangesAsync();

        var itemDuplicado = new ItemVendible(
            producto.Id,
            "Pantalon L",
            "COD123",
            "L"
        );

        context.ItemVendible.Add(itemDuplicado);

        await Assert.ThrowsAsync<DbUpdateException>(() =>
            context.SaveChangesAsync()
        );
    }

    [Fact]
    public async Task Stock_DeberiaPersistirseAsociadoAItemVendible()
    {
        using var context = DatabaseContextTestFactory.Create();

        var producto = new Producto(1,"Zapatilla", 500, 900);
        var item2  = new ItemVendible(1,"Zapatilla 42", "Z42", "42");
        producto.AgregarItem(item2);

        context.Producto.Add(producto);
        await context.SaveChangesAsync();

        var item = producto.ItemsVendibles.First();

        var stock = new Stock(
            IdItemVendible: item.Id,
            cantidadInicial: 10,
            stockMinimo: 2
        );

        context.Stock.Add(stock);
        await context.SaveChangesAsync();

        var stockDb = await context.Stock
            .FirstOrDefaultAsync(s => s.IdItemVendible == item.Id);

        stockDb.Should().NotBeNull();
        stockDb!.CantidadDisponible.Should().Be(10);
        stockDb.CantidadReservada.Should().Be(0);
    }

    [Fact]
    public async Task Stock_SinItemVendible_DeberiaFallarPorFK()
    {
        using var context = DatabaseContextTestFactory.Create();

        var stock = new Stock(
            IdItemVendible: 999,
            cantidadInicial: 5,
            stockMinimo: 1
        );

        context.Stock.Add(stock);

        await Assert.ThrowsAsync<DbUpdateException>(() =>
            context.SaveChangesAsync()
        );
    }

    [Fact]
    public async Task ObtenerStockPorItemVendible_DeberiaFuncionarCorrectamente()
    {
        using var context = DatabaseContextTestFactory.Create();

        var producto = new Producto(1,"Camisa", 400, 700);
        var item2 = new ItemVendible(1,"Camisa M", "CM01", "M");
        producto.AgregarItem(item2);

        context.Producto.Add(producto);
        await context.SaveChangesAsync();

        var item = producto.ItemsVendibles.First();

        var stock = new Stock(item.Id, 20, 3);
        context.Stock.Add(stock);
        await context.SaveChangesAsync();

        var stockRepo = new StockRepository(context);

        var stockDb = await stockRepo.ObtenerPorItemVendible(item.Id);

        stockDb.Should().NotBeNull();
        stockDb!.CantidadDisponible.Should().Be(20);
    }

    [Fact]
    public async Task Producto_NoDeberiaEliminarItemsVendiblesEnCascade()
    {
        using var context = DatabaseContextTestFactory.Create();

        var producto = new Producto(1,"Buzo", 800, 1200);
        var item2 = new ItemVendible(1,"Buzo L", "BZL", "L");
        producto.AgregarItem(item2);

        context.Producto.Add(producto);


       await Assert.ThrowsAsync<DbUpdateException>(() =>
        {
            context.Producto.Remove(producto);
            return context.SaveChangesAsync();
        });
    }
}
