-- SIGE-LS360 | Script 3: trigger que fija la fecha de cierre automáticamente
USE SigeLS360;
GO
CREATE OR ALTER TRIGGER trg_Incidencias_Cierre ON Incidencias
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF UPDATE(Estado)
        UPDATE i SET FechaCierre = GETDATE()
        FROM Incidencias i INNER JOIN inserted n ON n.IdIncidencia = i.IdIncidencia
        WHERE n.Estado = 'Cerrada' AND i.FechaCierre IS NULL;
END
GO
