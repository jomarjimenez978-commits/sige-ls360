-- SIGE-LS360 | Script 4: datos de prueba
USE SigeLS360;
GO
INSERT INTO Roles (Nombre) VALUES ('Empleado'), ('Tecnico'), ('Supervisor'), ('Administrador');
INSERT INTO Sucursales (Nombre, Ciudad) VALUES
 ('La Sirena Santiago', 'Santiago'), ('La Sirena Santo Domingo', 'Santo Domingo');
INSERT INTO Categorias (Nombre, TiempoLimiteHoras) VALUES
 ('Caja registradora', 4), ('Red e internet', 8), ('Sistema interno', 12), ('Equipo de oficina', 24);
-- Contraseña de prueba de ambos usuarios: Admin123
INSERT INTO Usuarios (Nombre, Correo, ClaveHash, IdRol, IdSucursal) VALUES
 ('Administrador', 'admin@sige.local', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256','Admin123'), 2), 4, 1),
 ('Empleado Prueba', 'empleado@sige.local', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256','Admin123'), 2), 1, 1);
GO
