using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VentasApp.Domain.Modelo.Cliente;

namespace VentasApp.Infrastructure.Persistencia.Configuraciones;
public class ClienteConfiguracion : IEntityTypeConfiguration<Cliente>
{
      public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Cliente");
            builder.HasKey(e => e.Id);
            builder.Property(c => c.Id)
                  .HasColumnName("id_cliente")
                  .ValueGeneratedOnAdd();
            builder.Property(c => c.Instagram)
                  .HasColumnName("instagram")
                  .HasColumnType("VARCHAR(100)")
                  .IsRequired(false);
            builder.Property(c => c.Nombre)
                  .HasColumnName("nombre")
                  .HasColumnType("VARCHAR(50)")
                  .IsRequired();
            builder.Property(c => c.FechaAlta)
                  .HasColumnName("fecha_alta")
                  .HasColumnType("DATE")
                  .IsRequired();
        }
}

