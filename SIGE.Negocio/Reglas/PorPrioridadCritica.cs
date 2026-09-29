using SIGE.Entidades;

namespace SIGE.Negocio.Reglas
{
    public class PorPrioridadCritica : IReglaEscalamiento
    {
        public string Motivo => "Incidencia crítica sin atender";
        public bool Aplica(Incidencia i) =>
            i.Prioridad == "Critica" && i.Estado == "Abierta" && i.HorasTranscurridas() > 1;
    }
}
