using System.ComponentModel.DataAnnotations;
using FinanceiroApi.Enums;

namespace FinanceiroApi.Dtos;

/*Esse DTO é usado para entrada de dados na criação de categorias, aplicando validações com DataAnnotations e garantindo que apenas os campos necessários sejam recebidos pela API.*/
public class CriarCategoriaDto
{
    [Required]
    [MaxLength(400)]
    public string Descricao { get; set; } = string.Empty;

    [Required]
    public FinalidadeCategoria Finalidade { get; set; }
}