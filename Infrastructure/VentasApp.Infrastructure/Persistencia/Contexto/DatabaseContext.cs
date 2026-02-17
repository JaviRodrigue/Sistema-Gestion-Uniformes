using Microsoft.EntityFrameworkCore;
using VentasApp.Domain.Modelo.Venta;
using VentasApp.Domain.Modelo.Categoria;
using VentasApp.Domain.Modelo.Pago;
using VentasApp.Domain.Enum;
using VentasApp.Domain.Modelo.Cliente;
using VentasApp.Domain.Modelo.Productos;
using VentasApp.Infrastructure.Persistencia.Configuraciones;

namespace VentasApp.Infrastructure.Persistencia.Contexto;

public class DatabaseContext : DbContext
{
    public DatabaseContext() { }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    //Ventas
    public DbSet<Venta> Ventas { get; set; }
    public DbSet<DetalleVenta> DetalleVenta { get; set; }
    public DbSet<Compra> Compras { get; set; }
    
    //Cliente
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Telefono> Telefonos { get; set; }
    
    //Pagos
    public DbSet<Pago> Pagos { get; set; }
    public DbSet<PagoMetodo> PagoMetodos { get; set; }
    public DbSet<MedioPago> MedioPagos { get; set; }
    
    //Productos
    public DbSet<Producto> Producto { get; set; }
    public DbSet<ItemVendible> ItemVendible { get; set; }
    public DbSet<Stock> Stock { get; set; }
    public DbSet<Categoria> Categoria { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Guardar la base de datos fuera de bin, en Infrastructure/VentasApp.Infrastructure/Archivo
            // Localizar la raíz del repositorio (carpeta 'Sistema-Gestion-Uniformes') desde AppContext.BaseDirectory
            var root = GetRepositoryRoot(AppContext.BaseDirectory, "Sistema-Gestion-Uniformes");
            var folder = root is not null
                ? Path.Combine(root, "Infrastructure", "VentasApp.Infrastructure", "Archivo")
                : Path.Combine(AppContext.BaseDirectory, "Archivo");

            Directory.CreateDirectory(folder);
            var dbPath = Path.Combine(folder, "Database.sqlite");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }

    private static string? GetRepositoryRoot(string startPath, string repoFolderName)
    {
        var dir = new DirectoryInfo(startPath);
        while (dir is not null)
        {
            if (string.Equals(dir.Name, repoFolderName, StringComparison.OrdinalIgnoreCase))
            {
                return dir.FullName;
            }

            // Parar si llegamos al perfil de usuario o a la raíz
            if (dir.Parent is null) break;
            dir = dir.Parent;
        }

        return null;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new VentaConfiguracion());
        modelBuilder.ApplyConfiguration(new DetalleVentaConfiguracion());
        modelBuilder.ApplyConfiguration(new CompraConfiguracion());
        modelBuilder.ApplyConfiguration(new ClienteConfiguracion());
        modelBuilder.ApplyConfiguration(new TelefonoConfiguracion());
        modelBuilder.ApplyConfiguration(new PagoConfiguracion());
        modelBuilder.ApplyConfiguration(new PagoMetodoConfiguracion());
        modelBuilder.ApplyConfiguration(new MedioPagoConfiguracion());
        modelBuilder.ApplyConfiguration(new ProductoConfiguracion());
        modelBuilder.ApplyConfiguration(new ItemVendibleConfiguracion());
        modelBuilder.ApplyConfiguration(new StockConfiguracion());
        modelBuilder.ApplyConfiguration(new CategoriaConfiguracion());
    }
}
