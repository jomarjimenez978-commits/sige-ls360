using System.Configuration;
using System.Data.SqlClient;

namespace SIGE.Datos
{
    // Patrón Singleton: una única instancia que administra la cadena de conexión.
    public sealed class Conexion
    {
        private static readonly Conexion _instancia = new Conexion();
        public static Conexion Instancia => _instancia;

        private readonly string _cadena =
            ConfigurationManager.ConnectionStrings["SigeLS360"].ConnectionString;

        private Conexion() { }

        public SqlConnection Crear() => new SqlConnection(_cadena);
    }
}
