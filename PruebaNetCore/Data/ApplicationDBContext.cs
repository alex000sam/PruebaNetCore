using Microsoft.EntityFrameworkCore;
using PruebaNetCore.Modelos;
using System.Reflection;

namespace PruebaNetCore.Data
{
    public class ApplicationDBContext : DbContext
    {

        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Persona> Personas => Set<Persona>();
        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Cuenta> Cuentas => Set<Cuenta>();
        public DbSet<Movimiento> Movimientos => Set<Movimiento>();

    }
}
