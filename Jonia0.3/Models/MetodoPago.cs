﻿using System;
using System.Collections.Generic;

namespace Jonia0._3.Models;

public partial class MetodoPago
{
    public int IdMp { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
