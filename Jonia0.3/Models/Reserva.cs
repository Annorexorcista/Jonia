using System;
using System.Collections.Generic;

namespace Jonia0._3.Models;

public partial class Reserva
{
    public int IdReserva { get; set; }

    public int? NroDocumentoCliente { get; set; }

    public int? NroDocumentoTrabajador { get; set; }

    public string? Informacion { get; set; }

    public DateOnly? FechaRegistro { get; set; }

    public DateOnly? FechaEntrada { get; set; }

    public DateOnly? FechaSalida { get; set; }

    public int? NumeroPersonas { get; set; }

    public int? MetodoPago { get; set; }

    public int? Estado { get; set; }

    public double? Iva { get; set; }

    public decimal? Subtotal { get; set; }

    public decimal? Total { get; set; }

    public virtual ICollection<Abono> Abonos { get; set; } = new List<Abono>();

    public virtual ICollection<DetalleReservaPaquete> DetalleReservaPaquetes { get; set; } = new List<DetalleReservaPaquete>();

    public virtual ICollection<DetalleReservaServicio> DetalleReservaServicios { get; set; } = new List<DetalleReservaServicio>();

    public virtual Estado? EstadoNavigation { get; set; }

    public virtual MetodoPago? MetodoPagoNavigation { get; set; }

    public virtual Cliente? NroDocumentoClienteNavigation { get; set; }

    public virtual Usuario? NroDocumentoTrabajadorNavigation { get; set; }
}
