using FinanceiroApi.Dtos;
using FinanceiroApi.Models;

namespace FinanceiroApi.Services.Interfaces;

public interface IPessoaService
{
    Task<List<Pessoa>> ListarAsync();
    Task<Pessoa?> ObterPorIdAsync(int id);
    Task<Pessoa> CriarAsync(CriarPessoaDto dto);
    Task<Pessoa?> EditarAsync(int id, AtualizarPessoaDto dto);
    Task<bool> DeletarAsync(int id);
}