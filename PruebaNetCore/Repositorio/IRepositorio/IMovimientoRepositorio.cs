using PruebaNetCore.Modelos;

namespace PruebaNetCore.Repositorio.IRepositorio
{
    public interface IMovimientoRepositorio
    {
        ICollection<Movimiento> GetMovimientos();
        Movimiento? GetMovimiento(int movimientoId);
        bool ExisteMovimiento(int movimientoId);
        bool CrearMovimiento(Movimiento movimiento);
        bool ActualizarMovimiento(Movimiento movimiento);
        bool BorrarMovimiento(Movimiento movimiento);
        ICollection<Movimiento> GetMovimientosDeCuenta(int numeroCuenta);
        ICollection<Movimiento> GetMovimientosDeCuentasDeCliente(int clienteId);
        bool Guardar();
    }
}
