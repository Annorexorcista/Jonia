using System;
using System.Collections.Generic;

namespace Jonia0._3.Models;

public partial class Habitacione
{
    public int IdHabitacion { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public int? IdTipo { get; set; }

    public decimal? Precio { get; set; }

    public bool? Estado { get; set; }

    public virtual TipoHabitacion? IdTipoNavigation { get; set; }

    public virtual ICollection<Paquete> Paquetes { get; set; } = new List<Paquete>();
}
