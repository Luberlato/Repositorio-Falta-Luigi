using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VHBurguer.Domains;

namespace VHBurguer.Contexts;

public partial class VH_BurguerContext : DbContext
{
    public VH_BurguerContext()
    {
    }

    public VH_BurguerContext(DbContextOptions<VH_BurguerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<Log_AlteracaoProduto> Log_AlteracaoProduto { get; set; }

    public virtual DbSet<Produto> Produto { get; set; }

    public virtual DbSet<ProdutoPromocao> ProdutoPromocao { get; set; }

    public virtual DbSet<Promocao> Promocao { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=VH_Burguer;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.CategoriaID).HasName("PK__Categori__F353C1C512780372");

            entity.Property(e => e.Nome)
                .HasMaxLength(60)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Log_AlteracaoProduto>(entity =>
        {
            entity.HasKey(e => e.Log_AlteracaoProduto1).HasName("PK__Log_Alte__7FD7B5E2885E50D1");

            entity.Property(e => e.Log_AlteracaoProduto1).HasColumnName("Log_AlteracaoProduto");
            entity.Property(e => e.DataAlteracao).HasPrecision(0);
            entity.Property(e => e.NomeAnterior)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ValorAnterior).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Produto).WithMany(p => p.Log_AlteracaoProduto)
                .HasForeignKey(d => d.ProdutoID)
                .HasConstraintName("FK__Log_Alter__Produ__5FB337D6");
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.ProdutoID).HasName("PK__Produto__9C8800C3631744A2");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("trg_ExclusaoProduto");
                    tb.HasTrigger("trg_LogAlteracaoProduto");
                });

            entity.HasIndex(e => e.Nome, "UQ__Produto__7D8FE3B26E3EAE98").IsUnique();

            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Preco).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.StatusProduto).HasDefaultValue(false);

            entity.HasOne(d => d.Usuario).WithMany(p => p.Produto)
                .HasForeignKey(d => d.UsuarioID)
                .HasConstraintName("FK__Produto__Usuario__5070F446");

            entity.HasMany(d => d.Categoria).WithMany(p => p.Produto)
                .UsingEntity<Dictionary<string, object>>(
                    "ProdutoCategoria",
                    r => r.HasOne<Categoria>().WithMany()
                        .HasForeignKey("CategoriaID")
                        .HasConstraintName("Fk_ProdutoCategoria_Categoria"),
                    l => l.HasOne<Produto>().WithMany()
                        .HasForeignKey("ProdutoID")
                        .HasConstraintName("Fk_ProdutoCategoria_Produto"),
                    j =>
                    {
                        j.HasKey("ProdutoID", "CategoriaID").HasName("Pk_ProdutoCategoria");
                    });
        });

        modelBuilder.Entity<ProdutoPromocao>(entity =>
        {
            entity.HasKey(e => new { e.ProdutoID, e.PromocaoID }).HasName("Pk_ProdutoPromocao");

            entity.Property(e => e.PrecoAtual).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Produto).WithMany(p => p.ProdutoPromocao)
                .HasForeignKey(d => d.ProdutoID)
                .HasConstraintName("Fk_ProdutoPromocao_Produto");

            entity.HasOne(d => d.Promocao).WithMany(p => p.ProdutoPromocao)
                .HasForeignKey(d => d.PromocaoID)
                .HasConstraintName("Fk_ProdutoPromocao_Promocao");
        });

        modelBuilder.Entity<Promocao>(entity =>
        {
            entity.HasKey(e => e.PromocaoID).HasName("PK__Promocao__254B583DD2CD4F1B");

            entity.Property(e => e.DataExpiracao).HasPrecision(0);
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StatusPromocao).HasDefaultValue(true);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioID).HasName("PK__Usuario__2B3DE79896C2AC9F");

            entity.ToTable(tb => tb.HasTrigger("trg_ExclusaoUsuario"));

            entity.HasIndex(e => e.Email, "UQ__Usuario__A9D10534EB7DF870").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Senha).HasMaxLength(32);
            entity.Property(e => e.StatusUsuario).HasDefaultValue(true);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
