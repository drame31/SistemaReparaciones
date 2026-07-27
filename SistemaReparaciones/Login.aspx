<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SistemaReparaciones.Login" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Ingresar - Taller de Reparaciones</title>
    <link rel="stylesheet" href="Estilos/estilos.css" />
</head>
<body class="pagina-login">
    <form id="form1" runat="server" defaultbutton="btnEntrar" defaultfocus="txtUsuario">
        <div class="caja-login">

            <h1>Taller de Reparaciones</h1>
            <p class="nota">Ingrese sus datos para entrar al sistema.</p>

            <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="mensaje mensaje-error" />

            <div class="campo">
                <label for="txtUsuario">Usuario</label>
                <asp:TextBox ID="txtUsuario" runat="server" MaxLength="30" autocomplete="username" />
            </div>

            <div class="campo">
                <label for="txtContrasena">Contrasena</label>
                <asp:TextBox ID="txtContrasena" runat="server" TextMode="Password" MaxLength="50" autocomplete="current-password" />
                <span id="avisoMayusculas" class="aviso-mayusculas">Tiene las mayusculas activadas (Bloq Mayus)</span>
            </div>

            <asp:Button ID="btnEntrar" runat="server" Text="Entrar" CssClass="boton principal" OnClick="btnEntrar_Click" />

            <div class="pie-login">
                Usuarios de prueba (haga clic para llenar el formulario):<br />
                <button type="button" class="enlace-usuario" onclick="llenar('admin', 'admin123');">admin / admin123</button><br />
                <button type="button" class="enlace-usuario" onclick="llenar('tecnico', 'tecnico123');">tecnico / tecnico123</button>
            </div>

        </div>
    </form>

    <script type="text/javascript">
        function llenar(usuario, clave) {
            document.getElementById('txtUsuario').value = usuario;
            document.getElementById('txtContrasena').value = clave;
            document.getElementById('txtContrasena').focus();
        }

        // La contrasena va cifrada, asi que una mayuscula de mas la invalida.
        // Este aviso evita perder tiempo buscando el error en otro lado.
        (function () {
            var clave = document.getElementById('txtContrasena');
            var aviso = document.getElementById('avisoMayusculas');

            function revisar(e) {
                var activado = e.getModifierState && e.getModifierState('CapsLock');
                aviso.style.display = activado ? 'block' : 'none';
            }

            clave.addEventListener('keydown', revisar);
            clave.addEventListener('keyup', revisar);
        })();
    </script>
</body>
</html>
