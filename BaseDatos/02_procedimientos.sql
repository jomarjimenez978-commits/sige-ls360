-- SIGE-LS360 | Script 2: procedimientos almacenados
USE SigeLS360;
GO
CREATE OR ALTER PROCEDURE sp_Login @Correo NVARCHAR(100), @ClaveHash VARCHAR(64)
AS
BEGIN
    SELECT u.IdUsuario, u.Nombre, u.Correo, r.Nombre AS Rol, u.IdSucursal
    FROM Usuarios u INNER JOIN Roles r ON r.IdRol = u.IdRol
    WHERE u.Correo = @Correo AND u.ClaveHash = @ClaveHash AND u.Activo = 1;
END
GO
CREATE OR ALTER PROCEDURE sp_ListarCategorias
AS
BEGIN
    SELECT IdCategoria, Nombre, TiempoLimiteHoras FROM Categorias ORDER BY Nombre;
END
GO
CREATE OR ALTER PROCEDURE sp_RegistrarIncidencia
    @Titulo NVARCHAR(120), @Descripcion NVARCHAR(MAX), @IdCategoria INT,
    @Prioridad NVARCHAR(10), @IdReporta INT, @IdSucursal INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Incidencias (Titulo, Descripcion, IdCategoria, Prioridad, IdReporta, IdSucursal)
    VALUES (@Titulo, @Descripcion, @IdCategoria, @Prioridad, @IdReporta, @IdSucursal);
    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
GO
CREATE OR ALTER PROCEDURE sp_ListarIncidencias @Estado NVARCHAR(15) = NULL
AS
BEGIN
    SELECT i.IdIncidencia, i.Titulo, i.Descripcion, i.IdCategoria, c.Nombre AS Categoria,
           c.TiempoLimiteHoras, i.Prioridad, i.Estado, i.NivelActual, i.IdReporta,
           s.Nombre AS Sucursal, i.FechaCreacion, i.FechaCierre
    FROM Incidencias i
    INNER JOIN Categorias c ON c.IdCategoria = i.IdCategoria
    INNER JOIN Sucursales s ON s.IdSucursal = i.IdSucursal
    WHERE (@Estado IS NULL OR i.Estado = @Estado)
    ORDER BY i.FechaCreacion DESC;
END
GO
CREATE OR ALTER PROCEDURE sp_CambiarEstado
    @IdIncidencia INT, @EstadoNuevo NVARCHAR(15), @Comentario NVARCHAR(500), @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE Incidencias SET Estado = @EstadoNuevo WHERE IdIncidencia = @IdIncidencia;
        INSERT INTO Seguimientos (IdIncidencia, IdUsuario, Comentario, EstadoNuevo)
        VALUES (@IdIncidencia, @IdUsuario, @Comentario, @EstadoNuevo);
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
CREATE OR ALTER PROCEDURE sp_EscalarIncidencia
    @IdIncidencia INT, @Motivo NVARCHAR(200), @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Nivel INT;
    SELECT @Nivel = NivelActual FROM Incidencias WHERE IdIncidencia = @IdIncidencia;
    IF @Nivel IS NULL THROW 50001, 'La incidencia no existe.', 1;
    IF @Nivel >= 3 THROW 50002, 'La incidencia ya esta en el nivel maximo.', 1;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE Incidencias SET NivelActual = @Nivel + 1 WHERE IdIncidencia = @IdIncidencia;
        INSERT INTO Escalamientos (IdIncidencia, NivelOrigen, NivelDestino, Motivo, IdUsuario)
        VALUES (@IdIncidencia, @Nivel, @Nivel + 1, @Motivo, @IdUsuario);
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
