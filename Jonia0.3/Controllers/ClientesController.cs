using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Jonia0._3.Models;
using Jonia0._3.Datos;
using Jonia0._3.Servicios;
using System.Net.Mail;
using System.Net;

namespace Jonia0._3.Controllers
{
    public class ClientesController : Controller
    {
        private readonly JoniaDbContext _context;

        public ClientesController(JoniaDbContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            var joniaDbContext = _context.Clientes.Include(c => c.IdRolNavigation).Include(c => c.TipoDocumentoNavigation);
            return View(await joniaDbContext.ToListAsync());
        }

        [HttpPost]
        public IActionResult ActualizarEstado(int? id, bool estado)
        {

            if (id.HasValue)
            {
                var rol = _context.Clientes.Find(id.Value);

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

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.IdRolNavigation)
                .Include(c => c.TipoDocumentoNavigation)
                .FirstOrDefaultAsync(m => m.NroDocumento == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            ViewBag.Rol = _context.Rols;
            ViewBag.Tipo = _context.TipoDocumentos;
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(Cliente cliente)

        {
            if (cliente.Contrasena != cliente.Confirmarclave)
            {
                ViewBag.Nombre = cliente.Nombre;
                ViewBag.Correo = cliente.Correo;
                ViewBag.Mensaje = "Las contraseñas no coinciden";
                return View();
            }

            Cliente clienteExistente = DBClientes.Obtener(cliente.Correo);
            if (clienteExistente != null)
            {
                ViewBag.Mensaje = "El correo ya se encuentra registrado";
                return View();
            }

            cliente.Contrasena = UtilidadServicios.ConvertirSHA256(cliente.Contrasena);
            cliente.Token = UtilidadServicios.GenerarToken();
            cliente.Restablecer = false;
            cliente.Confirmado = false;
            bool respuesta = DBClientes.Registrar(cliente);

            if (respuesta)
            {
                EnviarCorreoConfirmacion(cliente.Correo, cliente.Nombre, cliente.Token);
                return View();
            }

            return View();
        }

        private void EnviarCorreoConfirmacion(string correo, string nombre, string token)
        {
            var fromAddress = new MailAddress("pedazoehtiesto@gmail.com", "Jonia");
            var toAddress = new MailAddress(correo);
            const string fromPassword = "gmnupflehdxrqqmt";
            const string subject = "Confirmación de registro";

            string urlConfirmacion = Url.Action("Confirmar", "Inicio", new { token }, Request.Scheme);

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

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            ViewBag.Rol = _context.Rols;
            ViewBag.Tipo = _context.TipoDocumentos;
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NroDocumento,TipoDocumento,Nombre,Apellido,Correo,Contrasena,Confirmarclave,Celular,Direccion,FechaNacimiento,IdRol,Estado,Confirmado,Restablecer,Token")] Cliente cliente)
        {
            if (id != cliente.NroDocumento)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.NroDocumento))
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
            ViewData["IdRol"] = new SelectList(_context.Rols, "IdRol", "IdRol", cliente.IdRol);
            ViewData["TipoDocumento"] = new SelectList(_context.TipoDocumentos, "IdTd", "IdTd", cliente.TipoDocumento);
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.IdRolNavigation)
                .Include(c => c.TipoDocumentoNavigation)
                .FirstOrDefaultAsync(m => m.NroDocumento == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.NroDocumento == id);
        }
    }
}
