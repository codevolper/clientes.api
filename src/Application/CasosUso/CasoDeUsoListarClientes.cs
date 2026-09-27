using Clientes.API.Domain.Entities;
using Clientes.API.Domain.Interfaces;
using Clientes.API.Infrastructure.Cache;

namespace Clientes.API.Application.CasosUso;

public class CasoDeUsoListarClientes
{
    private readonly ICacheService _cache;
    private readonly IClienteRepositorio _repositorio;    

    public CasoDeUsoListarClientes(IClienteRepositorio repositorio, ICacheService cache)
    {
        _repositorio = repositorio;
        _cache = cache;
    }

    public async Task<IEnumerable<Cliente>> ListarClientesAsync()
    {
        const string chave = "clientes:todos";
        if (_cache.TryGet<IEnumerable<Cliente>>(chave, out var cached))
            return cached!;

        var lista = await _repositorio.ListarTodosAsync();
        _cache.Set(chave, lista, TimeSpan.FromMinutes(10));

        return lista;
    }
}
