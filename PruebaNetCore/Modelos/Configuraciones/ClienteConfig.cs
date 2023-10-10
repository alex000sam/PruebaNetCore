using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PruebaNetCore.Modelos.Configuraciones
{
    public class ClienteConfig : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.Property(c => c.Nombre).HasMaxLength(150);
            builder.Property(c => c.Genero).HasMaxLength(1);
            builder.Property(c => c.Direccion).HasMaxLength(300);
            builder.Property(c => c.Telefono).HasMaxLength(10);
            builder.Property(c => c.Contraseña).HasMaxLength(16);
        }
    }
}
