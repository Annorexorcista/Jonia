using System;
using System.Collections.Generic;

namespace Jonia0._3.Models;

public partial class Paquete
{
    public int IdPaquete { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public decimal? Precio { get; set; }

    public int? IdHabitacion { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<DetalleReservaPaquete> DetalleReservaPaquetes { get; set; } = new List<DetalleReservaPaquete>();

    public virtual Habitacione? IdHabitacionNavigation { get; set; }

    public virtual ICollection<PaquetesServicio> PaquetesServicios { get; set; } = new List<PaquetesServicio>();
}
