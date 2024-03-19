using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Jonia0._3.Models;
using System.Data;
using System.Data.SqlClient;
using NuGet.ContentModel;
using NuGet.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jonia0._3.Datos
{
    public class DBClientes
    {
        private static string CadenaSQL = "Server=MEDAPRCSGFSP663\\SQLEXPRESS; DataBase=Jonia_DB; Trusted_Connection=True; TrustServerCertificate=True";

        public static bool Registrar(Cliente cliente)


        {
            bool respuesta = false;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(CadenaSQL))

                {

                    string query = "insert into clientes (Nro_Documento,Tipo_Documento,Nombre,Apellido,Correo,Contrasena,Celular,Direccion,Fecha_Nacimiento,Restablecer,Confirmado,Token)";
                    query += "values(@nro_Documento,@tipo_Documento,@nombre,@apellido,@correo,@contrasena,@celular,@direccion,@fecha_Nacimiento,@restablecer,@confirmado,@token)";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@nro_Documento", cliente.NroDocumento);
                    cmd.Parameters.AddWithValue("@tipo_Documento", cliente.TipoDocumento);
                    cmd.Parameters.AddWithValue("@nombre", cliente.Nombre);
                    cmd.Parameters.AddWithValue("@apellido", cliente.Apellido);
                    cmd.Parameters.AddWithValue("@correo", cliente.Correo);
                    cmd.Parameters.AddWithValue("@contrasena", cliente.Contrasena);
                    cmd.Parameters.AddWithValue("@celular", cliente.Celular);
                    cmd.Parameters.AddWithValue("@direccion", cliente.Direccion);
                    cmd.Parameters.AddWithValue("@fecha_Nacimiento", cliente.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@restablecer", cliente.Restablecer);
                    cmd.Parameters.AddWithValue("@confirmado", cliente.Confirmado);
                    cmd.Parameters.AddWithValue("@token", cliente.Token);
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

        public static Cliente Validar(string correo, string contrasena)
        {
            Cliente cliente = null;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(CadenaSQL))
                {
                    string query = "select Nro_Documento,Nombre,Apellido,Celular,Direccion,Fecha_Nacimiento,Restablecer,Confirmado from clientes";
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
                            cliente = new Cliente()
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

            return cliente;
        }

        public static Cliente Obtener(string correo)
        {
            Cliente cliente = null;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(CadenaSQL))
                {
                    string query = "select Nro_Documento,Nombre,Contrasena,Apellido,Correo,Celular,Direccion,Fecha_Nacimiento,Restablecer,Confirmado,Token from clientes";
                    query += " where Correo=@correo";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@correo", correo);

                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            cliente = new Cliente()
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

            return cliente;
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

                    string query = @"update clientes set Restablecer=@restablecer, Contrasena=@contrasena where Token=@token and Correo=@correo";


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
                    string query = @"update clientes set Confirmado=1 where Token=@token";


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
