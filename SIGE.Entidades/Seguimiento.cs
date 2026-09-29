namespace SIGE.Entidades
{
    public class Seguimiento : EntidadBase
    {
        public int IdIncidencia { get; set; }
        public int IdUsuario { get; set; }
        public string Comentario { get; set; }
        public string EstadoNuevo { get; set; }
    }
}
