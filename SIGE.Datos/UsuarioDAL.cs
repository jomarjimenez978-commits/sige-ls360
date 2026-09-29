using System;
using System.Data;
using System.Data.SqlClient;
using SIGE.Entidades;

namespace SIGE.Datos
{
    public class UsuarioDAL
    {
        public Usuario Login(string correo, string claveHash)
        {
            try
            {
                using (var cn = Conexion.Instancia.Crear())
                using (var cmd = new SqlCommand("sp_Login", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Correo", correo);
                    cmd.Parameters.AddWithValue("@ClaveHash", claveHash);
                    cn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (!dr.Read()) return null;
                        return new Usuario
                        {
                            Id = Convert.ToInt32(dr["IdUsuario"]),
                            Nombre = dr["Nombre"].ToString(),
                            Correo = dr["Correo"].ToString(),
                            Rol = dr["Rol"].ToString(),
                            IdSucursal = Convert.ToInt32(dr["IdSucursal"])
                        };
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new AccesoDatosException("No se pudo verificar el usuario.", ex);
            }
        }
    }
}
