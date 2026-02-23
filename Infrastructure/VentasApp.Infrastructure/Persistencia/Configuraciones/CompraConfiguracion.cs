using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VentasApp.Domain.Modelo.Venta;

namespace VentasApp.Infrastructure.Persistencia.Configuraciones;

    public class CompraConfiguracion : IEntityTypeConfiguration<Compra>
    {
      public void Configure(EntityTypeBuilder<Compra> builder)
      {
        builder.ToTable("Compra");

            builder.HasKey(c => new { c.IdVenta, c.IdCliente }); // Clave primaria compuesta

            builder.Property(c => c.IdVenta)
                  .HasColumnName("id_venta")
                  .IsRequired();
            builder.Property(c => c.IdCliente)
                  .HasColumnName("id_cliente")
                  .IsRequired();

      }
    } 


