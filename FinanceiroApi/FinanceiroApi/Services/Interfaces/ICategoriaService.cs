using FinanceiroApi.Dtos;
using FinanceiroApi.Models;

namespace FinanceiroApi.Services.Interfaces;

public interface ICategoriaService
{
    Task<List<Categoria>> ListarAsync();
    Task<Categoria?> ObterPorIdAsync(int id);
    Task<Categoria> CriarAsync(CriarCategoriaDto dto);
}