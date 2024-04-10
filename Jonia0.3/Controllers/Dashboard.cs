using Jonia0._3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jonia0._2.Controllers
{
    [Authorize(Policy = "Dashboard")]
    public class DashboardController : Controller
    {
        private readonly JoniaDbContext _context;

        public DashboardController(JoniaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ObtenerVentasTotales()
        {
            // Obtener la suma de los montos de todas las reservas
            decimal? ventasTotales = _context.Reservas.Sum(r => r.Total);

            // Devolver las ventas totales en formato JSON
            return Json(new { ventasTotales });
        }

        [HttpGet]
        public IActionResult ObtenerNumeroReservas()
        {
            int reservasTotales = _context.Reservas.Count();
            if (reservasTotales >= 0)
            {
                return Json(new { reservasTotales });
            }
            else
            {
                return BadRequest("No se pudo obtener el número total de reservas.");
            }
        }
    }
}