using PruebaNetCore.Data;
using PruebaNetCore.Modelos;
using PruebaNetCore.Repositorio.IRepositorio;

namespace PruebaNetCore.Repositorio
{
    public class ClienteRepositorio : IClienteRepositorio
    {
        private readonly ApplicationDBContext context;
        public ClienteRepositorio(ApplicationDBContext context)
        {
            this.context = context;
        }

        public bool ActualizarCliente(Cliente cliente)
        {
            context.Clientes.Update(cliente);
            return Guardar();
        }

        public bool BorrarCliente(Cliente cliente)
        {
            context.Clientes.Remove(cliente);
            return Guardar();
        }

        public bool CrearCliente(Cliente cliente)
        {
            context.Clientes.Add(cliente);
            return Guardar();
        }

        public bool ExisteCliente(string nombre)
        {
            bool valor = context.Clientes.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }

        public bool ExisteCliente(int clienteId)
        {
            return context.Clientes.Any(c => c.Id == clienteId);
        }

        public Cliente? GetCliente(int clienteId)
        {
            return context.Clientes.FirstOrDefault(c => c.Id == clienteId);
        }

        public ICollection<Cliente> GetClientes()
        {
            return context.Clientes.OrderBy(c => c.Nombre).ToList();
        }

        public bool Guardar()
        {
            return context.SaveChanges() >= 0 ? true : false;
        }
    }
}
