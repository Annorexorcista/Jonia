using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Jonia0._3.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Jonia0._3.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly JoniaDbContext _context;

        public UsuariosController(JoniaDbContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var joniaDbContext = _context.Usuarios.Include(u => u.IdRolNavigation).Include(u => u.TipoDocumentoNavigation);
            return View(await joniaDbContext.ToListAsync());
        }

        [HttpPost]
        public IActionResult ActualizarEstado(int? id, bool estado)
        {

            if (id.HasValue)
            {
                var rol = _context.Usuarios.Find(id.Value);

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

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .Include(u => u.TipoDocumentoNavigation)
                .FirstOrDefaultAsync(m => m.NroDocumento == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        public IActionResult Create()
        {
            ViewBag.TipoDocumentos = _context.TipoDocumentos.ToList();
            ViewBag.Rol = _context.Rols.ToList();
            // Otra lógica de inicialización si es necesario
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NroDocumento,TipoDocumento,Nombre,Apellido,Correo,Contrasena,Confirmarclave,Celular,FechaNacimiento,IdRol,Estado,Confirmado,Restablecer,Token")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TipoDocumentos = _context.TipoDocumentos.ToList(); // Actualiza la lista de tipos de documentos en caso de error
            ViewBag.Rol = _context.Rols.ToList();
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewBag.TipoDocumentos = _context.TipoDocumentos.ToList();
            ViewBag.Rol = _context.Rols.ToList();
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NroDocumento,TipoDocumento,Nombre,Apellido,Correo,Contrasena,Confirmarclave,Celular,FechaNacimiento,IdRol,Estado,Confirmado,Restablecer,Token")] Usuario usuario)
        {
            if (id != usuario.NroDocumento)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.NroDocumento))
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

            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .Include(u => u.TipoDocumentoNavigation)
                .FirstOrDefaultAsync(m => m.NroDocumento == id);


            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.NroDocumento == id);
        }
    }
}