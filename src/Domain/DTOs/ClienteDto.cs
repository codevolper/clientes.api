namespace Clientes.API.Domain.DTOs;

public class ClienteDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public decimal ValorLimite { get; set; }
}
