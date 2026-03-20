using System.ComponentModel.DataAnnotations;
using FinanceiroApi.Enums;

namespace FinanceiroApi.Dtos;

/*Esses DTOs são usados para estruturar 
respostas de relatórios financeiros, incluindo propriedades calculadas como saldo, 
o que ajuda a centralizar regras e manter o controller mais limpo.*/

public class CriarTransacaoDto
{
    [Required]
    [MaxLength(400)]
    public string Descricao { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal Valor { get; set; }

    [Required]
    public TipoTransacao Tipo { get; set; }

    [Required]
    public int CategoriaId { get; set; }

    [Required]
    public int PessoaId { get; set; }
}