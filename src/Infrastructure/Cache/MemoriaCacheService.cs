using Microsoft.Extensions.Caching.Memory;

namespace Clientes.API.Infrastructure.Cache;

public interface ICacheService
{
    void Set<T>(string chave, T valor, TimeSpan ttl);

    bool TryGet<T>(string chave, out T? valor);

    void Remove(string chave);
}

public class MemoriaCacheService : ICacheService
{
    private readonly IMemoryCache _cache;

    public MemoriaCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void Set<T>(string chave, T valor, TimeSpan ttl)
    {
        _cache.Set(chave, valor, ttl);
    }

    public bool TryGet<T>(string chave, out T? valor)
    {
        return _cache.TryGetValue(chave, out valor);
    }

    public void Remove(string chave)
    {
        _cache.Remove(chave);
    }
}
