using FinanceiroApi.Data;
using FinanceiroApi.Dtos;
using FinanceiroApi.Models;
using FinanceiroApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceiroApi.Services;

/*Esse service encapsula o CRUD de pessoas utilizando Entity Framework Core, separando a lógica de acesso a dados do controller e permitindo uma melhor organização e manutenção da aplicação.*/

public class PessoaService : IPessoaService
{
    private readonly AppDbContext _context;

    public PessoaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Pessoa>> ListarAsync()
    {
        return await _context.Pessoas
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .ToListAsync();
    }

    public async Task<Pessoa?> ObterPorIdAsync(int id)
    {
        return await _context.Pessoas
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Pessoa> CriarAsync(CriarPessoaDto dto)
    {
        var pessoa = new Pessoa
        {
            Nome = dto.Nome,
            Idade = dto.Idade
        };

        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();

        return pessoa;
    }

    public async Task<Pessoa?> EditarAsync(int id, AtualizarPessoaDto dto)
    {
        var pessoa = await _context.Pessoas.FindAsync(id);
        if (pessoa is null) return null;

        pessoa.Nome = dto.Nome;
        pessoa.Idade = dto.Idade;

        await _context.SaveChangesAsync();
        return pessoa;
    }

    public async Task<bool> DeletarAsync(int id)
    {
        var pessoa = await _context.Pessoas.FindAsync(id);
        if (pessoa is null) return false;

        _context.Pessoas.Remove(pessoa);
        await _context.SaveChangesAsync();

        return true;
    }
}