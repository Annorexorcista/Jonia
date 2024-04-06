using System;
using System.Collections.Generic;

namespace Jonia0._3.Models;

public partial class DetalleReservaPaquete
{
    public int IdRp { get; set; }

    public int? IdReserva { get; set; }

    public int? IdPaquete { get; set; }

    public int? Cantidad { get; set; }

    public decimal? Precio { get; set; }

    public virtual Paquete? IdPaqueteNavigation { get; set; }

    public virtual Reserva? IdReservaNavigation { get; set; }
}
