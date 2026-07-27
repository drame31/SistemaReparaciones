<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SistemaReparaciones.Default" Title="Inicio - Taller de Reparaciones" %>

<asp:Content ID="cuerpo" ContentPlaceHolderID="Contenido" runat="server">

    <h1 class="titulo-pagina">
        <asp:Literal ID="litSaludo" runat="server" />
    </h1>
    <p class="subtitulo-pagina">Resumen de lo que hay registrado en el taller.</p>

    <div class="tarjetas">
        <div class="tarjeta">
            <div class="numero"><asp:Literal ID="litEquipos" runat="server" /></div>
            <div class="etiqueta">Equipos registrados</div>
        </div>
        <div class="tarjeta">
            <div class="numero"><asp:Literal ID="litUsuarios" runat="server" /></div>
            <div class="etiqueta">Clientes</div>
        </div>
        <div class="tarjeta">
            <div class="numero"><asp:Literal ID="litTecnicos" runat="server" /></div>
            <div class="etiqueta">Tecnicos</div>
        </div>
        <div class="tarjeta abiertas">
            <div class="numero"><asp:Literal ID="litAbiertas" runat="server" /></div>
            <div class="etiqueta">Reparaciones sin terminar</div>
        </div>
    </div>

    <div class="panel">
        <h2>Ultimas reparaciones</h2>
        <asp:GridView ID="gvUltimas" runat="server" AutoGenerateColumns="false"
            CssClass="tabla" GridLines="None" UseAccessibleHeader="true"
            AlternatingRowStyle-CssClass="fila-alterna">
            <Columns>
                <asp:BoundField DataField="ReparacionID" HeaderText="#"
                    ItemStyle-CssClass="columna-id" HeaderStyle-CssClass="columna-id" />
                <asp:BoundField DataField="DescripcionEquipo" HeaderText="Equipo" />
                <asp:BoundField DataField="NombreUsuario" HeaderText="Cliente" />
                <asp:BoundField DataField="FechaSolicitud" HeaderText="Solicitud"
                    DataFormatString="{0:dd/MM/yyyy}" />
                <asp:TemplateField HeaderText="Estado">
                    <ItemTemplate>
                        <span class='estado <%# ClaseEstado(Eval("Estado").ToString()) %>'>
                            <%# Eval("Estado") %></span>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <div class="vacio">Todavia no hay reparaciones registradas.</div>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>

    <div class="panel">
        <h2>Accesos rapidos</h2>
        <div class="cuerpo atajos">
            <a class="boton secundario" href="Equipos.aspx">Registrar un equipo</a>
            <a class="boton secundario" href="Usuarios.aspx">Agregar un cliente</a>
            <a class="boton secundario" href="Reparaciones.aspx">Abrir una reparacion</a>
            <a class="boton secundario" href="Asignaciones.aspx">Asignar un tecnico</a>
        </div>
    </div>

</asp:Content>
