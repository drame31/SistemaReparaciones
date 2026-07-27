# Sistema de Reparaciones

Proyecto del curso de Programación. Es un sistema web para llevar el control de un
taller de reparación de equipos: los clientes que dejan equipos, los técnicos que
los atienden, las reparaciones que se abren y el trabajo que se le hace a cada una.

Está hecho con ASP.NET Web Forms (C#, .NET Framework 4.7.2) y SQL Server.

## Base de datos

Los scripts están en la carpeta `BaseDatos` y hay que correrlos en orden:

| Archivo | Qué hace |
| --- | --- |
| `01_BaseDatos.sql` | Crea la base `TallerReparaciones` con las 7 tablas, llaves primarias, foráneas, restricciones e índices |
| `02_Procedimientos.sql` | Crea los 32 procedimientos almacenados |
| `03_DatosIniciales.sql` | Mete datos de prueba para que el sistema no arranque vacío |

Se pueden abrir en SQL Server Management Studio y darle F5 a cada uno, o desde la
consola:

```
sqlcmd -S .\SQLEXPRESS -E -i BaseDatos\01_BaseDatos.sql
sqlcmd -S .\SQLEXPRESS -E -i BaseDatos\02_Procedimientos.sql
sqlcmd -S .\SQLEXPRESS -E -i BaseDatos\03_DatosIniciales.sql
```

### Tablas

Las seis tablas del diagrama del enunciado:

- **Usuarios** – los clientes del taller
- **Equipos** – lo que deja cada cliente (va contra Usuarios)
- **Tecnicos** – el personal del taller
- **Reparaciones** – la orden de trabajo de un equipo
- **DetallesReparacion** – los trabajos concretos dentro de una reparación
- **Asignaciones** – qué técnico atiende cuál reparación

Y una más que agregué aparte:

- **UsuariosSistema** – las personas que entran al sistema. La puse separada
  porque la tabla `Usuarios` del diagrama son los clientes del taller, que no
  son los mismos que usan el programa. Las contraseñas se guardan en SHA-256,
  no en texto plano.

### Procedimientos almacenados

Cada tabla tiene sus cinco procedimientos: `sp_ListarX`, `sp_ObtenerX`,
`sp_InsertarX`, `sp_ActualizarX` y `sp_EliminarX`. Aparte están `sp_ValidarLogin`
y `sp_ResumenGeneral`, que es el que llena los contadores de la pantalla de inicio.

Toda la aplicación trabaja por procedimientos almacenados. No hay una sola
consulta SQL escrita dentro del código C#.

Algunas reglas quedaron dentro de los mismos procedimientos, por ejemplo no dejar
borrar un cliente que todavía tiene equipos registrados, o no repetir el correo
electrónico de un cliente.

## Cómo correrlo

1. Correr los tres scripts de la base de datos.
2. Abrir `SistemaReparaciones.sln` en Visual Studio.
3. Si la instancia de SQL Server no se llama `.\SQLEXPRESS`, cambiar la cadena de
   conexión en `Web.config`.
4. F5.

Usuarios para entrar:

| Usuario | Contraseña | Rol |
| --- | --- | --- |
| admin | admin123 | Administrador |
| tecnico | tecnico123 | Técnico |

## Cómo está organizado

El proyecto está separado en capas:

```
SistemaReparaciones/
├── Modelo/      las clases que representan cada tabla
├── Datos/       el acceso a SQL Server, una clase por tabla
├── Logica/      las validaciones y reglas antes de tocar la base
├── Estilos/     el CSS
└── *.aspx       las pantallas
```

La idea es que una pantalla nunca habla directo con la base de datos. Llama a la
capa de lógica, esa valida y llama a la capa de datos, y la capa de datos ejecuta
el procedimiento almacenado. Si mañana hay que cambiar una validación se cambia en
un solo lugar.

`Datos/Conexion.cs` es el único archivo que abre conexiones a SQL Server, así la
cadena de conexión se lee una sola vez y las conexiones siempre se cierran.

`PaginaBase.cs` es de donde heredan todas las pantallas. Antes de cargar cualquier
cosa revisa que haya una sesión abierta y si no la hay devuelve al login, para no
tener que repetir ese chequeo en cada página.

## Pantallas

- **Login** – entrada al sistema
- **Inicio** – contadores y las últimas reparaciones
- **Equipos**, **Clientes**, **Tecnicos** – los mantenimientos que pedía el enunciado
- **Reparaciones**, **Detalles**, **Asignaciones** – el resto de las tablas

Todas las pantallas de mantenimiento funcionan igual: arriba el formulario para
agregar o editar y abajo la tabla con buscador y los botones de editar y eliminar.

## Cosas que quedaron pendientes

- Los roles se guardan en la sesión pero todavía no limitan lo que cada uno puede
  hacer. Un técnico ve lo mismo que un administrador.
- La búsqueda filtra en memoria después de traer la lista completa. Con pocos
  registros va bien, pero si la tabla creciera habría que hacer el filtro dentro
  del procedimiento almacenado.
- No hay paginación en las tablas.
