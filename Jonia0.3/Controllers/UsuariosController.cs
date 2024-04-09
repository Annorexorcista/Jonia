using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Jonia0._3.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Jonia0._3.Datos;
using Jonia0._3.Servicios;
using System.Net.Mail;
using System.Net;

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
            
            // Otra lógica de inicialización si es necesario
            return View();
        }

        [HttpPost]
        public ActionResult Create(Usuario usuario)

        {
            var roles = _context.Rols; 
            ViewBag.Roles = roles;

            if (usuario.Contrasena != usuario.Confirmarclave)
            {
                ViewBag.Nombre = usuario.Nombre;
                ViewBag.Correo = usuario.Correo;
                ViewBag.Mensaje = "Las contraseñas no coinciden";
                return View();
            }

            Usuario usuarioExistente = DBCUsuario.Obtener(usuario.Correo);
            if (usuarioExistente != null)
            {
                ViewBag.Mensaje = "El correo ya se encuentra registrado";
                return View();
            }

            usuario.Contrasena = UtilidadServicios.ConvertirSHA256(usuario.Contrasena);
            usuario.Token = UtilidadServicios.GenerarToken();
            usuario.Restablecer = false;
            usuario.Confirmado = false;
            bool respuesta = DBCUsuario.Registrar(usuario);

            if (respuesta)
            {
                EnviarCorreoConfirmacion(usuario.Correo, usuario.Nombre, usuario.Token);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
            
        }

        private void EnviarCorreoConfirmacion(string correo, string nombre, string token)
        {
            var fromAddress = new MailAddress("pedazoehtiesto@gmail.com", "Jonia");
            var toAddress = new MailAddress(correo);
            const string fromPassword = "gmnupflehdxrqqmt";
            const string subject = "Confirmación de registro";

            string urlConfirmacion = Url.Action("Confirmar", "Admin", new { token }, Request.Scheme);

            string body = string.Format(@"
			<!DOCTYPE html>
			<html lang='es'>
			<body>
			    <div style='width:600px;padding:20px;border:1px solid #13142a;border-radius:12px;font-family:Sans-serif'>
			        <h1 style='color:#161b33'>Confirmar correo electrónico</h1>
			        <p style='margin-bottom:25px'>Estimado/a&nbsp;<b>{0}</b>:</p>
			        <p style='margin-bottom:25px'>Gracias por abrir una cuenta con nosotros. Para utilizar su cuenta, primero deberá confirmar su correo electrónico haciendo clic en el boton a continuación.</p>
			        <a href='{1}' style='padding:12px;border-radius:12px;background-color:#6181C7;color:#161b33;text-decoration:none' target='_blank'>Confirme su correo electronico</a>
			        <p style='margin-top:25px'>Gracias.</p>
			    </div>
			</body>
			</html>", nombre, urlConfirmacion);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }
        public ActionResult Confirmar(string token)
        {
            ViewBag.Respuesta = DBCUsuario.Confirmar(token);
            return View();
        }

        public ActionResult Restablecer()
        {
            return View();
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