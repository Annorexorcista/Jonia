using System;
using System.Collections.Generic;

namespace Jonia0._3.Models;

public partial class Servicio
{
    public int IdServicio { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public int? TipoServicio { get; set; }

    public decimal? Precio { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<DetalleReservaServicio> DetalleReservaServicios { get; set; } = new List<DetalleReservaServicio>();

    public virtual ICollection<PaquetesServicio> PaquetesServicios { get; set; } = new List<PaquetesServicio>();

    public virtual TipoServicio? TipoServicioNavigation { get; set; }
}
