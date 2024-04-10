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
    public class HabitacionesController : Controller
    {
        private readonly JoniaDbContext _context;

        public HabitacionesController(JoniaDbContext context)
        {
            _context = context;
        }

        // GET: Habitaciones
        [HttpGet]
        public async Task<IActionResult> Index(string search)
        {
            IQueryable<Habitacione> habitacionQuery = _context.Habitaciones;

            if (!String.IsNullOrEmpty(search))
            {
                habitacionQuery = habitacionQuery.Where(a => a.Nombre.ToString().Contains(search));
            }

            var habitacion = await habitacionQuery.ToListAsync();

            var joniaDbContext = _context.Habitaciones.Include(h => h.IdTipoNavigation);

            await joniaDbContext.ToListAsync();

            return View(habitacion);
        }

        [HttpPost]
        public IActionResult ActualizarEstado(int? id, bool estado)
        {

            if (id.HasValue)
            {
                var rol = _context.Habitaciones.Find(id.Value);

                if (rol != null)
                {
                    // Asignar el estado del abono según el valor del parámetro "estado"
                    rol.Estado = estado;
                    _context.SaveChangesAsync();
                    return View(rol);
                }
            }

            return NotFound(); // Abono no encontrado o ID nulo
        }

        // GET: Habitaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habitacione = await _context.Habitaciones
                .Include(h => h.IdTipoNavigation)
                .FirstOrDefaultAsync(m => m.IdHabitacion == id);
            if (habitacione == null)
            {
                return NotFound();
            }

            return View(habitacione);
        }

        // GET: Habitaciones/Create
        public IActionResult Create()
        {
            ViewBag.Tipo = _context.TipoHabitacions;
            return View();
        }

        // POST: Habitaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdHabitacion,Nombre,Descripcion,IdTipo,Precio,Estado")] Habitacione habitacione)
        {
            if (ModelState.IsValid)
            {
                _context.Add(habitacione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdTipo"] = new SelectList(_context.TipoHabitacions, "IdTipo", "IdTipo", habitacione.IdTipo);
            return View(habitacione);
        }

        // GET: Habitaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habitacione = await _context.Habitaciones.FindAsync(id);
            if (habitacione == null)
            {
                return NotFound();
            }
            ViewBag.Tipo = _context.TipoHabitacions;
            return View(habitacione);
        }

        // POST: Habitaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdHabitacion,Nombre,Descripcion,IdTipo,Precio,Estado")] Habitacione habitacione)
        {
            if (id != habitacione.IdHabitacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(habitacione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HabitacioneExists(habitacione.IdHabitacion))
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
            ViewData["IdTipo"] = new SelectList(_context.TipoHabitacions, "IdTipo", "IdTipo", habitacione.IdTipo);
            return View(habitacione);
        }

        // GET: Habitaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habitacione = await _context.Habitaciones
                .Include(h => h.IdTipoNavigation)
                .FirstOrDefaultAsync(m => m.IdHabitacion == id);
            if (habitacione == null)
            {
                return NotFound();
            }
            ViewBag.Tipo = _context.TipoHabitacions;
            return View(habitacione);
        }

        // POST: Habitaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var habitacione = await _context.Habitaciones.FindAsync(id);
            if (habitacione != null)
            {
                _context.Habitaciones.Remove(habitacione);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HabitacioneExists(int id)
        {
            return _context.Habitaciones.Any(e => e.IdHabitacion == id);
        }
    }
}
