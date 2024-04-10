using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Jonia0._3.Models;
using Newtonsoft.Json;

namespace Jonia0._3.Controllers
{
    public class PaquetesController : Controller
    {
        private readonly JoniaDbContext _context;

        public PaquetesController(JoniaDbContext context)
        {
            _context = context;
        }

        // GET: Paquetes
        [HttpGet]
        public async Task<IActionResult> Index(string search)
        {
            IQueryable<Paquete> paqueteQuery = _context.Paquetes;

            if (!String.IsNullOrEmpty(search))
            {
                paqueteQuery = paqueteQuery.Where(a => a.Nombre.ToString().Contains(search));
            }

            var paquete = await paqueteQuery.ToListAsync();
            var joniaDbContext = _context.Paquetes.Include(p => p.IdHabitacionNavigation);

            await joniaDbContext.ToListAsync();

            return View(paquete);
        }

        [HttpPost]
        public IActionResult ActualizarEstado(int? id, bool estado)
        {

            if (id.HasValue)
            {
                var rol = _context.Paquetes.Find(id.Value);

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

        public IActionResult obtenerCostoHabitacion(int id)
        {
            var costoHab = _context.Habitaciones.Where(s => s.IdHabitacion== id).Select(s => s.Precio).FirstOrDefault(); 

            return Json(new {costo = costoHab});
        }
        public IActionResult obtenerCostoServicio(int id)
        {
            var costoser = _context.Servicios.Where(s => s.IdServicio == id).Select(s => s.Precio).FirstOrDefault();

            return Json(new { costo = costoser });
        }

        // GET: Paquetes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicios = _context.PaquetesServicios.Include(s => s.IdServicioNavigation).Where(s => s.IdPaquete== id).ToList();

            ViewBag.Servicios = servicios;

            var paquete = await _context.Paquetes
                .Include(p => p.IdHabitacionNavigation)
                .FirstOrDefaultAsync(m => m.IdPaquete == id);
            if (paquete == null)
            {
                return NotFound();
            }

            return View(paquete);
        }

        // GET: Paquetes/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Tipo = _context.Habitaciones;
            ViewBag.Serv = _context.Servicios.ToList();
            return View(new PaqueteViewModel()); // Aquí pasamos un objeto PaqueteViewModel vacío
        }

        // POST: Paquetes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(Paquete paquete, string serviciosSeleccionados)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paquete);
                _context.SaveChanges();

                var listaServicios = JsonConvert.DeserializeObject<List<Servicio>>(serviciosSeleccionados.ToString());

                foreach (var s in listaServicios)
                {
                    var paqueteservicio = new PaquetesServicio()
                    {
                        IdPaquete = paquete.IdPaquete,
                        IdServicio = s.IdServicio,
                        Precio = s.Precio
                    };
                    _context.PaquetesServicios.Add(paqueteservicio);
                    _context.SaveChanges();
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(paquete);
        }

        // GET: Paquetes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paquete = await _context.Paquetes.FindAsync(id);
            if (paquete == null)
            {
                return NotFound();
            }
            ViewBag.Tipo = _context.Habitaciones;
            return View(paquete);
        }

        // POST: Paquetes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPaquete,Nombre,Descripcion,Precio,IdHabitacion,Estado")] Paquete paquete)
        {
            if (id != paquete.IdPaquete)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paquete);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaqueteExists(paquete.IdPaquete))
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
            ViewBag.Tipo = _context.Habitaciones;
            return View(paquete);
        }

        // GET: Paquetes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paquete = await _context.Paquetes
                .Include(p => p.IdHabitacionNavigation)
                .FirstOrDefaultAsync(m => m.IdPaquete == id);
            if (paquete == null)
            {
                return NotFound();
            }

            return View(paquete);
        }

        // POST: Paquetes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paquete = await _context.Paquetes.FindAsync(id);
            if (paquete != null)
            {
                _context.Paquetes.Remove(paquete);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaqueteExists(int id)
        {
            return _context.Paquetes.Any(e => e.IdPaquete == id);
        }
    }
}
