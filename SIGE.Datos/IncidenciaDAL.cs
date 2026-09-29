using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SIGE.Entidades;

namespace SIGE.Datos
{
    // Patrón DAO: toda la comunicación con SQL Server pasa por esta clase.
    public class IncidenciaDAL
    {
        public int Insertar(Incidencia i)
        {
            try
            {
                using (var cn = Conexion.Instancia.Crear())
                using (var cmd = new SqlCommand("sp_RegistrarIncidencia", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Titulo", i.Titulo);
                    cmd.Parameters.AddWithValue("@Descripcion", i.Descripcion);
                    cmd.Parameters.AddWithValue("@IdCategoria", i.IdCategoria);
                    cmd.Parameters.AddWithValue("@Prioridad", i.Prioridad);
                    cmd.Parameters.AddWithValue("@IdReporta", i.IdReporta);
                    cmd.Parameters.AddWithValue("@IdSucursal", i.IdSucursal);
                    cn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (SqlException ex)
            {
                throw new AccesoDatosException("No se pudo registrar la incidencia.", ex);
            }
        }

        public List<Incidencia> Listar(string estado = null)
        {
            var lista = new List<Incidencia>();
            try
            {
                using (var cn = Conexion.Instancia.Crear())
                using (var cmd = new SqlCommand("sp_ListarIncidencias", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Estado", (object)estado ?? DBNull.Value);
                    cn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Incidencia
                            {
                                Id = Convert.ToInt32(dr["IdIncidencia"]),
                                Titulo = dr["Titulo"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                                Categoria = dr["Categoria"].ToString(),
                                TiempoLimiteHoras = Convert.ToInt32(dr["TiempoLimiteHoras"]),
                                Prioridad = dr["Prioridad"].ToString(),
                                Estado = dr["Estado"].ToString(),
                                NivelActual = Convert.ToInt32(dr["NivelActual"]),
                                IdReporta = Convert.ToInt32(dr["IdReporta"]),
                                Sucursal = dr["Sucursal"].ToString(),
                                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                                FechaCierre = dr["FechaCierre"] == DBNull.Value
                                    ? (DateTime?)null : Convert.ToDateTime(dr["FechaCierre"])
                            });
                        }
                    }
                }
                return lista;
            }
            catch (SqlException ex)
            {
                throw new AccesoDatosException("No se pudo obtener la lista de incidencias.", ex);
            }
        }

        public DataTable ListarCategorias()
        {
            try
            {
                using (var cn = Conexion.Instancia.Crear())
                using (var cmd = new SqlCommand("sp_ListarCategorias", cn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var tabla = new DataTable();
                    da.Fill(tabla);
                    return tabla;
                }
            }
            catch (SqlException ex)
            {
                throw new AccesoDatosException("No se pudieron cargar las categorías.", ex);
            }
        }

        public void CambiarEstado(int idIncidencia, string estadoNuevo, string comentario, int idUsuario)
        {
            EjecutarSinResultado("sp_CambiarEstado", cmd =>
            {
                cmd.Parameters.AddWithValue("@IdIncidencia", idIncidencia);
                cmd.Parameters.AddWithValue("@EstadoNuevo", estadoNuevo);
                cmd.Parameters.AddWithValue("@Comentario", comentario);
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
            }, "No se pudo cambiar el estado.");
        }

        public void Escalar(int idIncidencia, string motivo, int idUsuario)
        {
            EjecutarSinResultado("sp_EscalarIncidencia", cmd =>
            {
                cmd.Parameters.AddWithValue("@IdIncidencia", idIncidencia);
                cmd.Parameters.AddWithValue("@Motivo", motivo);
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
            }, "No se pudo escalar la incidencia.");
        }

        private void EjecutarSinResultado(string sp, Action<SqlCommand> parametros, string mensaje)
        {
            try
            {
                using (var cn = Conexion.Instancia.Crear())
                using (var cmd = new SqlCommand(sp, cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    parametros(cmd);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new AccesoDatosException(mensaje + " " + ex.Message, ex);
            }
        }
    }
}
