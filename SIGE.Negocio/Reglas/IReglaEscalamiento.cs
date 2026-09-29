using SIGE.Entidades;

namespace SIGE.Negocio.Reglas
{
    // Patrón Strategy: cada regla decide por su cuenta si una incidencia debe escalarse.
    public interface IReglaEscalamiento
    {
        string Motivo { get; }
        bool Aplica(Incidencia incidencia);
    }
}
