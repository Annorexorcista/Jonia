using System;
using System.Collections.Generic;

namespace Jonia0._3.Models;

public partial class TipoDocumento
{
    public int IdTd { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
