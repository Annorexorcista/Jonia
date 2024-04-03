namespace Jonia0._3.Models
{
    public class ReservaViewModel
    {
        public Reserva Reserva { get; set; }
        public List<DetalleReservaPaquete> paquetes_seleccionados { get; set; }
        public List<DetalleReservaServicio> servicios_seleccionados { get; set; }
    }
}
