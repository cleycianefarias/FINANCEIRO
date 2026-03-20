namespace FinanceiroApi.Services.Interfaces;

using FinanceiroApi.Dtos;

public interface IRelatorioService
{
    Task<RelatorioPessoasResponseDto> ObterTotaisPorPessoaAsync();
    Task<RelatorioCategoriasResponseDto> ObterTotaisPorCategoriaAsync();
}