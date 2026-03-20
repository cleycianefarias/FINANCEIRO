namespace FinanceiroApi.Dtos;

public class TotalPorPessoaDto
{
    public int PessoaId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal TotalReceitas { get; set; }
    public decimal TotalDespesas { get; set; }
    public decimal Saldo => TotalReceitas - TotalDespesas;
}

public class TotalPorCategoriaDto
{
    public int CategoriaId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal TotalReceitas { get; set; }
    public decimal TotalDespesas { get; set; }
    public decimal Saldo => TotalReceitas - TotalDespesas;
}

public class TotalGeralDto
{
    public decimal TotalReceitas { get; set; }
    public decimal TotalDespesas { get; set; }
    public decimal Saldo => TotalReceitas - TotalDespesas;
}

public class RelatorioPessoasResponseDto
{
    public List<TotalPorPessoaDto> Pessoas { get; set; } = new();
    public TotalGeralDto TotalGeral { get; set; } = new();
}

public class RelatorioCategoriasResponseDto
{
    public List<TotalPorCategoriaDto> Categorias { get; set; } = new();
    public TotalGeralDto TotalGeral { get; set; } = new();
}