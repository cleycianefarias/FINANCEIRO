using FinanceiroApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceiroApi.Data;

/*
Esse AppDbContext faz 3 coisas principais:
Define as tabelas:
    DbSet<Pessoa>
    DbSet<Categoria>
    DbSet<Transacao>

Configura propriedades das colunas
Nome: obrigatório, 200
Descricao: obrigatória, 400
Valor: decimal(18,2)

Configura relacionamentos
Pessoa 1:N Transações
Categoria 1:N Transações
Delete de Pessoa = Cascade
Delete de Categoria = Restrict


O AppDbContext é o contexto do Entity Framework Core responsável por 
mapear as entidades da aplicação para o banco de dados, definir as tabelas, 
configurar propriedades das colunas e estabelecer relacionamentos e comportamentos de exclusão entre as entidades.
*/
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Pessoa> Pessoas => Set<Pessoa>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Transacao> Transacoes => Set<Transacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Pessoa>()
            .Property(p => p.Nome)
            .HasMaxLength(200)
            .IsRequired();

        modelBuilder.Entity<Categoria>()
            .Property(c => c.Descricao)
            .HasMaxLength(400)
            .IsRequired();

        modelBuilder.Entity<Transacao>()
            .Property(t => t.Descricao)
            .HasMaxLength(400)
            .IsRequired();

        modelBuilder.Entity<Transacao>()
            .Property(t => t.Valor)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Transacao>()
            .HasOne(t => t.Pessoa)
            .WithMany(p => p.Transacoes)
            .HasForeignKey(t => t.PessoaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Transacao>()
            .HasOne(t => t.Categoria)
            .WithMany(c => c.Transacoes)
            .HasForeignKey(t => t.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}