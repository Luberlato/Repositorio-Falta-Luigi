using System;
using System.Collections.Generic;

namespace VHBurguer.Domains;

public partial class Promocao
{
    public int PromocaoID { get; set; }

    public string? Nome { get; set; }

    public DateTime DataExpiracao { get; set; }

    public bool StatusPromocao { get; set; }

    public virtual ICollection<ProdutoPromocao> ProdutoPromocao { get; set; } = new List<ProdutoPromocao>();
}
