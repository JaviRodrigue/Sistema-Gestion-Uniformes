using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VentasApp.Domain.Modelo.Categoria;
namespace VentasApp.Infrastructure.Persistencia.Configuraciones;
public class CategoriaConfiguracion : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("Categoria");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nombre)
               .IsRequired()
               .HasMaxLength(50);

        builder.HasIndex(c => c.Nombre)
               .IsUnique();
        builder.Property(c => c.Activa)
                .IsRequired();
    }
}
