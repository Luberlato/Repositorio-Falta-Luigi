using System;
using System.Collections.Generic;

namespace VHBurguer.Domains;

public partial class Produto
{
    public int ProdutoID { get; set; }

    public string Nome { get; set; } = null!;

    public decimal Preco { get; set; }

    public string Descricao { get; set; } = null!;

    public byte[] Imagem { get; set; } = null!;

    public bool? StatusProduto { get; set; }

    public int? UsuarioID { get; set; }

    public virtual ICollection<Log_AlteracaoProduto> Log_AlteracaoProduto { get; set; } = new List<Log_AlteracaoProduto>();

    public virtual ICollection<ProdutoPromocao> ProdutoPromocao { get; set; } = new List<ProdutoPromocao>();

    public virtual Usuario? Usuario { get; set; }

    public virtual ICollection<Categoria> Categoria { get; set; } = new List<Categoria>();
}
