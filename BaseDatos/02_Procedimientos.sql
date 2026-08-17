/*
    Proyecto de Programacion - Taller de Reparaciones
    Script 2: procedimientos almacenados del CRUD

    Un grupo de 5 procedimientos por tabla (listar, obtener, insertar,
    actualizar, eliminar) mas el de validacion del login.
    Toda la aplicacion trabaja unicamente con estos procedimientos,
    no hay SQL escrito dentro del codigo C#.

    Los procedimientos de listar reciben @Busqueda, que es lo que el
    usuario escribio en el buscador de la pantalla. Si llega NULL se
    devuelve la tabla completa. El filtro se hace aqui y no en C# para
    no traerse todas las filas y descartarlas despues.
*/

USE TallerReparaciones;
GO

/* ============================================================
   USUARIOS
   ============================================================ */

CREATE OR ALTER PROCEDURE sp_ListarUsuarios
    @Busqueda VARCHAR(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT UsuarioID, Nombre, CorreoElectronico, Telefono
    FROM Usuarios
    WHERE @Busqueda IS NULL
       OR Nombre            LIKE '%' + @Busqueda + '%'
       OR CorreoElectronico LIKE '%' + @Busqueda + '%'
       OR Telefono          LIKE '%' + @Busqueda + '%'
    ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE sp_ObtenerUsuario
    @UsuarioID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT UsuarioID, Nombre, CorreoElectronico, Telefono
    FROM Usuarios
    WHERE UsuarioID = @UsuarioID;
END
GO

CREATE OR ALTER PROCEDURE sp_InsertarUsuario
    @Nombre            VARCHAR(100),
    @CorreoElectronico VARCHAR(100),
    @Telefono          VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Usuarios WHERE CorreoElectronico = @CorreoElectronico)
    BEGIN
        RAISERROR('Ya existe un cliente registrado con ese correo.', 16, 1);
        RETURN;
    END

    INSERT INTO Usuarios (Nombre, CorreoElectronico, Telefono)
    VALUES (@Nombre, @CorreoElectronico, @Telefono);

    SELECT SCOPE_IDENTITY() AS UsuarioID;
END
GO

CREATE OR ALTER PROCEDURE sp_ActualizarUsuario
    @UsuarioID         INT,
    @Nombre            VARCHAR(100),
    @CorreoElectronico VARCHAR(100),
    @Telefono          VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Usuarios
               WHERE CorreoElectronico = @CorreoElectronico AND UsuarioID <> @UsuarioID)
    BEGIN
        RAISERROR('Ese correo ya lo tiene otro cliente.', 16, 1);
        RETURN;
    END

    UPDATE Usuarios
    SET Nombre            = @Nombre,
        CorreoElectronico = @CorreoElectronico,
        Telefono          = @Telefono
    WHERE UsuarioID = @UsuarioID;
END
GO

CREATE OR ALTER PROCEDURE sp_EliminarUsuario
    @UsuarioID INT
AS
BEGIN
    SET NOCOUNT ON;

    -- No se borra si tiene equipos registrados, si no la llave foranea revienta
    IF EXISTS (SELECT 1 FROM Equipos WHERE UsuarioID = @UsuarioID)
    BEGIN
        RAISERROR('No se puede eliminar: el cliente tiene equipos registrados.', 16, 1);
        RETURN;
    END

    DELETE FROM Usuarios WHERE UsuarioID = @UsuarioID;
END
GO


/* ============================================================
   EQUIPOS
   ============================================================ */

CREATE OR ALTER PROCEDURE sp_ListarEquipos
    @Busqueda VARCHAR(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT e.EquipoID,
           e.TipoEquipo,
           e.Modelo,
           e.UsuarioID,
           u.Nombre AS NombreUsuario
    FROM Equipos e
         INNER JOIN Usuarios u ON u.UsuarioID = e.UsuarioID
    WHERE @Busqueda IS NULL
       OR e.TipoEquipo LIKE '%' + @Busqueda + '%'
       OR e.Modelo     LIKE '%' + @Busqueda + '%'
       OR u.Nombre     LIKE '%' + @Busqueda + '%'
    ORDER BY e.EquipoID DESC;
END
GO

CREATE OR ALTER PROCEDURE sp_ObtenerEquipo
    @EquipoID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT e.EquipoID,
           e.TipoEquipo,
           e.Modelo,
           e.UsuarioID,
           u.Nombre AS NombreUsuario
    FROM Equipos e
         INNER JOIN Usuarios u ON u.UsuarioID = e.UsuarioID
    WHERE e.EquipoID = @EquipoID;
END
GO

CREATE OR ALTER PROCEDURE sp_InsertarEquipo
    @TipoEquipo VARCHAR(50),
    @Modelo     VARCHAR(80),
    @UsuarioID  INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Equipos (TipoEquipo, Modelo, UsuarioID)
    VALUES (@TipoEquipo, @Modelo, @UsuarioID);

    SELECT SCOPE_IDENTITY() AS EquipoID;
END
GO

CREATE OR ALTER PROCEDURE sp_ActualizarEquipo
    @EquipoID   INT,
    @TipoEquipo VARCHAR(50),
    @Modelo     VARCHAR(80),
    @UsuarioID  INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Equipos
    SET TipoEquipo = @TipoEquipo,
        Modelo     = @Modelo,
        UsuarioID  = @UsuarioID
    WHERE EquipoID = @EquipoID;
END
GO

CREATE OR ALTER PROCEDURE sp_EliminarEquipo
    @EquipoID INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Reparaciones WHERE EquipoID = @EquipoID)
    BEGIN
        RAISERROR('No se puede eliminar: el equipo tiene reparaciones registradas.', 16, 1);
        RETURN;
    END

    DELETE FROM Equipos WHERE EquipoID = @EquipoID;
END
GO


/* ============================================================
   TECNICOS
   ============================================================ */

CREATE OR ALTER PROCEDURE sp_ListarTecnicos
    @Busqueda VARCHAR(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TecnicoID, Nombre, Especialidad
    FROM Tecnicos
    WHERE @Busqueda IS NULL
       OR Nombre       LIKE '%' + @Busqueda + '%'
       OR Especialidad LIKE '%' + @Busqueda + '%'
    ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE sp_ObtenerTecnico
    @TecnicoID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TecnicoID, Nombre, Especialidad
    FROM Tecnicos
    WHERE TecnicoID = @TecnicoID;
END
GO

CREATE OR ALTER PROCEDURE sp_InsertarTecnico
    @Nombre       VARCHAR(100),
    @Especialidad VARCHAR(80)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Tecnicos (Nombre, Especialidad)
    VALUES (@Nombre, @Especialidad);

    SELECT SCOPE_IDENTITY() AS TecnicoID;
END
GO

CREATE OR ALTER PROCEDURE sp_ActualizarTecnico
    @TecnicoID    INT,
    @Nombre       VARCHAR(100),
    @Especialidad VARCHAR(80)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Tecnicos
    SET Nombre       = @Nombre,
        Especialidad = @Especialidad
    WHERE TecnicoID = @TecnicoID;
END
GO

CREATE OR ALTER PROCEDURE sp_EliminarTecnico
    @TecnicoID INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Asignaciones WHERE TecnicoID = @TecnicoID)
    BEGIN
        RAISERROR('No se puede eliminar: el tecnico tiene asignaciones.', 16, 1);
        RETURN;
    END

    DELETE FROM Tecnicos WHERE TecnicoID = @TecnicoID;
END
GO


/* ============================================================
   REPARACIONES
   ============================================================ */

CREATE OR ALTER PROCEDURE sp_ListarReparaciones
    @Busqueda VARCHAR(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT r.ReparacionID,
           r.EquipoID,
           r.FechaSolicitud,
           r.Estado,
           e.TipoEquipo + ' ' + e.Modelo AS DescripcionEquipo,
           u.Nombre AS NombreUsuario
    FROM Reparaciones r
         INNER JOIN Equipos  e ON e.EquipoID  = r.EquipoID
         INNER JOIN Usuarios u ON u.UsuarioID = e.UsuarioID
    WHERE @Busqueda IS NULL
       OR e.TipoEquipo + ' ' + e.Modelo LIKE '%' + @Busqueda + '%'
       OR u.Nombre                      LIKE '%' + @Busqueda + '%'
       OR r.Estado                      LIKE '%' + @Busqueda + '%'
    ORDER BY r.FechaSolicitud DESC, r.ReparacionID DESC;
END
GO

CREATE OR ALTER PROCEDURE sp_ObtenerReparacion
    @ReparacionID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT r.ReparacionID,
           r.EquipoID,
           r.FechaSolicitud,
           r.Estado,
           e.TipoEquipo + ' ' + e.Modelo AS DescripcionEquipo,
           u.Nombre AS NombreUsuario
    FROM Reparaciones r
         INNER JOIN Equipos  e ON e.EquipoID  = r.EquipoID
         INNER JOIN Usuarios u ON u.UsuarioID = e.UsuarioID
    WHERE r.ReparacionID = @ReparacionID;
END
GO

CREATE OR ALTER PROCEDURE sp_InsertarReparacion
    @EquipoID       INT,
    @FechaSolicitud DATE,
    @Estado         VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Reparaciones (EquipoID, FechaSolicitud, Estado)
    VALUES (@EquipoID, @FechaSolicitud, @Estado);

    SELECT SCOPE_IDENTITY() AS ReparacionID;
END
GO

CREATE OR ALTER PROCEDURE sp_ActualizarReparacion
    @ReparacionID   INT,
    @EquipoID       INT,
    @FechaSolicitud DATE,
    @Estado         VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Reparaciones
    SET EquipoID       = @EquipoID,
        FechaSolicitud = @FechaSolicitud,
        Estado         = @Estado
    WHERE ReparacionID = @ReparacionID;
END
GO

CREATE OR ALTER PROCEDURE sp_EliminarReparacion
    @ReparacionID INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Asignaciones WHERE ReparacionID = @ReparacionID)
       OR EXISTS (SELECT 1 FROM DetallesReparacion WHERE ReparacionID = @ReparacionID)
    BEGIN
        RAISERROR('No se puede eliminar: la reparacion tiene detalles o asignaciones.', 16, 1);
        RETURN;
    END

    DELETE FROM Reparaciones WHERE ReparacionID = @ReparacionID;
END
GO


/* ============================================================
   DETALLES DE REPARACION
   ============================================================ */

CREATE OR ALTER PROCEDURE sp_ListarDetalles
    @Busqueda VARCHAR(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT d.DetalleID,
           d.ReparacionID,
           d.Descripcion,
           d.FechaInicio,
           d.FechaFin,
           e.TipoEquipo + ' ' + e.Modelo AS DescripcionEquipo
    FROM DetallesReparacion d
         INNER JOIN Reparaciones r ON r.ReparacionID = d.ReparacionID
         INNER JOIN Equipos      e ON e.EquipoID     = r.EquipoID
    WHERE @Busqueda IS NULL
       OR d.Descripcion                 LIKE '%' + @Busqueda + '%'
       OR e.TipoEquipo + ' ' + e.Modelo LIKE '%' + @Busqueda + '%'
    ORDER BY d.DetalleID DESC;
END
GO

CREATE OR ALTER PROCEDURE sp_ObtenerDetalle
    @DetalleID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT d.DetalleID,
           d.ReparacionID,
           d.Descripcion,
           d.FechaInicio,
           d.FechaFin,
           e.TipoEquipo + ' ' + e.Modelo AS DescripcionEquipo
    FROM DetallesReparacion d
         INNER JOIN Reparaciones r ON r.ReparacionID = d.ReparacionID
         INNER JOIN Equipos      e ON e.EquipoID     = r.EquipoID
    WHERE d.DetalleID = @DetalleID;
END
GO

CREATE OR ALTER PROCEDURE sp_InsertarDetalle
    @ReparacionID INT,
    @Descripcion  VARCHAR(250),
    @FechaInicio  DATE,
    @FechaFin     DATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO DetallesReparacion (ReparacionID, Descripcion, FechaInicio, FechaFin)
    VALUES (@ReparacionID, @Descripcion, @FechaInicio, @FechaFin);

    SELECT SCOPE_IDENTITY() AS DetalleID;
END
GO

CREATE OR ALTER PROCEDURE sp_ActualizarDetalle
    @DetalleID    INT,
    @ReparacionID INT,
    @Descripcion  VARCHAR(250),
    @FechaInicio  DATE,
    @FechaFin     DATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE DetallesReparacion
    SET ReparacionID = @ReparacionID,
        Descripcion  = @Descripcion,
        FechaInicio  = @FechaInicio,
        FechaFin     = @FechaFin
    WHERE DetalleID = @DetalleID;
END
GO

CREATE OR ALTER PROCEDURE sp_EliminarDetalle
    @DetalleID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM DetallesReparacion WHERE DetalleID = @DetalleID;
END
GO


/* ============================================================
   ASIGNACIONES
   ============================================================ */

CREATE OR ALTER PROCEDURE sp_ListarAsignaciones
    @Busqueda VARCHAR(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT a.AsignacionID,
           a.ReparacionID,
           a.TecnicoID,
           a.FechaAsignacion,
           t.Nombre AS NombreTecnico,
           e.TipoEquipo + ' ' + e.Modelo AS DescripcionEquipo
    FROM Asignaciones a
         INNER JOIN Tecnicos     t ON t.TecnicoID    = a.TecnicoID
         INNER JOIN Reparaciones r ON r.ReparacionID = a.ReparacionID
         INNER JOIN Equipos      e ON e.EquipoID     = r.EquipoID
    WHERE @Busqueda IS NULL
       OR t.Nombre                      LIKE '%' + @Busqueda + '%'
       OR e.TipoEquipo + ' ' + e.Modelo LIKE '%' + @Busqueda + '%'
    ORDER BY a.FechaAsignacion DESC, a.AsignacionID DESC;
END
GO

CREATE OR ALTER PROCEDURE sp_ObtenerAsignacion
    @AsignacionID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT a.AsignacionID,
           a.ReparacionID,
           a.TecnicoID,
           a.FechaAsignacion,
           t.Nombre AS NombreTecnico,
           e.TipoEquipo + ' ' + e.Modelo AS DescripcionEquipo
    FROM Asignaciones a
         INNER JOIN Tecnicos     t ON t.TecnicoID    = a.TecnicoID
         INNER JOIN Reparaciones r ON r.ReparacionID = a.ReparacionID
         INNER JOIN Equipos      e ON e.EquipoID     = r.EquipoID
    WHERE a.AsignacionID = @AsignacionID;
END
GO

CREATE OR ALTER PROCEDURE sp_InsertarAsignacion
    @ReparacionID    INT,
    @TecnicoID       INT,
    @FechaAsignacion DATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Asignaciones
               WHERE ReparacionID = @ReparacionID AND TecnicoID = @TecnicoID)
    BEGIN
        RAISERROR('Ese tecnico ya esta asignado a esta reparacion.', 16, 1);
        RETURN;
    END

    INSERT INTO Asignaciones (ReparacionID, TecnicoID, FechaAsignacion)
    VALUES (@ReparacionID, @TecnicoID, @FechaAsignacion);

    SELECT SCOPE_IDENTITY() AS AsignacionID;
END
GO

CREATE OR ALTER PROCEDURE sp_ActualizarAsignacion
    @AsignacionID    INT,
    @ReparacionID    INT,
    @TecnicoID       INT,
    @FechaAsignacion DATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Asignaciones
               WHERE ReparacionID = @ReparacionID
                 AND TecnicoID    = @TecnicoID
                 AND AsignacionID <> @AsignacionID)
    BEGIN
        RAISERROR('Ese tecnico ya esta asignado a esta reparacion.', 16, 1);
        RETURN;
    END

    UPDATE Asignaciones
    SET ReparacionID    = @ReparacionID,
        TecnicoID       = @TecnicoID,
        FechaAsignacion = @FechaAsignacion
    WHERE AsignacionID = @AsignacionID;
END
GO

CREATE OR ALTER PROCEDURE sp_EliminarAsignacion
    @AsignacionID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Asignaciones WHERE AsignacionID = @AsignacionID;
END
GO


/* ============================================================
   LOGIN Y RESUMEN
   ============================================================ */

/* La contrasena llega ya convertida a SHA-256 desde la capa de logica,
   aqui solo se compara. */
CREATE OR ALTER PROCEDURE sp_ValidarLogin
    @NombreUsuario VARCHAR(30),
    @Contrasena    VARCHAR(64)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT UsuarioSistemaID, NombreUsuario, Rol
    FROM UsuariosSistema
    WHERE NombreUsuario = @NombreUsuario
      AND Contrasena    = @Contrasena
      AND Activo        = 1;
END
GO

-- Contadores para la pantalla de inicio
CREATE OR ALTER PROCEDURE sp_ResumenGeneral
AS
BEGIN
    SET NOCOUNT ON;

    SELECT (SELECT COUNT(*) FROM Equipos)   AS TotalEquipos,
           (SELECT COUNT(*) FROM Usuarios)  AS TotalUsuarios,
           (SELECT COUNT(*) FROM Tecnicos)  AS TotalTecnicos,
           (SELECT COUNT(*) FROM Reparaciones
            WHERE Estado IN ('Pendiente', 'En proceso')) AS ReparacionesAbiertas;
END
GO
