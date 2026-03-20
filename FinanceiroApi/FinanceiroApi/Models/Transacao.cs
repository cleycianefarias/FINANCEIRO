using System.ComponentModel.DataAnnotations;
using FinanceiroApi.Enums;

namespace FinanceiroApi.Models;

public class Transacao
{
    public int Id { get; set; }

    [Required]
    [MaxLength(400)]
    public string Descricao { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal Valor { get; set; }

    [Required]
    public TipoTransacao Tipo { get; set; }

    [Required]
    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }

    [Required]
    public int PessoaId { get; set; }
    public Pessoa? Pessoa { get; set; }
}