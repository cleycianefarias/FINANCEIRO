using System.ComponentModel.DataAnnotations;

namespace FinanceiroApi.Dtos;

/*"Utilizo DTOs distintos para criação e atualização de pessoas, 
aplicando validações com DataAnnotations e incluindo propriedades calculadas, 
como a verificação de menoridade, para centralizar regras de negócio e evitar duplicação de lógica.*/
public class CriarPessoaDto
{
    [Required]
    [MaxLength(200)]
    public string Nome { get; set; } = string.Empty;

    [Range(0, 150)]
    public int Idade { get; set; }
}

public class AtualizarPessoaDto
{
    [Required]
    [MaxLength(200)]
    public string Nome { get; set; } = string.Empty;

    [Range(0, 150)]
    public int Idade { get; set; }

    public bool EhMenorDeIdade => Idade < 18;
}