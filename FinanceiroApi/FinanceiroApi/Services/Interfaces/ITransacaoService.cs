using FinanceiroApi.Dtos;

namespace FinanceiroApi.Services.Interfaces;

public interface ITransacaoService
{
    Task<List<object>> ListarAsync();
    Task<object?> ObterPorIdAsync(int id);
    Task<(bool Sucesso, string? Erro, object? Dados, int? IdCriado)> CriarAsync(CriarTransacaoDto dto);
}