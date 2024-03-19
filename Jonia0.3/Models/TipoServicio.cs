using System;
using System.Collections.Generic;

namespace Jonia0._3.Models;

public partial class TipoServicio
{
    public int IdTs { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
