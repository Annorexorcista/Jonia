using System;
using System.Collections.Generic;

namespace Jonia0._3.Models;

public partial class DetalleReservaServicio
{
    public int IdRs { get; set; }

    public int? IdReserva { get; set; }

    public int? IdServicio { get; set; }

    public int? Cantidad { get; set; }

    public decimal? Precio { get; set; }

    public virtual Reserva? IdReservaNavigation { get; set; }

    public virtual Servicio? IdServicioNavigation { get; set; }
}
