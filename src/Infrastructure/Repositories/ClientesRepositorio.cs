using Clientes.API.Domain.Entities;
using Clientes.API.Domain.Interfaces;
using Clientes.API.Infrastructure.Persistencia;
using System.Data;

namespace Clientes.API.Infrastructure.Repositories;

public class ClientesRepositorio : IClienteRepositorio
{
    private readonly InMemoryDatabase _db;

    public ClientesRepositorio(InMemoryDatabase db)
    {
        _db = db;
    }

    public Task InserirAsync(Cliente cliente)
    {
        var tabela = _db.GetTable("Clientes");

        var linha = tabela.NewRow();
        linha["Id"] = cliente.Id;
        linha["Nome"] = cliente.Nome;      
        linha["CPF"] = cliente.CPF;
        linha["ValorLimite"] = cliente.ValorLimite;
        tabela.Rows.Add(linha);

        return Task.CompletedTask;
    }

    public Task<IEnumerable<Cliente>> ListarTodosAsync()
    {
        var tabela = _db.GetTable("Clientes");
        var lista = tabela.Rows.Cast<DataRow>().Select(r => new Cliente
        {
            Id = (Guid)r["Id"],
            Nome = r["Nome"].ToString() ?? string.Empty,            
            CPF = r["CPF"].ToString() ?? string.Empty,
            ValorLimite = (decimal)r["ValorLimite"]
        }).ToList();

        return Task.FromResult<IEnumerable<Cliente>>(lista);
    }

    public Task<Cliente?> ObterPorCPFAsync(string email)
    {
        var tabela = _db.GetTable("Clientes");
        var row = tabela.Rows.Cast<DataRow>().FirstOrDefault(r => string.Equals(r["CPF"]?.ToString(), email, StringComparison.OrdinalIgnoreCase));

        if (row == null)
            return Task.FromResult<Cliente?>(null);

        var cliente = new Cliente
        {
            Id = (Guid)row["Id"],
            Nome = row["Nome"].ToString() ?? string.Empty,            
            CPF = row["CPF"].ToString() ?? string.Empty,
            ValorLimite = (decimal)row["ValorLimite"]
        };

        return Task.FromResult<Cliente?>(cliente);
    } 
}