using FinanceiroApi.Data;
using FinanceiroApi.Dtos;
using FinanceiroApi.Models;
using FinanceiroApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceiroApi.Services;

/*Esse service encapsula a lógica de acesso a dados de categorias, separando a responsabilidade do controller e permitindo centralizar operações como listagem, busca e criação utilizando Entity Framework Core.*/

public class CategoriaService : ICategoriaService
{
    private readonly AppDbContext _context;

    public CategoriaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Categoria>> ListarAsync()
    {
        return await _context.Categorias //tabela no banco
            .AsNoTracking() //melhora performance (não rastreia mudanças)
            .OrderBy(c => c.Id) //ordena por Id
            .ToListAsync(); //executa a query
    }

    public async Task<Categoria?> ObterPorIdAsync(int id)
    {
        return await _context.Categorias
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Categoria> CriarAsync(CriarCategoriaDto dto)
    {
        var categoria = new Categoria
        {
            Descricao = dto.Descricao,
            Finalidade = dto.Finalidade
        };

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        return categoria;
    }
}