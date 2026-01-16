using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VentasApp.Domain.Modelo.Producto;

public class ProductoConfiguracion : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        builder.ToTable("Producto");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nombre)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(p => p.Costo)
               .HasColumnType("DOUBLE")
               .IsRequired();

        builder.Property(p => p.PrecioVenta)
               .HasColumnType("DOUBLE")
               .IsRequired();

       builder.Property(p => p.Activo)
            .IsRequired();

        builder.HasMany(p => p.ItemsVendibles)
              .WithOne(i => i.Producto)
              .HasForeignKey(i => i.IdProducto);
    }
}
