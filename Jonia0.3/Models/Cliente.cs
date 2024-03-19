using System;
using System.Collections.Generic;

namespace Jonia0._3.Models;

public partial class Cliente
{
    public int NroDocumento { get; set; }

    public int? TipoDocumento { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Correo { get; set; }

    public string? Contrasena { get; set; }

    public string? ConfirmarClave { get; set; }

    public string? Celular { get; set; }

    public string? Direccion { get; set; }

    public DateTime? FechaNacimiento { get; set; }

    public int? IdRol { get; set; }

    public bool? Estado { get; set; }

    public bool? Confirmado { get; set; }

    public bool? Restablecer { get; set; }

    public string? Token { get; set; }

    public virtual Rol? IdRolNavigation { get; set; }

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    public virtual TipoDocumento? TipoDocumentoNavigation { get; set; }
}
