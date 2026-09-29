using System;
using System.Security.Cryptography;
using System.Text;
using SIGE.Datos;
using SIGE.Entidades;

namespace SIGE.Negocio
{
    public class UsuarioBL
    {
        private readonly UsuarioDAL _dal = new UsuarioDAL();

        public Usuario IniciarSesion(string correo, string clave)
        {
            Validador.ValidarCredenciales(correo, clave);
            var usuario = _dal.Login(correo.Trim(), Hash(clave));
            if (usuario == null)
                throw new ValidacionException("Correo o contraseña incorrectos.");
            Sesion.UsuarioActual = usuario;
            return usuario;
        }

        // SHA-256 en hexadecimal mayúscula (coincide con CONVERT(..., 2) de SQL Server).
        private static string Hash(string texto)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(texto));
                return BitConverter.ToString(bytes).Replace("-", "");
            }
        }
    }
}
