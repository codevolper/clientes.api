using System.Data;

namespace Clientes.API.Infrastructure.Persistencia;

/// <summary>
/// Base de dados em memória usando DataSet/DataTable simulando tabelas relacionais.
/// </summary>
public class InMemoryDatabase
{
    private readonly DataSet _dataset = new();
    private readonly object _lock = new();

    public InMemoryDatabase()
    {
        InicializarTabelas();
    }

    private void InicializarTabelas()
    {
        var clientes = new DataTable("Clientes");

        clientes.Columns.Add("Id", typeof(Guid));
        clientes.Columns.Add("Nome", typeof(string));
        clientes.Columns.Add("CPF", typeof(string));       
        clientes.Columns.Add("ValorLimite", typeof(decimal));

        clientes.PrimaryKey = new[] { clientes.Columns["Id"] };

        _dataset.Tables.Add(clientes);
    }

    public DataTable GetTable(string nome)
    {
        lock (_lock)
        {
            return _dataset.Tables[nome];
        }
    }

    public DataSet GetDataSet()
    {
        lock (_lock)
        {
            return _dataset;
        }
    }
}
