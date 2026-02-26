using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using VentasApp.Infrastructure.Persistencia.Contexto;

namespace VentasApp.Infrastructure.Persistencia.Migrations;

/// <summary>
/// Helper para aplicar migraciones manuales a la base de datos sin usar EF Migrations.
/// Útil para cambios menores en desarrollo con SQLite.
/// </summary>
public static class DatabaseMigrationHelper
{
    /// <summary>
    /// Aplica todas las migraciones pendientes al esquema de la base de datos.
    /// </summary>
    public static async Task AplicarMigracionesAsync(DatabaseContext context)
    {
        await AgregarColumnaVerificadoAPagoSiNoExiste(context);
        await AgregarColumnaEntregadoADetalleVentaSiNoExiste(context);
        await AgregarColumnaCodigoVentaSiNoExiste(context);
    }

    private static async Task AgregarColumnaVerificadoAPagoSiNoExiste(DatabaseContext context)
    {
        var connection = context.Database.GetDbConnection();
        var wasClosedInitially = connection.State == System.Data.ConnectionState.Closed;

        try
        {
            if (wasClosedInitially)
            {
                await connection.OpenAsync();
            }

            // Verificar si la columna existe
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "PRAGMA table_info(Pago);";
            
            var columnaExiste = false;
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var columnName = reader.GetString(1); // name es la segunda columna en table_info
                    if (columnName.Equals("verificado", StringComparison.OrdinalIgnoreCase))
                    {
                        columnaExiste = true;
                        break;
                    }
                }
            }

            // Si no existe, agregarla
            if (!columnaExiste)
            {
                using var alterCmd = connection.CreateCommand();
                alterCmd.CommandText = "ALTER TABLE Pago ADD COLUMN verificado INTEGER NOT NULL DEFAULT 0;";
                await alterCmd.ExecuteNonQueryAsync();
                
                System.Diagnostics.Debug.WriteLine("? Columna 'verificado' agregada a la tabla Pago");
            }
        }
        finally
        {
            if (wasClosedInitially && connection.State == System.Data.ConnectionState.Open)
            {
                await connection.CloseAsync();
            }
        }
    }

    private static async Task AgregarColumnaEntregadoADetalleVentaSiNoExiste(DatabaseContext context)
    {
        var connection = context.Database.GetDbConnection();
        var wasClosedInitially = connection.State == System.Data.ConnectionState.Closed;

        try
        {
            if (wasClosedInitially)
            {
                await connection.OpenAsync();
            }

            // Verificar si la columna existe
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "PRAGMA table_info(DetalleVenta);";
            
            var columnaExiste = false;
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var columnName = reader.GetString(1);
                    if (columnName.Equals("entregado", StringComparison.OrdinalIgnoreCase))
                    {
                        columnaExiste = true;
                        break;
                    }
                }
            }

            // Si no existe, agregarla
            if (!columnaExiste)
            {
                using var alterCmd = connection.CreateCommand();
                alterCmd.CommandText = "ALTER TABLE DetalleVenta ADD COLUMN entregado INTEGER NOT NULL DEFAULT 0;";
                await alterCmd.ExecuteNonQueryAsync();
                
                System.Diagnostics.Debug.WriteLine("? Columna 'entregado' agregada a la tabla DetalleVenta");
            }
        }
        finally
        {
            if (wasClosedInitially && connection.State == System.Data.ConnectionState.Open)
            {
                await connection.CloseAsync();
            }
        }
    }

    private static async Task AgregarColumnaCodigoVentaSiNoExiste(DatabaseContext context)
    {
        var connection = context.Database.GetDbConnection();
        var wasClosedInitially = connection.State == System.Data.ConnectionState.Closed;

        try
        {
            if (wasClosedInitially)
            {
                await connection.OpenAsync();
            }

            using var cmd = connection.CreateCommand();
            cmd.CommandText = "PRAGMA table_info(Venta);";
            
            var columnaExiste = false;
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var columnName = reader.GetString(1);
                    if (columnName.Equals("CodigoVenta", StringComparison.OrdinalIgnoreCase))
                    {
                        columnaExiste = true;
                        break;
                    }
                }
            }

            if (!columnaExiste)
            {
                using var alterCmd = connection.CreateCommand();
                alterCmd.CommandText = "ALTER TABLE Venta ADD COLUMN CodigoVenta TEXT;";
                await alterCmd.ExecuteNonQueryAsync();

                using var updateCmd = connection.CreateCommand();
                updateCmd.CommandText = "UPDATE Venta SET CodigoVenta = CAST(Id AS TEXT) WHERE CodigoVenta IS NULL;";
                await updateCmd.ExecuteNonQueryAsync();

                using var indexCmd = connection.CreateCommand();
                indexCmd.CommandText = "CREATE UNIQUE INDEX IX_Venta_CodigoVenta ON Venta (CodigoVenta);";
                await indexCmd.ExecuteNonQueryAsync();
                
                System.Diagnostics.Debug.WriteLine("? Columna 'CodigoVenta' agregada a la tabla Venta");
            }
        }
        finally
        {
            if (wasClosedInitially && connection.State == System.Data.ConnectionState.Open)
            {
                await connection.CloseAsync();
            }
        }
    }
}
