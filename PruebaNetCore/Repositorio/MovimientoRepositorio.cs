using Microsoft.EntityFrameworkCore;
using PruebaNetCore.Data;
using PruebaNetCore.Modelos;
using PruebaNetCore.Repositorio.IRepositorio;

namespace PruebaNetCore.Repositorio
{
    public class MovimientoRepositorio : IMovimientoRepositorio
    {
        private readonly ApplicationDBContext context;
        public MovimientoRepositorio(ApplicationDBContext context)
        {
            this.context = context;
        }

        public bool ActualizarMovimiento(Movimiento movimiento)
        {
            context.Movimientos.Update(movimiento);
            return Guardar();
        }

        public bool BorrarMovimiento(Movimiento movimiento)
        {
            context.Movimientos.Remove(movimiento);
            return Guardar();
        }

        public bool CrearMovimiento(Movimiento movimiento)
        {
            context.Movimientos.Add(movimiento);
            return Guardar();
        }

        public bool ExisteMovimiento(int movimientoId)
        {
            return context.Movimientos.Any(m => m.Id == movimientoId);
        }

        public Movimiento? GetMovimiento(int movimientoId)
        {
            return context.Movimientos.FirstOrDefault(m => m.Id == movimientoId);
        }

        public ICollection<Movimiento> GetMovimientos()
        {
            return context.Movimientos.OrderBy(m => m.Fecha).ToList();
        }

        public ICollection<Movimiento> GetMovimientosDeCuenta(int numeroCuenta)
        {
            return context.Movimientos.Where(m => m.CuentaId == numeroCuenta).OrderBy(m => m.Fecha).ToList();
        }

        public ICollection<Movimiento> GetMovimientosDeCuentasDeCliente(int clienteId)
        {
            return context.Movimientos.Include(m => m.Cuenta).Where(m => m.Cuenta.ClienteId == clienteId).OrderBy(m => m.Fecha).ToList();
        }

        public bool Guardar()
        {
            return context.SaveChanges() >= 0 ? true : false;
        }
    }
}
