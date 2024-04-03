using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Jonia0._3.Models;

namespace Jonia0._3.Controllers
{
    public class ReservasController : Controller
    {
        private readonly JoniaDbContext _context;

        public ReservasController(JoniaDbContext context)
        {
            _context = context;
        }

        // GET: Reservas
        public async Task<IActionResult> Index(string buscar)
        {
            var metodo = _context.MetodoPagos.ToList();
            ViewBag.Metodo = metodo;

            var estado = _context.Estados.ToList();
            ViewBag.Estado = estado;

            var reservas = from reserva in _context.Reservas select reserva;

            if(!String.IsNullOrEmpty(buscar))
                reservas = reservas.Where(s=>s.Informacion!.Contains(buscar));

            return View(await reservas.ToListAsync());
        }

        public IActionResult obtenerNroPersonas(int id)
        {
            var nroPer = _context.Habitaciones.Include(s => s.IdTipoNavigation).Where(j => j.IdHabitacion == id).Select(p => p.IdTipoNavigation.NroPersonas).FirstOrDefault();

            return Json(new { nro = nroPer });
        }

        public IActionResult obtenerCostoPaquete(int id)
        {
            var costoPaq = _context.Paquetes.Where(s => s.IdPaquete == id).Select(s => s.Precio).FirstOrDefault();

            return Json(new { costo = costoPaq });
        }

        public IActionResult obtenerNombreHabitacion(int id)
        {
            var nombreHab = _context.Paquetes.Include(s => s.IdHabitacionNavigation).Where(j => j.IdPaquete == id).Select(p => p.IdHabitacionNavigation.Nombre).FirstOrDefault();

            return Json(new { nombre = nombreHab });
        }
        public IActionResult obtenerTipoHabitacion(int id)
        {
            var tipoHab = _context.Habitaciones.Include(s => s.IdTipoNavigation).Where(j => j.IdHabitacion == id).Select(p => p.IdTipoNavigation.Nombre).FirstOrDefault();

            return Json(new { nombre = tipoHab });
        }
        public IActionResult obtenerCostoServicio(int id)
        {
            var costoSer = _context.Servicios.Where(s => s.IdServicio == id).Select(s => s.Precio).FirstOrDefault();

            return Json(new { costo = costoSer });
        }
        public IActionResult obtenerTipoServicio(int id)
        {
            var tipoSer = _context.Servicios.Include(s => s.TipoServicioNavigation).Where(j => j.IdServicio == id).Select(p => p.TipoServicioNavigation.Nombre).FirstOrDefault();

            return Json(new { nombre = tipoSer });
        }

        // GET: Reservas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.EstadoNavigation)
                .Include(r => r.MetodoPagoNavigation)
                .Include(r => r.NroDocumentoClienteNavigation)
                .Include(r => r.NroDocumentoTrabajadorNavigation)
                .FirstOrDefaultAsync(m => m.IdReserva == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // GET: Reservas/Create
        public IActionResult Create()
        {
            ViewData["Estado"] = new SelectList(_context.Estados, "IdEstado", "IdEstado");
            var metodo = _context.MetodoPagos.ToList();
            ViewBag.Metodo = metodo;

            var clientes = _context.Clientes.Select(u => new
            {
                Id = u.NroDocumento,
                NombreCompleto = $"{u.Nombre} {u.Apellido}"
            }).ToList();
            ViewBag.Clientes = new SelectList(clientes, "Id", "NombreCompleto");

            var trabajadores = _context.Usuarios.Select(u => new
            {
                Id = u.NroDocumento,
                NombreCompleto = $"{u.Nombre} {u.Apellido}" // Concatenar nombre y apellido
            }).ToList();

            var paquete = _context.Paquetes.ToList();
            ViewBag.Paquetes = paquete;

            var servicio = _context.Servicios.Include(s => s.TipoServicioNavigation).Where(s => s.TipoServicio != 1).ToList();
            ViewBag.Servicios = servicio;

            ViewBag.Trabajadores = new SelectList(trabajadores, "Id", "NombreCompleto");

            return View();
        }

        // POST: Reservas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdReserva,NroDocumentoCliente,NroDocumentoTrabajador,Informacion,FechaRegistro,FechaEntrada,FechaSalida,NumeroAdultos,NumeroNinos,MetodoPago,HoraLlegada,HoraSalida,Estado,Iva,Subtotal,Total")] Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Estado"] = new SelectList(_context.Estados, "IdEstado", "IdEstado", reserva.Estado);
            ViewData["MetodoPago"] = new SelectList(_context.MetodoPagos, "IdMp", "IdMp", reserva.MetodoPago);

            var clientes = _context.Clientes.Select(u => new
            {
                Id = u.NroDocumento,
                NombreCompleto = $"{u.Nombre} {u.Apellido}"
            }).ToList();
            ViewBag.Clientes = new SelectList(clientes, "Id", "NombreCompleto", reserva.NroDocumentoCliente);

            var trabajadores = _context.Usuarios.Select(u => new
            {
                Id = u.NroDocumento,
                NombreCompleto = $"{u.Nombre} {u.Apellido}" // Concatenar nombre y apellido
            }).ToList();

            var paquete = _context.Paquetes.ToList();
            ViewBag.Paquetes = paquete;


            ViewBag.Trabajadores = new SelectList(trabajadores, "Id", "NombreCompleto", reserva.NroDocumentoTrabajador); // Marcar el trabajador seleccionado si hay un error
            return View(reserva);
        }

        // GET: Reservas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }
            ViewData["Estado"] = new SelectList(_context.Estados, "IdEstado", "IdEstado", reserva.Estado);
            ViewData["MetodoPago"] = new SelectList(_context.MetodoPagos, "IdMp", "IdMp", reserva.MetodoPago);
            var metodo = _context.MetodoPagos.ToList();
            ViewBag.Metodo = metodo;

            var clientes = _context.Clientes.Select(u => new
            {
                Id = u.NroDocumento,
                NombreCompleto = $"{u.Nombre} {u.Apellido}"
            }).ToList();
            ViewBag.Clientes = new SelectList(clientes, "Id", "NombreCompleto");

            var trabajadores = _context.Usuarios.Select(u => new
            {
                Id = u.NroDocumento,
                NombreCompleto = $"{u.Nombre} {u.Apellido}" // Concatenar nombre y apellido
            }).ToList();

            var paquete = _context.Paquetes.ToList();
            ViewBag.Paquetes = paquete;

            ViewBag.Trabajadores = new SelectList(trabajadores, "Id", "NombreCompleto");
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdReserva,NroDocumentoCliente,NroDocumentoTrabajador,Informacion,FechaRegistro,FechaEntrada,FechaSalida,NumeroAdultos,NumeroNinos,MetodoPago,HoraLlegada,HoraSalida,Estado,Iva,Subtotal,Total")] Reserva reserva)
        {
            if (id != reserva.IdReserva)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reserva);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaExists(reserva.IdReserva))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Estado"] = new SelectList(_context.Estados, "IdEstado", "IdEstado", reserva.Estado);
            ViewData["MetodoPago"] = new SelectList(_context.MetodoPagos, "IdMp", "IdMp", reserva.MetodoPago);
            var clientes = _context.Clientes.Select(u => new
            {
                Id = u.NroDocumento,
                NombreCompleto = $"{u.Nombre} {u.Apellido}"
            }).ToList();
            ViewBag.Clientes = new SelectList(clientes, "Id", "NombreCompleto", reserva.NroDocumentoCliente);

            var trabajadores = _context.Usuarios.Select(u => new
            {
                Id = u.NroDocumento,
                NombreCompleto = $"{u.Nombre} {u.Apellido}" // Concatenar nombre y apellido
            }).ToList();

            var paquete = _context.Paquetes.ToList();
            ViewBag.Paquetes = paquete;


            ViewBag.Trabajadores = new SelectList(trabajadores, "Id", "NombreCompleto", reserva.NroDocumentoTrabajador); // Marcar el trabajador seleccionado si hay un error
            return View(reserva);
        }

        // GET: Reservas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.EstadoNavigation)
                .Include(r => r.MetodoPagoNavigation)
                .Include(r => r.NroDocumentoClienteNavigation)
                .Include(r => r.NroDocumentoTrabajadorNavigation)
                .FirstOrDefaultAsync(m => m.IdReserva == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.IdReserva == id);
        }
    }
}
