using System;

namespace SIGE.Entidades
{
    public class Incidencia : EntidadBase
    {
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int IdCategoria { get; set; }
        public string Categoria { get; set; }
        public int TiempoLimiteHoras { get; set; }
        public string Prioridad { get; set; }
        public string Estado { get; set; } = "Abierta";
        public int NivelActual { get; set; } = 1;
        public int IdReporta { get; set; }
        public int IdSucursal { get; set; }
        public string Sucursal { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaCierre { get; set; }

        public double HorasTranscurridas() => (DateTime.Now - FechaCreacion).TotalHours;

        public bool EstaVencida() =>
            Estado != "Cerrada" && Estado != "Resuelta" && HorasTranscurridas() > TiempoLimiteHoras;
    }
}
