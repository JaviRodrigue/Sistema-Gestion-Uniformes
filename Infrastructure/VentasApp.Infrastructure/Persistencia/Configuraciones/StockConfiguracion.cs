namespace VentasApp.Infrastructure.Persistencia.Configuraciones;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VentasApp.Domain.Modelo.Producto;
public class StockConfiguracion : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.ToTable("Stock");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Cantidad)
               .IsRequired();
    }
}
