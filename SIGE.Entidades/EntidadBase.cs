using System;

namespace SIGE.Entidades
{
    // Herencia: todas las entidades comparten el identificador y la fecha de registro.
    public abstract class EntidadBase
    {
        public int Id { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
