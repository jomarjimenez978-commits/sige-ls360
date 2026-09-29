namespace SIGE.Entidades
{
    public class Usuario : EntidadBase
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Rol { get; set; }
        public int IdSucursal { get; set; }

        public bool EsSupervisor => Rol == "Supervisor" || Rol == "Administrador";
    }
}
