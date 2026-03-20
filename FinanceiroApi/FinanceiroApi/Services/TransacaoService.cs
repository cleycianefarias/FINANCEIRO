using FinanceiroApi.Data;
using FinanceiroApi.Dtos;
using FinanceiroApi.Enums;
using FinanceiroApi.Models;
using FinanceiroApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceiroApi.Services;

public class TransacaoService : ITransacaoService
{
    private readonly AppDbContext _context;

    public TransacaoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<object>> ListarAsync()
    {
        var transacoes = await _context.Transacoes
            .AsNoTracking()
            .Include(t => t.Pessoa)
            .Include(t => t.Categoria)
            .OrderBy(t => t.Id)
            .Select(t => new
            {
                t.Id,
                t.Descricao,
                t.Valor,
                t.Tipo,
                t.PessoaId,
                Pessoa = t.Pessoa!.Nome,
                t.CategoriaId,
                Categoria = t.Categoria!.Descricao
            })
            .ToListAsync();

        return transacoes.Cast<object>().ToList();
    }

    public async Task<object?> ObterPorIdAsync(int id)
    {
        var transacao = await _context.Transacoes
            .AsNoTracking()
            .Include(t => t.Pessoa)
            .Include(t => t.Categoria)
            .Where(t => t.Id == id)
            .Select(t => new
            {
                t.Id,
                t.Descricao,
                t.Valor,
                t.Tipo,
                t.PessoaId,
                Pessoa = t.Pessoa!.Nome,
                t.CategoriaId,
                Categoria = t.Categoria!.Descricao
            })
            .FirstOrDefaultAsync();

        return transacao;
    }

    public async Task<(bool Sucesso, string? Erro, object? Dados, int? IdCriado)> CriarAsync(CriarTransacaoDto dto)
    {
        var pessoa = await _context.Pessoas.FindAsync(dto.PessoaId);
        if (pessoa is null)
            return (false, "Pessoa não encontrada.", null, null);

        var categoria = await _context.Categorias.FindAsync(dto.CategoriaId);
        if (categoria is null)
            return (false, "Categoria não encontrada.", null, null);

        if (pessoa.Idade < 18 && dto.Tipo == TipoTransacao.Receita)
            return (false, "Menor de idade só pode possuir transações do tipo despesa.", null, null);

        var categoriaValida =
            categoria.Finalidade == FinalidadeCategoria.Ambas ||
            (categoria.Finalidade == FinalidadeCategoria.Despesa && dto.Tipo == TipoTransacao.Despesa) ||
            (categoria.Finalidade == FinalidadeCategoria.Receita && dto.Tipo == TipoTransacao.Receita);

        if (!categoriaValida)
            return (false, "A categoria informada não é compatível com o tipo da transação.", null, null);

        var transacao = new Transacao
        {
            Descricao = dto.Descricao,
            Valor = dto.Valor,
            Tipo = dto.Tipo,
            CategoriaId = dto.CategoriaId,
            PessoaId = dto.PessoaId
        };

        _context.Transacoes.Add(transacao);
        await _context.SaveChangesAsync();

        var resposta = new
        {
            transacao.Id,
            transacao.Descricao,
            transacao.Valor,
            transacao.Tipo,
            transacao.PessoaId,
            Pessoa = pessoa.Nome,
            transacao.CategoriaId,
            Categoria = categoria.Descricao
        };

        return (true, null, resposta, transacao.Id);
    }
}