using System;
using System.Collections.Generic;

namespace Jonia0._3.Models;

public partial class TipoHabitacion
{
    public int IdTipo { get; set; }

    public string? Nombre { get; set; }

    public int? NroPersonas { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<Habitacione> Habitaciones { get; set; } = new List<Habitacione>();
}
