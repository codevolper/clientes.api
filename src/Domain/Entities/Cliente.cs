namespace Clientes.API.Domain.Entities;

public class Cliente
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string CPF { get; set; } = string.Empty;
    
    public decimal ValorLimite { get; set; }
}
