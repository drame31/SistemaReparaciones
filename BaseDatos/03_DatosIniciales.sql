/*
    Proyecto de Programacion - Taller de Reparaciones
    Script 3: datos de prueba para que el sistema no arranque vacio
*/

USE TallerReparaciones;
GO

-- Clientes
INSERT INTO Usuarios (Nombre, CorreoElectronico, Telefono) VALUES
    ('Marcela Rojas Vargas',   'marcela.rojas@gmail.com',  '8712-4409'),
    ('Luis Fernando Alfaro',   'lf.alfaro@hotmail.com',    '6043-1187'),
    ('Karla Jimenez Mora',     'karlajm@gmail.com',        '8390-7752'),
    ('Oficina Contable Ureña', 'admin@contableurena.cr',   '2265-8810'),
    ('Diego Sanchez Brenes',   'diego.sb92@gmail.com',     NULL);
GO

-- Tecnicos del taller
INSERT INTO Tecnicos (Nombre, Especialidad) VALUES
    ('Andres Quiros Leon',    'Laptops y portatiles'),
    ('Sofia Ramirez Castro',  'Impresoras'),
    ('Jose Pablo Mendez',     'Redes y servidores'),
    ('Natalia Vega Solano',   'Telefonos y tablets');
GO

-- Equipos que dejaron los clientes
INSERT INTO Equipos (TipoEquipo, Modelo, UsuarioID) VALUES
    ('Laptop',     'HP Pavilion 15-eh1021',   1),
    ('Impresora',  'Epson L3250',             1),
    ('Laptop',     'Lenovo ThinkPad E14',     2),
    ('Desktop',    'Dell OptiPlex 3080',      4),
    ('Tablet',     'Samsung Galaxy Tab A8',   3),
    ('Laptop',     'MacBook Air M1',          5),
    ('Impresora',  'HP LaserJet Pro M404',    4);
GO

-- Reparaciones
INSERT INTO Reparaciones (EquipoID, FechaSolicitud, Estado) VALUES
    (1, '2026-06-12', 'Entregada'),
    (2, '2026-06-28', 'Terminada'),
    (3, '2026-07-03', 'En proceso'),
    (4, '2026-07-09', 'En proceso'),
    (5, '2026-07-15', 'Pendiente'),
    (6, '2026-07-18', 'Pendiente'),
    (7, '2026-07-21', 'En proceso');
GO

-- Detalle del trabajo hecho en cada reparacion
INSERT INTO DetallesReparacion (ReparacionID, Descripcion, FechaInicio, FechaFin) VALUES
    (1, 'Cambio de disco mecanico por SSD de 480 GB y reinstalacion del sistema.', '2026-06-12', '2026-06-14'),
    (1, 'Limpieza interna y cambio de pasta termica.',                             '2026-06-14', '2026-06-14'),
    (2, 'Cabezal tapado. Se hizo limpieza profunda y prueba de inyectores.',        '2026-06-28', '2026-07-01'),
    (3, 'Bisagra derecha quebrada, se pidio el repuesto al proveedor.',             '2026-07-03', NULL),
    (4, 'La maquina no da video. Se esta probando con otra fuente de poder.',       '2026-07-09', NULL),
    (7, 'Error de atasco de papel constante, se reviso el rodillo de arrastre.',    '2026-07-21', NULL);
GO

-- Que tecnico atiende cada reparacion
INSERT INTO Asignaciones (ReparacionID, TecnicoID, FechaAsignacion) VALUES
    (1, 1, '2026-06-12'),
    (2, 2, '2026-06-28'),
    (3, 1, '2026-07-03'),
    (4, 3, '2026-07-09'),
    (5, 4, '2026-07-15'),
    (7, 2, '2026-07-21');
GO

/* Usuarios que pueden entrar al sistema.
   La contrasena se guarda como SHA-256 en mayusculas, igual que la
   calcula la clase Seguridad del proyecto.

   Credenciales de prueba:
     admin  / admin123
     tecnico / tecnico123 */
INSERT INTO UsuariosSistema (NombreUsuario, Contrasena, Rol, Activo) VALUES
    ('admin',   CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'admin123'),   2), 'Administrador', 1),
    ('tecnico', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'tecnico123'), 2), 'Tecnico',       1);
GO

SELECT 'Datos cargados correctamente' AS Resultado;
GO
