using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VentasApp.Infrastructure.Persistencia.Contexto;

public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "VentasApp");

        Directory.CreateDirectory(folder);

        var dbPath = Path.Combine(folder, "Database.sqlite");

        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        optionsBuilder.UseSqlite($"Data Source={dbPath}");

        return new DatabaseContext(optionsBuilder.Options);
    }
}
