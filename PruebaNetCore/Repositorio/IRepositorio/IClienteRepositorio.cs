using PruebaNetCore.Modelos;

namespace PruebaNetCore.Repositorio.IRepositorio
{
    public interface IClienteRepositorio
    {
        ICollection<Cliente> GetClientes();
        Cliente? GetCliente(int clienteId);
        bool ExisteCliente(string nombre);
        bool ExisteCliente(int clienteId);
        bool CrearCliente(Cliente cliente);
        bool ActualizarCliente(Cliente cliente);
        bool BorrarCliente(Cliente cliente);
        bool Guardar();
    }
}
