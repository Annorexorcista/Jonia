using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Jonia0._3.Models;
using Jonia0._3.Models.PermisosSeleccionados;
using Microsoft.AspNetCore.Authorization;

namespace Jonia0._3.Controllers
{
    [Authorize(Policy = "Roles")]
    public class RolController : Controller
    {
        private readonly JoniaDbContext _context;

        public RolController(JoniaDbContext context)
        {
            _context = context;
        }

        // GET: Rol
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rols.ToListAsync());
        }

        [HttpPost]
        public IActionResult ActualizarEstado(int? id, bool estado)
        {

            if (id.HasValue)
            {
                var rol = _context.Rols.Find(id.Value);

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

        // GET: Rol/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permisos = _context.RolPermisos.Include(s => s.IdPermisoNavigation).Where(s => s.IdRol == id).ToList();

            ViewBag.Permisos = permisos;

            var rol = await _context.Rols
                .FirstOrDefaultAsync(m => m.IdRol == id);
            if (rol == null)
            {
                return NotFound();
            }

            return View(rol);
        }

        // GET: Rol/Create
        public IActionResult Create()
        {
            // Obtén la lista de permisos desde la base de datos
            var permisos = _context.Permisos.ToList();

            // Pásala a la vista usando ViewBag
            ViewBag.Permisos = permisos;

            return View();
        }

        // POST: Rol/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rol rol, List<int> permisosSeleccionados)
        {
            if (ModelState.IsValid)
            {
                if(rol.Nombre == null)
                {
                    TempData["error"] = "Se deben llenar todos los campos.";
                    return RedirectToAction();
                }

                _context.Add(rol);
                await _context.SaveChangesAsync();

                // Después de guardar el rol, guarda los permisos seleccionados
                foreach (var permisoId in permisosSeleccionados)
                {
                    var rolPermiso = new RolPermiso { IdRol = rol.IdRol, IdPermiso = permisoId };
                    _context.RolPermisos.Add(rolPermiso);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(rol);
        }

        // GET: Rol/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permisos = _context.Permisos.ToList();

            // Pásala a la vista usando ViewBag
            List<PermisosSel> listaPermisos = new List<PermisosSel>();

            foreach(var permiso in permisos)
            {
                var asignado_ = _context.RolPermisos.Any(s => s.IdPermiso == permiso.IdPermiso && s.IdRol == id);

                PermisosSel opermisosel = new PermisosSel
                {
                    oPermiso = _context.Permisos.Where(s => s.IdPermiso == permiso.IdPermiso).FirstOrDefault(),
                    
                    asignado = asignado_
                };

                listaPermisos.Add(opermisosel);
            }
            ViewBag.Permisos = listaPermisos;

            var rol = await _context.Rols.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }
            return View(rol);
        }

        // POST: Rol/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Rol rol, List<int> permisosSeleccionados)
        {
            if (id != rol.IdRol)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var permisosOriginales = _context.RolPermisos
                    .Where(rp => rp.IdRol == id).ToList();

                _context.RolPermisos.RemoveRange(permisosOriginales);

                foreach (var permisoId in permisosSeleccionados)
                {
                    var rolPermiso = new RolPermiso { IdRol = rol.IdRol, IdPermiso = permisoId };
                    _context.RolPermisos.Add(rolPermiso);
                }

                try
                {
                    _context.Update(rol);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RolExists(rol.IdRol))
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
            return View(rol);
        }

        // GET: Rol/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rol = await _context.Rols
                .FirstOrDefaultAsync(m => m.IdRol == id);
            if (rol == null)
            {
                return NotFound();
            }

            return View(rol);
        }

        // POST: Rol/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var permisosOriginales = _context.RolPermisos
                   .Where(rp => rp.IdRol == id).ToList();

            _context.RolPermisos.RemoveRange(permisosOriginales);

            var rol = await _context.Rols.FindAsync(id);
            if (rol != null)
            {
                _context.Rols.Remove(rol);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RolExists(int id)
        {
            return _context.Rols.Any(e => e.IdRol == id);
        }
    }
}
