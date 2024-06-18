using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dominio;
using negocio;

namespace pokemon_Web
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           txtId.Enabled = false;
            try
            {
                if (!IsPostBack)
                {
                    ElementoNegocio negocio = new ElementoNegocio();
                    List<Elemento> lista = negocio.listar();  //creo una lista de elementos desde la DB
                    ddlTipo.DataSource = lista;
                    ddlTipo.DataValueField = "id"; //config la propiedad del objeto que estoy leyendo
                    ddlTipo.DataTextField = "Descripcion";   // nombre de la propiedad de la clase
                    ddlTipo.DataBind();

                    ddlDebilidad.DataSource = lista;
                    ddlDebilidad.DataValueField = "id";
                    ddlDebilidad.DataTextField = "Descripcion";
                    ddlDebilidad.DataBind();

                }
            }
            catch (Exception ex)
            {

                Session.Add("error", ex); // lo capturo en la sesion
                throw; //para saber si esta fallando
                //puedo redireccionarlo a una pantalla de error
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                Pokemon nuevo = new Pokemon();//creo un nuevo objeto tipo pokemon
                
                nuevo.Numero = int.Parse(txtNumero.Text);
                nuevo.Nombre = txtNombre.Text;
                nuevo.Descripcion = txtDescripcion.Text;
                nuevo.UrlImagen = txtImagenUrl.Text;

                //para los desplegables debo cargar un nuevo objeto
                nuevo.Tipo = new Elemento();
                nuevo.Tipo.id = int.Parse(ddlTipo.SelectedValue);
                nuevo.Debilidad = new Elemento();
                nuevo.Debilidad.id = int.Parse(ddlDebilidad.SelectedValue);

                PokemonNegocio negocio = new PokemonNegocio(); //agrego otro objeto para cargar el pokemon
                negocio.agregarConSP(nuevo);
                Response.Redirect("PokemonsLista.aspx",false);


            }
            catch (Exception ex)
            {
                Session.Add("error", ex);
                throw;
            }
        }

        protected void txtImagenUrl_TextChanged(object sender, EventArgs e)
        {
            imgPokemon.ImageUrl = txtImagenUrl.Text;
        }
    }
}