using System.ComponentModel.DataAnnotations;
using FinanceiroApi.Enums;

namespace FinanceiroApi.Models;

public class Categoria
{
    public int Id { get; set; }

    [Required]
    [MaxLength(400)]
    public string Descricao { get; set; } = string.Empty;

    [Required]
    public FinalidadeCategoria Finalidade { get; set; }

    public List<Transacao> Transacoes { get; set; } = new();
}