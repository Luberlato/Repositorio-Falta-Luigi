using System;
using System.Collections.Generic;

namespace VHBurguer.Domains;

public partial class Log_AlteracaoProduto
{
    public int Log_AlteracaoProduto1 { get; set; }

    public DateTime DataAlteracao { get; set; }

    public string? NomeAnterior { get; set; }

    public decimal? ValorAnterior { get; set; }

    public int? ProdutoID { get; set; }

    public virtual Produto? Produto { get; set; }
}
