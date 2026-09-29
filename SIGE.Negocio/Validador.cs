using System;
using System.Linq;
using System.Text.RegularExpressions;
using SIGE.Entidades;

namespace SIGE.Negocio
{
    public static class Validador
    {
        private static readonly string[] Prioridades = { "Baja", "Media", "Alta", "Critica" };
        private static readonly string[] Estados = { "Abierta", "En proceso", "Resuelta", "Cerrada" };

        public static void ValidarIncidencia(Incidencia i)
        {
            if (string.IsNullOrWhiteSpace(i.Titulo))
                throw new ValidacionException("El título es obligatorio.");
            if (i.Titulo.Length > 120)
                throw new ValidacionException("El título no puede superar 120 caracteres.");
            if (string.IsNullOrWhiteSpace(i.Descripcion))
                throw new ValidacionException("La descripción es obligatoria.");
            if (i.IdCategoria <= 0)
                throw new ValidacionException("Seleccione una categoría.");
            if (!Prioridades.Contains(i.Prioridad))
                throw new ValidacionException("Seleccione una prioridad válida.");
        }

        public static void ValidarCambioEstado(string estado, string comentario)
        {
            if (!Estados.Contains(estado))
                throw new ValidacionException("Estado no válido.");
            if ((estado == "Resuelta" || estado == "Cerrada") && string.IsNullOrWhiteSpace(comentario))
                throw new ValidacionException("Debe escribir un comentario de solución para resolver o cerrar.");
        }

        public static void ValidarCredenciales(string correo, string clave)
        {
            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(clave))
                throw new ValidacionException("Escriba su correo y contraseña.");
            if (!Regex.IsMatch(correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ValidacionException("El correo no tiene un formato válido.");
        }
    }
}
