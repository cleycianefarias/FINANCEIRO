using FinanceiroApi.Data;
using FinanceiroApi.Dtos;
using FinanceiroApi.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinanceiroApi.Services.Interfaces;

namespace FinanceiroApi.Controllers;

/*
Ele gera relatórios financeiros a partir das transações.
Ele NÃO altera dados, só consulta e calcula totais.

Em resumo esse controller
 Consulta dados
 Agrupa por pessoa ou categoria
 Filtra receitas e despesas
 Soma valores
 Retorna DTO com relatório
*/

[ApiController]
[Route("api/[controller]")]
public class RelatoriosController : ControllerBase
{
    private readonly IRelatorioService _relatorioService;

    public RelatoriosController(IRelatorioService relatorioService)
    {
        _relatorioService = relatorioService;
    }

    [HttpGet("totais-por-pessoa")]
    public async Task<IActionResult> TotaisPorPessoa()
    {
        var response = await _relatorioService.ObterTotaisPorPessoaAsync();
        return Ok(response);
    }

    [HttpGet("totais-por-categoria")]
    public async Task<IActionResult> TotaisPorCategoria()
    {
        var response = await _relatorioService.ObterTotaisPorCategoriaAsync();
        return Ok(response);
    }
}
