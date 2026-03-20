using FinanceiroApi.Dtos;
using FinanceiroApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceiroApi.Controllers;

/*
Esse controller agora fica responsável apenas pela camada HTTP.
As regras de negócio e acesso ao banco estão no service.
*/
[ApiController]
[Route("api/[controller]")]
public class TransacoesController : ControllerBase
{
    private readonly ITransacaoService _transacaoService;

    public TransacoesController(ITransacaoService transacaoService)
    {
        _transacaoService = transacaoService;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var transacoes = await _transacaoService.ListarAsync();
        return Ok(transacoes);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Obter(int id)
    {
        var transacao = await _transacaoService.ObterPorIdAsync(id);
        if (transacao is null) return NotFound();

        return Ok(transacao);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarTransacaoDto dto)
    {
        var resultado = await _transacaoService.CriarAsync(dto);

        if (!resultado.Sucesso)
            return BadRequest(resultado.Erro);

        return CreatedAtAction(nameof(Obter), new { id = resultado.IdCriado }, resultado.Dados);
    }
}