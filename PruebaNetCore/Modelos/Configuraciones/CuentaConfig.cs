using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PruebaNetCore.Modelos.Configuraciones
{
    public class CuentaConfig : IEntityTypeConfiguration<Cuenta>
    {
        public void Configure(EntityTypeBuilder<Cuenta> builder)
        {
            builder.HasKey(c => c.NumeroCuenta);
            builder.Property(c => c.NumeroCuenta).ValueGeneratedNever();
            builder.Property(c => c.TipoCuenta).HasMaxLength(20);
            builder.Property(c => c.SaldoInicial).HasPrecision(18, 2);
        }
    }
}
