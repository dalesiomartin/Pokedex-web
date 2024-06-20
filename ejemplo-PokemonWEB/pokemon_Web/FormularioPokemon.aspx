<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="FormularioPokemon.aspx.cs" Inherits="pokemon_Web.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row">
        <div class="col-6">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <div class="mb-3">
                <label for="txtId" class="form-label">Id</label>
                <asp:TextBox ID="txtId" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="mb-3">
                <label for="txtNombre" class="form-label">Nombre:</label>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="mb-3">
                <label for="txtNumero" class="form-label">Número:</label>
                <asp:TextBox ID="txtNumero" runat="server" CssClass="form-control"></asp:TextBox>
            </div>



            <div class="mb-3">
                <label for="ddlTipo" class="form-label">Tipo:</label>
                <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>

            <div class="mb-3">
                <label for="ddlDebilidad" class="form-label">Debilidad</label>
                <asp:DropDownList ID="ddlDebilidad" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>

            <div class="mb-3">
                <asp:Button ID="btnAceptar" runat="server" CssClass="btn btn-primary" OnClick="btnAceptar_Click" Text="Aceptar" />
                <a href="PokemonsLista.aspx">Cancelar</a>
                <asp:Button Text="Inactivar" ID="btnInactivar" OnClick="btnInactivar_Click" CssClass="btn btn-warning" runat="server" />

            </div>

        </div>

        <div class="col-6">
            <div class="mb-3">
                <label for="txtDescripcion" class="form-label">Descripción:</label>
                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
            </div>


            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="mb-3">
                        <label for="txtImagenUrl" class="form-label">Url Imagen</label>
                        <asp:TextBox ID="txtImagenUrl" runat="server" CssClass="form-control"
                            AutoPostBack="true" OnTextChanged="txtImagenUrl_TextChanged"></asp:TextBox>
                    </div>
                    <asp:Image ImageUrl="https://t4.ftcdn.net/jpg/05/97/47/95/240_F_597479556_7bbQ7t4Z8k3xbAloHFHVdZIizWK1PdOo.jpg"
                        ID="imgPokemon" runat="server" Width="60%" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="row">
            <div class="col-6">

                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnEliminar" runat="server" OnClick="btnEliminar_Click" CssClass="btn btn-danger" Text="Eliminar" />
                        </div>

            <% if (ConfirmaEliminacion)
                {%>
                        <div class="mb-3">
                            <asp:CheckBox Text="Confirmar Eliminacion" ID="chkConfirmaEliminacion" runat="server" />
                            <asp:Button ID="ConfirmaEliminar" runat="server"  OnClick="ConfirmaEliminar_Click" CssClass="btn btn-outline-danger" Text="Eliminar" />
                        </div>
                        </div>
              <% }%>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
</asp:Content>
