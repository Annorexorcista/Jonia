using System;
using System.Collections.Generic;

namespace Jonia0._3.Models;

public partial class PaquetesServicio
{
    public int IdPs { get; set; }

    public int? IdPaquete { get; set; }

    public int? IdServicio { get; set; }

    public decimal? Precio { get; set; }

    public virtual Paquete? IdPaqueteNavigation { get; set; }

    public virtual Servicio? IdServicioNavigation { get; set; }
}
