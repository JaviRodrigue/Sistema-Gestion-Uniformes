using Microsoft.EntityFrameworkCore;
using VentasApp.Infrastructure.Persistencia.Contexto;

public static class DatabaseContextTestFactory
{
    public static DatabaseContext Create()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseSqlite("Filename=:memory:")
            .Options;

        var context = new DatabaseContext(options);
        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        return context;
    }
}
