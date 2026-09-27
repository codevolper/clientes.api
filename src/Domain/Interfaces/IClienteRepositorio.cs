using Clientes.API.Domain.Entities;

namespace Clientes.API.Domain.Interfaces;

public interface IClienteRepositorio
{
    Task InserirAsync(Cliente cliente);

    Task<IEnumerable<Cliente>> ListarTodosAsync();

    Task<Cliente?> ObterPorCPFAsync(string email);
}
