using System.Collections.Generic;
using System.Data;
using SIGE.Datos;
using SIGE.Entidades;
using SIGE.Negocio.Reglas;

namespace SIGE.Negocio
{
    public class IncidenciaBL
    {
        private readonly IncidenciaDAL _dal = new IncidenciaDAL();
        private readonly List<IReglaEscalamiento> _reglas = new List<IReglaEscalamiento>
        {
            new PorTiempoVencido(),
            new PorPrioridadCritica()
        };

        public int Registrar(Incidencia inc)
        {
            Validador.ValidarIncidencia(inc);
            inc.IdReporta = Sesion.UsuarioActual.Id;
            inc.IdSucursal = Sesion.UsuarioActual.IdSucursal;
            return _dal.Insertar(inc);
        }

        public List<Incidencia> Listar(string estado = null) => _dal.Listar(estado);

        public DataTable ListarCategorias() => _dal.ListarCategorias();

        public void CambiarEstado(int id, string estado, string comentario)
        {
            Validador.ValidarCambioEstado(estado, comentario);
            _dal.CambiarEstado(id, estado, comentario ?? "", Sesion.UsuarioActual.Id);
        }

        public void EscalarManual(int id, string motivo)
        {
            if (string.IsNullOrWhiteSpace(motivo))
                throw new ValidacionException("Escriba el motivo del escalamiento.");
            _dal.Escalar(id, motivo, Sesion.UsuarioActual.Id);
        }

        // Revisa todas las incidencias activas y escala las que cumplen alguna regla.
        public int EscalarAutomaticamente()
        {
            int escaladas = 0;
            foreach (var inc in _dal.Listar())
            {
                if (inc.Estado == "Cerrada" || inc.Estado == "Resuelta" || inc.NivelActual >= 3) continue;
                foreach (var regla in _reglas)
                {
                    if (regla.Aplica(inc))
                    {
                        _dal.Escalar(inc.Id, regla.Motivo, Sesion.UsuarioActual.Id);
                        escaladas++;
                        break;
                    }
                }
            }
            return escaladas;
        }
    }
}
