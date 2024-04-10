using System;
using System.Collections.Generic;
using System.Web;
using Jonia0._3.Models;
using Jonia0._3.Datos;
using Jonia0._3.Servicios;
using Microsoft.AspNetCore.Mvc;
using Jonia0._3.Controllers;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Jonia0._3.Controllers
{
    public class Inicio : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult ErrorPage() { return View(); }

        [HttpPost]
        public ActionResult Login(string correo, string contrasena)
        {
            Cliente cliente = DBClientes.Validar(correo, UtilidadServicios.ConvertirSHA256(contrasena));

            if (cliente != null)
            {
                if (cliente.Confirmado == false)
                {
                    ViewBag.Mensaje = $"Por favor confirmar la direccion de correo electronico en {correo}";
                }
                else if (cliente.Restablecer == true)
                {
                    ViewBag.Mensaje = $"Se solicito un restablecimiento de cuenta en {correo}";
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            else
            {
                ViewBag.Mensaje = "No se encontraron concidencias";
            }


            return View();
        }

        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(Cliente cliente)

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
                TempData["Creado"] = true;
                TempData["Mensaje"] = $"Su cuenta ha sido creada correctamente. Confirmar correo en {cliente.Correo}";
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
        public ActionResult Confirmar(string token)
        {
            ViewBag.Respuesta = DBClientes.Confirmar(token);
            return View();
        }

        public ActionResult Restablecer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Restablecer(string correo)
        {
            Cliente cliente = DBClientes.Obtener(correo);
            ViewBag.Correo = correo;
            if (cliente != null)
            {
                bool respuesta = DBClientes.RestablecerActualizar(1, cliente.Contrasena, cliente.Token, cliente.Correo);

                if (respuesta)
                {
                    EnviarCorreoRestablecimiento(cliente.Correo, cliente.Token);
                    ViewBag.Mensaje = $"Se ha enviado un correo a {cliente.Correo} para restablecer la contraseña.";
                    return View();
                }
                else
                {
                    ViewBag.Mensaje = "No se pudo restablecer la cuenta";
                }

            }
            else
            {
                ViewBag.Mensaje = "No se encontraron coincidencias";
            }

            return View();
        }

        private void EnviarCorreoRestablecimiento(string correo, string token)
        {
            var fromAddress = new MailAddress("pedazoehtiesto@gmail.com");
            var toAddress = new MailAddress(correo);
            const string fromPassword = "gmnupflehdxrqqmt";
            const string subject = "Restablecimiento de contraseña";

            string urlRestablecimiento = Url.Action("Actualizar", "Inicio", new { token }, Request.Scheme);

            string body = string.Format(@"
			<!DOCTYPE html>
			<html lang='es'>
			<body>
			    <div style='width:600px;padding:20px;border:1px solid #DBDBDB;border-radius:12px;font-family:Sans-serif'>
			        <h1 style='color:#C76F61'>Restablecer contraseña</h1>
			        <p style='margin-bottom:25px'>Estimado/a&nbsp;<b>{0}</b>:</p>
			        <p style='margin-bottom:25px'>Se solicitó un restablecimiento de contraseña para tu cuenta, haz clic en el botón que aparece a continuación para cambiar tu contraseña.</p>
			        <a style='padding:12px;border-radius:12px;background-color:#6181C7;color:#fff;text-decoration:none' href='{1}' target='_blank'>Cambiar contraseña</a>
			        <p style='margin-top:25px'>Gracias.</p>
			    </div>
			</body>
			</html>", correo, urlRestablecimiento);

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

        public ActionResult Actualizar(string token)
        {
            ViewBag.Token = token;
            return View();
        }

        [HttpPost]
        public ActionResult Actualizar(string token, string clave, string confirmarClave, string correo)
        {
            ViewBag.Token = token;
            if (clave != confirmarClave)
            {
                ViewBag.Mensaje = "Las contraseñas no coinciden";
                return View();
            }

            bool respuesta = DBClientes.RestablecerActualizar(0, UtilidadServicios.ConvertirSHA256(clave), token, correo);

            if (respuesta)
                ViewBag.Restablecido = true;
            else
                ViewBag.Mensaje = "No se pudo actualizar";

            return View();
        }
    }
}
