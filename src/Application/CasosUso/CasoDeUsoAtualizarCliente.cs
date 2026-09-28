using Clientes.API.Domain.DTOs;
using Clientes.API.Domain.Interfaces;
using Clientes.API.Infrastructure.Cache;

namespace Clientes.API.Application.CasosUso
{
    public class CasoDeUsoAtualizarCliente
    {
        private readonly ICacheService _cache;
        private readonly IClienteRepositorio _repositorio;

        public CasoDeUsoAtualizarCliente(ICacheService cache, IClienteRepositorio repositorio)
        {
            _cache = cache;
            _repositorio = repositorio;
        }

        public async Task<ClienteDto> AtualizarSaldoCliente(ClienteDto clienteDto)
        {
            var existente = await _repositorio.ObterPorCPFAsync(clienteDto.CPF);
            if (existente == null)
                throw new InvalidOperationException("Cliente não encontrado");

            var retorno = await _repositorio.AtualizarSaldo(new Domain.Entities.Cliente() 
            { 
                Id = clienteDto.Id, 
                CPF = clienteDto.CPF, 
                Nome = clienteDto.Nome, 
                ValorLimite = clienteDto.ValorLimite 
            });

            return new ClienteDto
            {
                Id = retorno.Id,
                Nome = retorno.Nome,
                CPF = retorno.CPF,
                ValorLimite = retorno.ValorLimite
            };
        }
    }
}
