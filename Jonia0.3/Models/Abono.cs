using System;
using System.Collections.Generic;

namespace Jonia0._3.Models;

public partial class Abono
{
    public int IdAbono { get; set; }

    public int? IdReserva { get; set; }

    public DateOnly? FechaRegistro { get; set; }

    public decimal? ValorDeuda { get; set; }

    public decimal? SubtotalAbonado { get; set; }

    public decimal? TotalPendiente { get; set; }

    public double? Iva { get; set; }

    public decimal? TotalAbonado { get; set; }

    public double? Porcentaje { get; set; }

    public bool? Estado { get; set; }

    public virtual Reserva? IdReservaNavigation { get; set; }
}
