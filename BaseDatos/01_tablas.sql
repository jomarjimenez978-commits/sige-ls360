-- SIGE-LS360 | Script 1: creación de la base de datos y tablas
IF DB_ID('SigeLS360') IS NULL CREATE DATABASE SigeLS360;
GO
USE SigeLS360;
GO
CREATE TABLE Roles (
    IdRol INT IDENTITY PRIMARY KEY,
    Nombre NVARCHAR(30) NOT NULL UNIQUE
);
CREATE TABLE Sucursales (
    IdSucursal INT IDENTITY PRIMARY KEY,
    Nombre NVARCHAR(80) NOT NULL,
    Ciudad NVARCHAR(60) NOT NULL
);
CREATE TABLE Categorias (
    IdCategoria INT IDENTITY PRIMARY KEY,
    Nombre NVARCHAR(60) NOT NULL UNIQUE,
    TiempoLimiteHoras INT NOT NULL CHECK (TiempoLimiteHoras > 0)
);
CREATE TABLE Usuarios (
    IdUsuario INT IDENTITY PRIMARY KEY,
    Nombre NVARCHAR(80) NOT NULL,
    Correo NVARCHAR(100) NOT NULL UNIQUE,
    ClaveHash VARCHAR(64) NOT NULL,
    IdRol INT NOT NULL REFERENCES Roles(IdRol),
    IdSucursal INT NOT NULL REFERENCES Sucursales(IdSucursal),
    Activo BIT NOT NULL DEFAULT 1
);
CREATE TABLE Incidencias (
    IdIncidencia INT IDENTITY PRIMARY KEY,
    Titulo NVARCHAR(120) NOT NULL,
    Descripcion NVARCHAR(MAX) NOT NULL,
    IdCategoria INT NOT NULL REFERENCES Categorias(IdCategoria),
    Prioridad NVARCHAR(10) NOT NULL CHECK (Prioridad IN ('Baja','Media','Alta','Critica')),
    Estado NVARCHAR(15) NOT NULL DEFAULT 'Abierta'
        CHECK (Estado IN ('Abierta','En proceso','Resuelta','Cerrada')),
    NivelActual INT NOT NULL DEFAULT 1 CHECK (NivelActual BETWEEN 1 AND 3),
    IdReporta INT NOT NULL REFERENCES Usuarios(IdUsuario),
    IdAsignado INT NULL REFERENCES Usuarios(IdUsuario),
    IdSucursal INT NOT NULL REFERENCES Sucursales(IdSucursal),
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    FechaCierre DATETIME NULL
);
CREATE TABLE Seguimientos (
    IdSeguimiento INT IDENTITY PRIMARY KEY,
    IdIncidencia INT NOT NULL REFERENCES Incidencias(IdIncidencia),
    IdUsuario INT NOT NULL REFERENCES Usuarios(IdUsuario),
    Comentario NVARCHAR(500) NOT NULL,
    EstadoNuevo NVARCHAR(15) NOT NULL,
    Fecha DATETIME NOT NULL DEFAULT GETDATE()
);
CREATE TABLE Escalamientos (
    IdEscalamiento INT IDENTITY PRIMARY KEY,
    IdIncidencia INT NOT NULL REFERENCES Incidencias(IdIncidencia),
    NivelOrigen INT NOT NULL,
    NivelDestino INT NOT NULL,
    Motivo NVARCHAR(200) NOT NULL,
    IdUsuario INT NOT NULL REFERENCES Usuarios(IdUsuario),
    Fecha DATETIME NOT NULL DEFAULT GETDATE()
);
CREATE INDEX IX_Incidencias_Estado ON Incidencias(Estado, FechaCreacion);
CREATE INDEX IX_Incidencias_Sucursal ON Incidencias(IdSucursal);
CREATE INDEX IX_Seguimientos_Incidencia ON Seguimientos(IdIncidencia);
GO
