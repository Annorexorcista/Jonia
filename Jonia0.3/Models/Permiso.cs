using System;
using System.Collections.Generic;

namespace Jonia0._3.Models;

public partial class Permiso
{
    public int IdPermiso { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();
}
