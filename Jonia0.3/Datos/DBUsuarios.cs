using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using Jonia0._3.Models;
using System.Data;
using System.Data.SqlClient;

namespace Jonia0._3.Datos
{
    public class DBCUsuario
    {
        private static string CadenaSQL = "Server=DESKTOP-T60HGRI; DataBase=Jonia_DB; Trusted_Connection=True; TrustServerCertificate=True";

		public static bool Registrar(Usuario usuario)


		{
			bool respuesta = false;
			try
			{
				using (SqlConnection oconexion = new SqlConnection(CadenaSQL))

				{

					string query = "insert into usuarios (Nro_Documento,Tipo_Documento,Nombre,Apellido,Correo,Contrasena,Celular,Fecha_Nacimiento,Restablecer,Confirmado,Token)";
					query += "values(@nro_Documento,@tipo_Documento,@nombre,@apellido,@correo,@contrasena,@celular,@fecha_Nacimiento,@restablecer,@confirmado,@token)";

					SqlCommand cmd = new SqlCommand(query, oconexion);
					cmd.Parameters.AddWithValue("@nro_Documento", usuario.NroDocumento);
					cmd.Parameters.AddWithValue("@tipo_Documento", usuario.TipoDocumento);
					cmd.Parameters.AddWithValue("@nombre", usuario.Nombre);
					cmd.Parameters.AddWithValue("@apellido", usuario.Apellido);
					cmd.Parameters.AddWithValue("@correo", usuario.Correo);
					cmd.Parameters.AddWithValue("@contrasena", usuario.Contrasena);
					cmd.Parameters.AddWithValue("@celular", usuario.Celular);
					cmd.Parameters.AddWithValue("@fecha_Nacimiento", usuario.FechaNacimiento);
					cmd.Parameters.AddWithValue("@restablecer", usuario.Restablecer);
					cmd.Parameters.AddWithValue("@confirmado", usuario.Confirmado);
					cmd.Parameters.AddWithValue("@token", usuario.Token);
					cmd.CommandType = CommandType.Text;

					oconexion.Open();

					int filasAfectadas = cmd.ExecuteNonQuery();
					if (filasAfectadas > 0) respuesta = true;
				}

				return respuesta;

			}
			catch (Exception ex)
			{

				throw ex;
			}
		}

		public static Usuario Validar(string correo, string contrasena)
		{
			Usuario usuario = null;
			try
			{
				using (SqlConnection oconexion = new SqlConnection(CadenaSQL))
				{
					string query = "select Nro_Documento,Nombre,Apellido,Celular,Fecha_Nacimiento,Restablecer,Confirmado from usuarios";
					query += " where Correo=@correo and Contrasena=@contrasena";

					SqlCommand cmd = new SqlCommand(query, oconexion);
					cmd.Parameters.AddWithValue("@correo", correo);
					cmd.Parameters.AddWithValue("@contrasena", contrasena);

					cmd.CommandType = CommandType.Text;

					oconexion.Open();

					using (SqlDataReader dr = cmd.ExecuteReader())
					{
						if (dr.Read())
						{
							usuario = new Usuario()
							{
								Nombre = dr["Nombre"].ToString(),
								Restablecer = (bool)dr["Restablecer"],
								Confirmado = (bool)dr["Confirmado"]
							};
						}
					}
				}

			}
			catch (Exception ex)
			{

				throw ex;
			}

			return usuario;
		}

		public static Usuario Obtener(string correo)
		{
			Usuario usuario = null;
			try
			{
				using (SqlConnection oconexion = new SqlConnection(CadenaSQL))
				{
					string query = "select Nro_Documento,Nombre,Contrasena,Apellido,Correo,Celular,Fecha_Nacimiento,Restablecer,Confirmado,Token from usuarios";
					query += " where Correo=@correo";

					SqlCommand cmd = new SqlCommand(query, oconexion);
					cmd.Parameters.AddWithValue("@correo", correo);

					cmd.CommandType = CommandType.Text;

					oconexion.Open();

					using (SqlDataReader dr = cmd.ExecuteReader())
					{
						if (dr.Read())
						{
							usuario = new Usuario()
							{
								Nombre = dr["Nombre"].ToString(),
								Contrasena = dr["Contrasena"].ToString(),
								Restablecer = (bool)dr["Restablecer"],
								Confirmado = (bool)dr["Confirmado"],
								Token = dr["Token"].ToString(),
								Correo = dr["Correo"].ToString()
							};
						}
					}
				}

			}
			catch (Exception ex)
			{

				throw ex;
			}

			return usuario;
		}

		public static bool RestablecerActualizar(int restablecer, string contrasena, string token, string correo)


		{
			bool respuesta = false;
			try
			{
				using (SqlConnection oconexion = new SqlConnection(CadenaSQL))
				{
					Console.WriteLine($"Correo: {correo}");
					Console.WriteLine($"Contraseña: {contrasena}");
					Console.WriteLine($"Token: {token}");

					string query = @"update usuarios set Restablecer=@restablecer, Contrasena=@contrasena where Token=@token and Correo=@correo";


					SqlCommand cmd = new SqlCommand(query, oconexion);
					cmd.Parameters.AddWithValue("@restablecer", restablecer);
					cmd.Parameters.AddWithValue("@contrasena", contrasena);
					cmd.Parameters.AddWithValue("@token", token);
					cmd.Parameters.AddWithValue("@correo", correo);
					cmd.CommandType = CommandType.Text;

					oconexion.Open();

					int filasAfectadas = cmd.ExecuteNonQuery();
					if (filasAfectadas > 0) respuesta = true;
				}

				return respuesta;

			}
			catch (Exception ex)
			{

				throw ex;
			}
		}

		public static bool Confirmar(string token)


		{
			bool respuesta = false;
			try
			{
				using (SqlConnection oconexion = new SqlConnection(CadenaSQL))
				{
					string query = @"update usuarios set Confirmado=1 where Token=@token";


					SqlCommand cmd = new SqlCommand(query, oconexion);
					cmd.Parameters.AddWithValue("@token", token);
					cmd.CommandType = CommandType.Text;

					oconexion.Open();

					int filasAfectadas = cmd.ExecuteNonQuery();
					if (filasAfectadas > 0) respuesta = true;
				}

				return respuesta;

			}
			catch (Exception ex)
			{

				throw ex;
			}
		}

	}
}
