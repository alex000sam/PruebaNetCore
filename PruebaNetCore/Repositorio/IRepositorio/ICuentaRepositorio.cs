using PruebaNetCore.Modelos;

namespace PruebaNetCore.Repositorio.IRepositorio
{
    public interface ICuentaRepositorio
    {
        ICollection<Cuenta> GetCuentas();
        Cuenta? GetCuenta(int numeroCuenta);
        bool ExisteCuenta(int numeroCuenta);
        bool CrearCuenta(Cuenta cuenta);
        bool ActualizarCuenta(Cuenta cuenta);
        bool BorrarCuenta(Cuenta cuenta);
        ICollection<Cuenta> GetCuentasDeCliente(int clienteId);
        decimal SaldoActual(int numeroCuenta);
        bool Guardar();
    }
}
