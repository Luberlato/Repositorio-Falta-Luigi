using System;
using System.Collections.Generic;

namespace VHBurguer.Domains;

public partial class Usuario
{
    public int UsuarioID { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public byte[]? Senha { get; set; }

    public bool? StatusUsuario { get; set; }

    public virtual ICollection<Produto> Produto { get; set; } = new List<Produto>();
}
