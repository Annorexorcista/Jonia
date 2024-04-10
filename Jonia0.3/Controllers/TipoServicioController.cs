using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Jonia0._3.Models;
using Microsoft.AspNetCore.Authorization;

namespace Jonia0._3.Controllers
{
    [Authorize(Policy = "TipoServicio")]
    public class TipoServicioController : Controller
    {
        private readonly JoniaDbContext _context;

        public TipoServicioController(JoniaDbContext context)
        {
            _context = context;
        }

        // GET: TipoServicio
        [HttpGet]
        public async Task<IActionResult> Index(string search)
        {
            IQueryable<TipoServicio> tipoQuery = _context.TipoServicios;

            if (!String.IsNullOrEmpty(search))
            {
                tipoQuery = tipoQuery.Where(a => a.Nombre.ToString().Contains(search));
            }

            var tipo = await tipoQuery.ToListAsync();

            return View(tipo);
        }

        [HttpPost]
        public IActionResult ActualizarEstado(int? id, bool estado)
        {

            if (id.HasValue)
            {
                var tipo = _context.TipoServicios.Find(id.Value);

                if (tipo != null)
                {
                    // Asignar el estado del abono según el valor del parámetro "estado"
                    tipo.Estado = estado;
                    _context.SaveChangesAsync();
                    return View(tipo);
                }
            }

            return NotFound(); // Abono no encontrado o ID nulo
        }

        // GET: TipoServicio/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoServicio = await _context.TipoServicios
                .FirstOrDefaultAsync(m => m.IdTs == id);
            if (tipoServicio == null)
            {
                return NotFound();
            }

            return View(tipoServicio);
        }

        // GET: TipoServicio/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoServicio/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTs,Nombre")] TipoServicio tipoServicio)
        {
            if (ModelState.IsValid)
            {
                if(tipoServicio.Nombre == null)
                {
                    TempData["error"] = "Se deben llenar todos los campos.";
                    return RedirectToAction();
                }

                _context.Add(tipoServicio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoServicio);
        }

        // GET: TipoServicio/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoServicio = await _context.TipoServicios.FindAsync(id);
            if (tipoServicio == null)
            {
                return NotFound();
            }
            return View(tipoServicio);
        }

        // POST: TipoServicio/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTs,Nombre")] TipoServicio tipoServicio)
        {
            if (id != tipoServicio.IdTs)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoServicio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoServicioExists(tipoServicio.IdTs))
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
            return View(tipoServicio);
        }

        // GET: TipoServicio/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoServicio = await _context.TipoServicios
                .FirstOrDefaultAsync(m => m.IdTs == id);
            if (tipoServicio == null)
            {
                return NotFound();
            }

            return View(tipoServicio);
        }

        // POST: TipoServicio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoServicio = await _context.TipoServicios.FindAsync(id);
            if (tipoServicio != null)
            {
                _context.TipoServicios.Remove(tipoServicio);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoServicioExists(int id)
        {
            return _context.TipoServicios.Any(e => e.IdTs == id);
        }
    }
}
