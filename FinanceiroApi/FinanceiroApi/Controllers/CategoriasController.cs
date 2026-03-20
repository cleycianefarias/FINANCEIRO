using FinanceiroApi.Dtos;
using FinanceiroApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceiroApi.Controllers;

/*
    Esse controller faz 03 operações
    1° lista todas as categorias
    2° lista categorias por ID
    3° cria uma nova categoria

    Ele delega a lógica para o service.
*/
[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var categorias = await _categoriaService.ListarAsync();
        return Ok(categorias);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Obter(int id)
    {
        var categoria = await _categoriaService.ObterPorIdAsync(id);
        if (categoria is null) return NotFound();

        return Ok(categoria);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarCategoriaDto dto)
    {
        var categoria = await _categoriaService.CriarAsync(dto);
        return CreatedAtAction(nameof(Obter), new { id = categoria.Id }, categoria);
    }
}