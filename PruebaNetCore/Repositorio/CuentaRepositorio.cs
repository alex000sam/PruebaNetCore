using PruebaNetCore.Data;
using PruebaNetCore.Modelos;
using PruebaNetCore.Repositorio.IRepositorio;

namespace PruebaNetCore.Repositorio
{
    public class CuentaRepositorio : ICuentaRepositorio
    {
        private readonly ApplicationDBContext context;
        public CuentaRepositorio(ApplicationDBContext context)
        {
            this.context = context;
        }

        public bool ActualizarCuenta(Cuenta cuenta)
        {
            context.Cuentas.Update(cuenta);
            return Guardar();
        }

        public bool BorrarCuenta(Cuenta cuenta)
        {
            context.Cuentas.Remove(cuenta);
            return Guardar();
        }

        public bool CrearCuenta(Cuenta cuenta)
        {
            context.Cuentas.Add(cuenta);
            return Guardar();
        }

        public bool ExisteCuenta(int numeroCuenta)
        {
            return context.Cuentas.Any(c => c.NumeroCuenta == numeroCuenta);
        }

        public Cuenta? GetCuenta(int numeroCuenta)
        {
            return context.Cuentas.FirstOrDefault(c => c.NumeroCuenta == numeroCuenta);
        }

        public ICollection<Cuenta> GetCuentas()
        {
            return context.Cuentas.OrderBy(c => c.NumeroCuenta).ToList();
        }

        public ICollection<Cuenta> GetCuentasDeCliente(int clienteId)
        {
            return context.Cuentas.Where(c => c.ClienteId == clienteId).OrderBy(c => c.NumeroCuenta).ToList();
        }

        public decimal SaldoActual(int numeroCuenta)
        {
            decimal saldoACtual = 0;

            var ultimoMovimiento = context.Movimientos
                .Where(m => m.CuentaId == numeroCuenta)
                .OrderByDescending(m => m.Fecha)
                .FirstOrDefault();

            if (ultimoMovimiento is null)
            {
                var cuenta = context.Cuentas.FirstOrDefault(c => c.NumeroCuenta == numeroCuenta);
                if (cuenta is not null)
                {
                    saldoACtual = cuenta.SaldoInicial;
                }
            }
            else
            {
                saldoACtual = ultimoMovimiento.Saldo;
            }

            return saldoACtual;
        }

        public bool Guardar()
        {
            return context.SaveChanges() >= 0 ? true : false;
        }
    }
}
