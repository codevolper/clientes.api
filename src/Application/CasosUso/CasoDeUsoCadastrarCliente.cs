using Clientes.API.Domain.DTOs;
using Clientes.API.Domain.Entities;
using Clientes.API.Domain.Interfaces;
using Clientes.API.Infrastructure.Cache;

namespace Clientes.API.Application.CasosUso;

public class CasoDeUsoCadastrarCliente
{
    private readonly ICacheService _cache;
    private readonly IClienteRepositorio _repositorio;

    public CasoDeUsoCadastrarCliente(IClienteRepositorio repositorio, ICacheService cache)
    {
        _cache = cache;
        _repositorio = repositorio;
    }

    public async Task<ClienteRespostaDto> ExecutarAsync(ClienteDto clienteDto)
    {
        try
        {
            //Regra de negócio: Cliente deve ser único
            var existente = await _repositorio.ObterPorCPFAsync(clienteDto.CPF);
            if (existente != null)
                throw new InvalidOperationException("Já existe um cliente cadastrado com este CPF.");

            // Regra de negócio: ValorLimite não pode ser negativo
            if (clienteDto.ValorLimite < 0)
                throw new InvalidOperationException("O campo 'Valor Limite' não pode ser negativo.");

            //Regra de negócio: Id do cliente deve ser gerado automaticamente
            clienteDto.Id = Guid.NewGuid();

            var cliente = new Cliente
            {
                Id = clienteDto.Id,
                Nome = clienteDto.Nome,
                CPF = clienteDto.CPF,
                ValorLimite = clienteDto.ValorLimite
            };

            await _repositorio.InserirAsync(cliente);

            // Remove o cache de clientes para garantir que a lista seja atualizada na próxima consulta
            _cache.Remove("clientes:todos");

            return new ClienteRespostaDto { Id = clienteDto.Id, Status = "OK" };
        }
        catch (Exception ex)
        {            
            return new ClienteRespostaDto
            {                
                Status = "ERRO",                
                DetalheErro = ex.Message
            };
        }
    }

    public async Task<ClienteDto> AtualizarSaldoClienteAsync(ClienteDto clienteDto)
    {
        var existente = await _repositorio.ObterPorGuidAsync(clienteDto.Id);
        if (existente == null)
            throw new InvalidOperationException("Cliente não encontrado");

        var retorno = await _repositorio.AtualizarSaldo(new Domain.Entities.Cliente()
        {

            Id = clienteDto.Id,
            CPF = clienteDto.CPF,
            Nome = clienteDto.Nome,
            ValorLimite = clienteDto.ValorLimite
        });

        _cache.Remove("clientes:todos");

        return new ClienteDto
        {
            Id = retorno.Id,
            Nome = retorno.Nome,
            CPF = retorno.CPF,
            ValorLimite = retorno.ValorLimite
        };
    }
}
