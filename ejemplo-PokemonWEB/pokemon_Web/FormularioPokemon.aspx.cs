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
        public bool ConfirmaEliminacion { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            txtId.Enabled = false;
            ConfirmaEliminacion = false;

            try
            {
                //CONFIGURACION INICIAL DE LA PANTALLA
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

                //CONFIGURACION SI ESTAMOS MODIFICANDO
                //aplico un operador ternario
                string id = Request.QueryString["id"] != null ? Request.QueryString["id"].ToString() : "";
                if (id != "" && !IsPostBack)
                {
                    PokemonNegocio negocio = new PokemonNegocio();
                    //List<Pokemon> lista = negocio.listar(id); //  esto devuelve una lista
                    //Pokemon seleccionado = lista[0]; //tomo el primer elemento de la lista
                    //simplifico las lineas anteriores por esta
                    Pokemon seleccionado = (negocio.listar(id))[0];

                    //guardo el pokemon seleccionado en session
                    Session.Add("pokeseleccionado", seleccionado);

                    //precargar los campos
                    txtId.Text = id;
                    txtNombre.Text = seleccionado.Nombre;
                    txtDescripcion.Text = seleccionado.Descripcion;
                    txtImagenUrl.Text = seleccionado.UrlImagen;
                    txtNumero.Text = seleccionado.Numero.ToString();

                    //cargo los precargables
                    ddlTipo.SelectedValue = seleccionado.Tipo.id.ToString();
                    ddlDebilidad.SelectedValue = seleccionado.Debilidad.id.ToString();

                    //para precargar la imagen por url, estoy forzando el metodo
                    txtImagenUrl_TextChanged(sender, e); //en el escritorio, usamos el metodo cargar, para no forzar los metodos


                    //CONFIG ACCIONES
                    if (!seleccionado.Activo)
                    {
                        btnInactivar.Text = "Reactivar";
                    }

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
                                                               //lo ideal que esto este arriba, debajo de pokemon nuevo

                if (Request.QueryString["id"] != null)
                {
                    nuevo.Id = int.Parse(txtId.Text); //FUNDAMENTAL CARGAR EL ID, SINO NO ME MODIF NADA
                    negocio.modificarConSP(nuevo);
                }
                else
                {
                    negocio.agregarConSP(nuevo);
                }

                Response.Redirect("PokemonsLista.aspx", false);


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

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            ConfirmaEliminacion= true;
        }

        protected void ConfirmaEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkConfirmaEliminacion.Checked)
                {
                    PokemonNegocio negocio = new PokemonNegocio();
                    negocio.eliminar(int.Parse(txtId.Text));
                    Response.Redirect("PokemonsLista.aspx");
                }
            }
                
            catch (Exception ex)
            {
                Session.Add("error", ex);
                throw;
            }
        }

        protected void btnInactivar_Click(object sender, EventArgs e)
        {
            try
            {
                PokemonNegocio negocio = new PokemonNegocio();
                Pokemon seleccionado = (Pokemon)Session["pokeseleccionado"];

                //negocio.eliminarLogico(int.Parse(txtId.Text)); // voy a modificar el parametro
                negocio.eliminarLogico(seleccionado.Id, !seleccionado.Activo); //toma el pokemon(id) y manda el estado opuesto (!activo)
                //caso de las banderas, en este caso quiero negar el estado del pokemon
                Response.Redirect("PokemonsLista.aspx");
            }
            catch (Exception ex)
            {

                Session.Add("error", ex);
            }
        }
    }
}