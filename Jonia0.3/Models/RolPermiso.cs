using System;
using System.Collections.Generic;

namespace Jonia0._3.Models;

public partial class RolPermiso
{
    public int IdRd { get; set; }

    public int? IdRol { get; set; }

    public int? IdPermiso { get; set; }

    public virtual Permiso? IdPermisoNavigation { get; set; }

    public virtual Rol? IdRolNavigation { get; set; }
}
