using FinanceiroApi.Dtos;
using FinanceiroApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceiroApi.Controllers;

/*Esse controller gerencia o CRUD completo de pessoas 
  1° Lista todas as pessoas
  2° Lista as pessoas por Id
  3° Cadastra uma nova pessoa
  4° Edita uma pessoa
  5° Deleta uma pessoa

  Quando cria um novo registro: Retorna 201 Created
  Quando atualiza um existente: Retorna 200 OK
  Quando deleta: Retorna 204 NoContent
*/

[ApiController]
[Route("api/[controller]")]
public class PessoasController : ControllerBase
{
    private readonly IPessoaService _pessoaService;

    public PessoasController(IPessoaService pessoaService)
    {
        _pessoaService = pessoaService;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var pessoas = await _pessoaService.ListarAsync();
        return Ok(pessoas);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Obter(int id)
    {
        var pessoa = await _pessoaService.ObterPorIdAsync(id);
        if (pessoa is null) return NotFound();

        return Ok(pessoa);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarPessoaDto dto)
    {
        var pessoa = await _pessoaService.CriarAsync(dto);
        return CreatedAtAction(nameof(Obter), new { id = pessoa.Id }, pessoa);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Editar(int id, [FromBody] AtualizarPessoaDto dto)
    {
        var pessoa = await _pessoaService.EditarAsync(id, dto);
        if (pessoa is null) return NotFound();

        return Ok(pessoa);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Deletar(int id)
    {
        var removido = await _pessoaService.DeletarAsync(id);
        if (!removido) return NotFound();

        return NoContent();
    }
}