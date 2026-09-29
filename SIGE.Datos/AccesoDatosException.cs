using System;

namespace SIGE.Datos
{
    public class AccesoDatosException : Exception
    {
        public AccesoDatosException(string mensaje, Exception interna) : base(mensaje, interna) { }
    }
}
