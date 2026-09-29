namespace SIGE.Entidades
{
    public class Escalamiento : EntidadBase
    {
        public int IdIncidencia { get; set; }
        public int NivelOrigen { get; set; }
        public int NivelDestino { get; set; }
        public string Motivo { get; set; }
        public int IdUsuario { get; set; }
    }
}
