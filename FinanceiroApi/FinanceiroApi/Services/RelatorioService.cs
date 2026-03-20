using FinanceiroApi.Data;
using FinanceiroApi.Dtos;
using FinanceiroApi.Enums;
using FinanceiroApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceiroApi.Services;

public class RelatorioService : IRelatorioService
{
    private readonly AppDbContext _context;

    public RelatorioService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<RelatorioPessoasResponseDto> ObterTotaisPorPessoaAsync()
    {
        var pessoas = await _context.Pessoas
            .AsNoTracking()
            .Include(p => p.Transacoes)
            .OrderBy(p => p.Id)
            .Select(p => new TotalPorPessoaDto
            {
                PessoaId = p.Id,
                Nome = p.Nome,
                TotalReceitas = p.Transacoes
                    .Where(t => t.Tipo == TipoTransacao.Receita)
                    .Sum(t => (decimal?)t.Valor) ?? 0,
                TotalDespesas = p.Transacoes
                    .Where(t => t.Tipo == TipoTransacao.Despesa)
                    .Sum(t => (decimal?)t.Valor) ?? 0
            })
            .ToListAsync();

        var totalGeral = new TotalGeralDto
        {
            TotalReceitas = pessoas.Sum(p => p.TotalReceitas),
            TotalDespesas = pessoas.Sum(p => p.TotalDespesas)
        };

        return new RelatorioPessoasResponseDto
        {
            Pessoas = pessoas,
            TotalGeral = totalGeral
        };
    }

    public async Task<RelatorioCategoriasResponseDto> ObterTotaisPorCategoriaAsync()
    {
        var categorias = await _context.Categorias
            .AsNoTracking()
            .Include(c => c.Transacoes)
            .OrderBy(c => c.Id)
            .Select(c => new TotalPorCategoriaDto
            {
                CategoriaId = c.Id,
                Descricao = c.Descricao,
                TotalReceitas = c.Transacoes
                    .Where(t => t.Tipo == TipoTransacao.Receita)
                    .Sum(t => (decimal?)t.Valor) ?? 0,
                TotalDespesas = c.Transacoes
                    .Where(t => t.Tipo == TipoTransacao.Despesa)
                    .Sum(t => (decimal?)t.Valor) ?? 0
            })
            .ToListAsync();

        var totalGeral = new TotalGeralDto
        {
            TotalReceitas = categorias.Sum(c => c.TotalReceitas),
            TotalDespesas = categorias.Sum(c => c.TotalDespesas)
        };

        return new RelatorioCategoriasResponseDto
        {
            Categorias = categorias,
            TotalGeral = totalGeral
        };
    }
}