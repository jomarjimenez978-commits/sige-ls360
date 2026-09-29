using SIGE.Entidades;

namespace SIGE.Negocio.Reglas
{
    public class PorTiempoVencido : IReglaEscalamiento
    {
        public string Motivo => "Tiempo límite de atención vencido";
        public bool Aplica(Incidencia i) => i.EstaVencida();
    }
}
