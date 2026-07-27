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
            </div>

            <asp:Button ID="btnEntrar" runat="server" Text="Entrar" CssClass="boton principal" OnClick="btnEntrar_Click" />

            <div class="pie-login">
                Usuarios de prueba:<br />
                admin / admin123<br />
                tecnico / tecnico123
            </div>

        </div>
    </form>
</body>
</html>
