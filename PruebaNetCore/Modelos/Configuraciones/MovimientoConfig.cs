using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PruebaNetCore.Modelos.Configuraciones
{
    public class MovimientoConfig : IEntityTypeConfiguration<Movimiento>
    {
        public void Configure(EntityTypeBuilder<Movimiento> builder)
        {
            builder.Property(m => m.TipoMovimiento).HasMaxLength(20);
            builder.Property(m => m.Valor).HasPrecision(18, 2);
            builder.Property(m => m.Saldo).HasPrecision(18, 2);
        }
    }
}
