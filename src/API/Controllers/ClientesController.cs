using Microsoft.AspNetCore.Mvc;
using Clientes.API.Application.CasosUso;
using Clientes.API.Domain.Entities;
using Clientes.API.API.Filters;
using Clientes.API.Domain.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Clientes.API.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly CasoDeUsoCadastrarCliente _casoCadastrar;
    private readonly CasoDeUsoListarClientes _casoListar;

    public ClientesController(CasoDeUsoCadastrarCliente casoCadastrar, CasoDeUsoListarClientes casoListar)
    {
        _casoCadastrar = casoCadastrar;
        _casoListar = casoListar;
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidarTokenAttribute))]
    public async Task<IActionResult> Cadastrar([FromBody] ClienteDto clienteDto)
    {
        try
        {           
            var resultado = await _casoCadastrar.ExecutarAsync(clienteDto);
            return CreatedAtAction(nameof(ObterTodos), resultado);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensagem = ex.Message });
        }
    }

    [HttpGet]
    [ServiceFilter(typeof(ValidarTokenAttribute))]
    public async Task<IActionResult> ObterTodos()
    {
        var lista = await _casoListar.ListarClientesAsync();
        var dtos = lista.Select(c => new ClienteDto
        {
            Id = c.Id,
            Nome = c.Nome,
            CPF = c.CPF,
            ValorLimite = c.ValorLimite
        });
        return Ok(dtos);
    }

    [HttpGet("{Id}")]
    [ServiceFilter(typeof(ValidarTokenAttribute))]
    public async Task<IActionResult> ObterCliente(Guid Id)
    {
        var lista = await _casoListar.ListarClientesAsync();
        
        var dto = lista.Where(c => c.Id == Id).Select(c => new ClienteDto
        {
            Id = c.Id,
            Nome = c.Nome,
            CPF = c.CPF,
            ValorLimite = c.ValorLimite
        }).FirstOrDefault();

        return Ok(dto);
    }

    [HttpPatch("atualizar-saldo")]
    [ServiceFilter(typeof(ValidarTokenAttribute))]
    public async Task<IActionResult> AtualizarSaldo(Guid Id, decimal valor)
    {
       var retorno = await _casoCadastrar.AtualizarSaldoClienteAsync(new ClienteDto() { Id = Id, ValorLimite = valor });
        return Ok(retorno);
    }
}
