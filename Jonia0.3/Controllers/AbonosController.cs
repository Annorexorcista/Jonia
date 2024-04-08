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
    public class AbonosController : Controller
    {
        private readonly JoniaDbContext _context;

        public AbonosController(JoniaDbContext context)
        {
            _context = context;
        }

        // GET: Abonos
        public async Task<IActionResult> Index()
        {
            var joniaDbContext = _context.Abonos.Include(a => a.IdReservaNavigation);
            return View(await joniaDbContext.ToListAsync());
        }
        [HttpGet]
        public IActionResult IndividualIndex(int idReserva)
        {

            var abonosReserva = _context.Abonos
         .Where(a => a.IdReserva == idReserva)
         .Include(a => a.IdReservaNavigation)
         .ToList();
            foreach (var abono in abonosReserva)
            {
                abono.Porcentaje = Math.Round((double)abono.Porcentaje * 100, 2);
            }

            return View(abonosReserva);

        }

        public IActionResult obtenerDeuda(int idReserva)
        {
            var deuda = _context.Reservas.Where(s => s.IdReserva == idReserva).Select(s => s.Total).FirstOrDefault();
            ViewBag.IdReserva = idReserva;

            return Json(new { costo = deuda });
        }

        [HttpPost]
        public IActionResult ActualizarEstado(int? id, bool estado)
        {

            if (id.HasValue)
            {
                var abono = _context.Abonos.Find(id.Value);

                if (abono != null)
                {
                    // Asignar el estado del abono según el valor del parámetro "estado"
                    abono.Estado = estado;
                    _context.SaveChangesAsync();
                    return View(abono);
                }
            }

            return NotFound(); // Abono no encontrado o ID nulo
        }

        // GET: Abonos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var abono = await _context.Abonos
                .Include(a => a.IdReservaNavigation)
                .FirstOrDefaultAsync(m => m.IdAbono == id);
                abono.Porcentaje = Math.Round((double)abono.Porcentaje * 100, 2);
            
            if (abono == null)
            {
                return NotFound();
            }

            return View(abono);
        }

        // GET: Abonos/Create
        public IActionResult Create(int idReserva)
        {
            var currentDateTime = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.CurrentDateTime = currentDateTime;

            var abono = new Abono
            {
                // Asigna el ID de la reserva al nuevo abono
                IdReserva = idReserva,
                // Otros campos del abono que puedas inicializar aquí
            };

            return View(abono);
        }

        // POST: Abonos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAbono,IdReserva,FechaRegistro,ValorDeuda,SubtotalAbonado,TotalPendiente,Iva,TotalAbonado,Porcentaje,Estado")] Abono abono)
        {
            if (ModelState.IsValid)
            {
                abono.FechaRegistro = DateOnly.FromDateTime(DateTime.Now);
                _context.Add(abono);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdReserva"] = new SelectList(_context.Reservas, "IdReserva", "IdReserva", abono.IdReserva);
            return View(abono);
        }

        // GET: Abonos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var abono = await _context.Abonos.FindAsync(id);
            if (abono == null)
            {
                return NotFound();
            }
            ViewData["IdReserva"] = new SelectList(_context.Reservas, "IdReserva", "IdReserva", abono.IdReserva);
            return View(abono);
        }

        // POST: Abonos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAbono,IdReserva,FechaRegistro,ValorDeuda,SubtotalAbonado,TotalPendiente,Iva,TotalAbonado,Porcentaje,Estado")] Abono abono)
        {
            if (id != abono.IdAbono)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(abono);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AbonoExists(abono.IdAbono))
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
            ViewData["IdReserva"] = new SelectList(_context.Reservas, "IdReserva", "IdReserva", abono.IdReserva);
            return View(abono);
        }

        // GET: Abonos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var abono = await _context.Abonos
                .Include(a => a.IdReservaNavigation)
                .FirstOrDefaultAsync(m => m.IdAbono == id);
            if (abono == null)
            {
                return NotFound();
            }

            return View(abono);
        }

        // POST: Abonos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var abono = await _context.Abonos.FindAsync(id);
            if (abono != null)
            {
                _context.Abonos.Remove(abono);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AbonoExists(int id)
        {
            return _context.Abonos.Any(e => e.IdAbono == id);
        }
    }
}
