/*
    Proyecto de Programacion - Taller de Reparaciones
    Script 1: creacion de la base de datos y las tablas

    Orden de ejecucion: 01 -> 02 -> 03
*/

IF DB_ID('TallerReparaciones') IS NULL
    CREATE DATABASE TallerReparaciones;
GO

USE TallerReparaciones;
GO

/* Si se vuelve a correr el script se borra todo para no duplicar.
   Las tablas se borran al reves por las llaves foraneas. */
IF OBJECT_ID('Asignaciones', 'U') IS NOT NULL DROP TABLE Asignaciones;
IF OBJECT_ID('DetallesReparacion', 'U') IS NOT NULL DROP TABLE DetallesReparacion;
IF OBJECT_ID('Reparaciones', 'U') IS NOT NULL DROP TABLE Reparaciones;
IF OBJECT_ID('Equipos', 'U') IS NOT NULL DROP TABLE Equipos;
IF OBJECT_ID('Tecnicos', 'U') IS NOT NULL DROP TABLE Tecnicos;
IF OBJECT_ID('Usuarios', 'U') IS NOT NULL DROP TABLE Usuarios;
IF OBJECT_ID('UsuariosSistema', 'U') IS NOT NULL DROP TABLE UsuariosSistema;
GO


-- Clientes que llevan los equipos al taller
CREATE TABLE Usuarios
(
    UsuarioID           INT IDENTITY(1,1) NOT NULL,
    Nombre              VARCHAR(100)      NOT NULL,
    CorreoElectronico   VARCHAR(100)      NOT NULL,
    Telefono            VARCHAR(20)       NULL,

    CONSTRAINT PK_Usuarios PRIMARY KEY (UsuarioID),
    CONSTRAINT UQ_Usuarios_Correo UNIQUE (CorreoElectronico)
);
GO


CREATE TABLE Equipos
(
    EquipoID    INT IDENTITY(1,1) NOT NULL,
    TipoEquipo  VARCHAR(50)       NOT NULL,
    Modelo      VARCHAR(80)       NOT NULL,
    UsuarioID   INT               NOT NULL,

    CONSTRAINT PK_Equipos PRIMARY KEY (EquipoID),
    CONSTRAINT FK_Equipos_Usuarios FOREIGN KEY (UsuarioID)
        REFERENCES Usuarios (UsuarioID)
);
GO


CREATE TABLE Tecnicos
(
    TecnicoID     INT IDENTITY(1,1) NOT NULL,
    Nombre        VARCHAR(100)      NOT NULL,
    Especialidad  VARCHAR(80)       NOT NULL,

    CONSTRAINT PK_Tecnicos PRIMARY KEY (TecnicoID)
);
GO


CREATE TABLE Reparaciones
(
    ReparacionID    INT IDENTITY(1,1) NOT NULL,
    EquipoID        INT               NOT NULL,
    FechaSolicitud  DATE              NOT NULL,
    Estado          VARCHAR(20)       NOT NULL,

    CONSTRAINT PK_Reparaciones PRIMARY KEY (ReparacionID),
    CONSTRAINT FK_Reparaciones_Equipos FOREIGN KEY (EquipoID)
        REFERENCES Equipos (EquipoID),
    CONSTRAINT CK_Reparaciones_Estado CHECK
        (Estado IN ('Pendiente', 'En proceso', 'Terminada', 'Entregada'))
);
GO


CREATE TABLE DetallesReparacion
(
    DetalleID     INT IDENTITY(1,1) NOT NULL,
    ReparacionID  INT               NOT NULL,
    Descripcion   VARCHAR(250)      NOT NULL,
    FechaInicio   DATE              NULL,
    FechaFin      DATE              NULL,

    CONSTRAINT PK_DetallesReparacion PRIMARY KEY (DetalleID),
    CONSTRAINT FK_Detalles_Reparaciones FOREIGN KEY (ReparacionID)
        REFERENCES Reparaciones (ReparacionID),
    -- la fecha de fin no puede ser anterior a la de inicio
    CONSTRAINT CK_Detalles_Fechas CHECK (FechaFin IS NULL OR FechaFin >= FechaInicio)
);
GO


CREATE TABLE Asignaciones
(
    AsignacionID     INT IDENTITY(1,1) NOT NULL,
    ReparacionID     INT               NOT NULL,
    TecnicoID        INT               NOT NULL,
    FechaAsignacion  DATE              NOT NULL,

    CONSTRAINT PK_Asignaciones PRIMARY KEY (AsignacionID),
    CONSTRAINT FK_Asignaciones_Reparaciones FOREIGN KEY (ReparacionID)
        REFERENCES Reparaciones (ReparacionID),
    CONSTRAINT FK_Asignaciones_Tecnicos FOREIGN KEY (TecnicoID)
        REFERENCES Tecnicos (TecnicoID),
    -- no se puede asignar dos veces el mismo tecnico a la misma reparacion
    CONSTRAINT UQ_Asignaciones UNIQUE (ReparacionID, TecnicoID)
);
GO


/* Esta tabla no viene en el diagrama. La agregue aparte para el login
   del punto 5, asi no hay que modificar la tabla Usuarios (que son los
   clientes del taller, no las personas que entran al sistema).
   La contrasena se guarda con SHA-256, nunca en texto plano. */
CREATE TABLE UsuariosSistema
(
    UsuarioSistemaID  INT IDENTITY(1,1) NOT NULL,
    NombreUsuario     VARCHAR(30)       NOT NULL,
    Contrasena        VARCHAR(64)       NOT NULL,
    Rol               VARCHAR(20)       NOT NULL,
    Activo            BIT               NOT NULL
        CONSTRAINT DF_UsuariosSistema_Activo DEFAULT (1),

    CONSTRAINT PK_UsuariosSistema PRIMARY KEY (UsuarioSistemaID),
    CONSTRAINT UQ_UsuariosSistema_Nombre UNIQUE (NombreUsuario)
);
GO


-- Indices para las busquedas que mas se usan en el sistema
CREATE INDEX IX_Equipos_UsuarioID ON Equipos (UsuarioID);
CREATE INDEX IX_Reparaciones_EquipoID ON Reparaciones (EquipoID);
CREATE INDEX IX_Detalles_ReparacionID ON DetallesReparacion (ReparacionID);
CREATE INDEX IX_Asignaciones_TecnicoID ON Asignaciones (TecnicoID);
GO
