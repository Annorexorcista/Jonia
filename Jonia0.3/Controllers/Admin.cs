using Jonia0._3.Datos;
using Jonia0._3.Models;
using Jonia0._3.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Jonia0._3.Controllers
{
	public class Admin : Controller
	{
		private readonly JoniaDbContext _Context;
		public Admin(JoniaDbContext context) 
		{
			_Context = context;
		}
		public ActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> Login(string correo, string contrasena)
		{
			Usuario usuario = DBCUsuario.Validar(correo, UtilidadServicios.ConvertirSHA256(contrasena));

			if (usuario != null)
			{
				if (usuario.Confirmado == false)
				{
					ViewBag.Mensaje = $"Por favor confirmar la direccion de correo electronico en {correo}";
				}
				else if (usuario.Restablecer == true)
				{
					ViewBag.Mensaje = $"Se solicito un restablecimiento de cuenta en {correo}";
				}
				else if (usuario.Estado == false)
				{
					ViewBag.Mensaje = "Su usuario está inhabilitado";
				}
				else
				{
					var claims = new List<Claim>();
					var claimsNombreRol = new List<Claim>();

					var permisosAsociados = _Context.RolPermisos.Where(s => s.IdRol == usuario.IdRol).Select(s => s.IdPermisoNavigation.Nombre).ToList();
					foreach(var permiso in permisosAsociados)
					{
						claims.Add(new Claim("Permiso", permiso));
					}

                    var nombresRol = _Context.Rols.Where(s => s.IdRol == usuario.IdRol).Select(s => s.Nombre).ToList();
                    foreach (var nombre in nombresRol)
                    {
                        claimsNombreRol.Add(new Claim("NombreRol", nombre));
                    }

                    

					var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

					await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

					return RedirectToAction("Index", "Dashboard");
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
		public ActionResult Registrar(Usuario usuario)

		{
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

		[HttpPost]
		public ActionResult Restablecer(string correo)
		{
			Usuario usuario = DBCUsuario.Obtener(correo);
			ViewBag.Correo = correo;
			if (usuario != null)
			{
				bool respuesta = DBCUsuario.RestablecerActualizar(1, usuario.Contrasena, usuario.Token, usuario.Correo);

				if (respuesta)
				{
					EnviarCorreoRestablecimiento(usuario.Correo, usuario.Token);
					ViewBag.Mensaje = $"Se ha enviado un correo a {usuario.Correo} para restablecer la contraseña.";
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

			string urlRestablecimiento = Url.Action("Actualizar", "Admin", new { token }, Request.Scheme);

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

			bool respuesta = DBCUsuario.RestablecerActualizar(0, UtilidadServicios.ConvertirSHA256(clave), token, correo);

			if (respuesta)
				ViewBag.Restablecido = true;
			else
				ViewBag.Mensaje = "No se pudo actualizar";

			return View();
		}

		public async Task<IActionResult> Salir()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login");
		} 
	}
}
