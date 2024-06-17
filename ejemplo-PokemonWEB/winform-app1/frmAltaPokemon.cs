using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;
using System.Configuration;

namespace winform_app1
{
    public partial class frmAltaPokemon : Form
    {
        private Pokemon pokemon = null;
        private OpenFileDialog archivo = null;

        public frmAltaPokemon()
        { //este constructo va con el agregar
            InitializeComponent();
        }

        //este viene de modificar pokemon
        public frmAltaPokemon(Pokemon pokemon)
        {
            InitializeComponent();
            this.pokemon = pokemon;
            Text = "Modificar Pokemon"; //me cambia el titulo de la pantalla
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            //Pokemon poke = new Pokemon(); // debo crear el objeto tipo pokemon, agrego using dominio
            PokemonNegocio negocio = new PokemonNegocio();  
           
            //NOTA: AL MOMENTO DEL BOTON MODIFICAR, AGREGUE UN ATRIBUTO LLAMADO pokemon=null
            //entonces ya no necesito poke, entonces lo que hare sera cambiarlamon
            
            try
            {
                //recorda que mi atrib pokemon =null, quiere decir que si es nulo, ES NUEVO POKEMON
                if (pokemon == null)
                    pokemon = new Pokemon();
                

                //agrego atributos del objeto 
                pokemon.Numero = int.Parse(txtNumero.Text);
                pokemon.Nombre = txtNombre.Text;
                pokemon.Descripcion = txtDescripcion.Text;
                pokemon.UrlImagen = txtUrlImagen.Text; //le agrego el evento LEAVE dentro de txtUrlImagen
               //sin este ultimo, fallo al cargar la imgagen pero tiene el tratamiento null
               //tambien cada campo que agregue, debo cargarlo a la consulta sql dentro del programa
                pokemon.Tipo = (Elemento)cboTipo.SelectedItem;
                pokemon.Debilidad = (Elemento)cboDebilidad.SelectedItem;

                //ahora debo enviarlo a la base de datos con pokemon negocio, con el metodo agregar
                //para ello debo invocar al proyecto negocio :using negocio;

                if (pokemon.Id !=0)
                {
                    negocio.modificar(pokemon);
                    MessageBox.Show("Modificado con exito!!");
                 
                }
                else
                {
                    negocio.agregar(pokemon);
                    MessageBox.Show("Agregado exitosamente!!");
                    
                }

                Close(); //es para que cierre y vuelva a la ventana principal



                //HASTA ACA SOLO ESTA EL FRON, FALTA LA LOGICA EN public void agregar(Pokemon nuevo){}


                //guardar imagen si la levanto localmente
                if (archivo != null && !(txtUrlImagen.Text.ToUpper().Contains("HTTP"))) //esto quiere decir que estas levantando una imagen local
                {
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName); //aca guardo la ruta y el nombre original del archivo

                    //lo que hace esto es: si me equivoque al usar una imagen local y luego selecciono una de internet, la imagen local no queda 
                    // gravada en la carpeta sino el enlace a internet. Hasta que le de aceptar

                }

               


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void frmAltaPokemon_Load(object sender, EventArgs e)
        {
            ElementoNegocio elementoNegocio = new ElementoNegocio();
            
            try
            {
                
                cboTipo.DataSource = elementoNegocio.listar();
                cboTipo.ValueMember = "id"; //aca le digo cual es su valor clave
                cboTipo.DisplayMember = "Descripcion";
                //estos son los nombres de las propiedades de la clase elemento

                cboDebilidad.DataSource = elementoNegocio.listar();
                cboDebilidad.ValueMember = "id";
                cboDebilidad.DisplayMember = "Descripcion";

                //ambas si bien tiene los mismo datos, si los uniera, el programa se rompe

                //para validar que sobre escriban y solo seleccione la opcion que les cargo
                // uso la propiedad DropDownStyle => DropDownList

                //para modificar el pokemon, quiero precargar los datos y debo hacer una validacion
                if (pokemon != null) { //quiere decir que tengo un pokemon para modificar
                                       //entonces debo precargarlo

                    txtNumero.Text = pokemon.Numero.ToString();
                    txtNombre.Text = pokemon.Nombre;
                    txtDescripcion.Text = pokemon.Descripcion;
                    txtUrlImagen.Text = pokemon.UrlImagen;
                    cargarImagen(pokemon.UrlImagen); //esto es para que me cargue las imagenes vacias

                    //ahora para cargar los valores plegables, debo agregarlo una conf mas
                    /*Estos son los desplegables
                     cboTipo.DataSource = elementoNegocio.listar();
                        cboDebilidad.DataSource = elementoNegocio.listar();
                    Ahora le que hare es decirle cual quiero que sea CLAVE y cual quiero que sea su VALUE
                    para despues decirle que value quiero que este preseleccionado
                     */
                    cboTipo.SelectedValue = pokemon.Tipo.id;
                    cboDebilidad.SelectedValue = pokemon.Debilidad.id;

                }




            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        { //Copio el metodo cargarImagen. lo podria tener en otra clase
            //tener el mismo metodo en dos formulario, no es lo mejor
            cargarImagen(txtUrlImagen.Text);
        }

        private void cargarImagen(string imagen)
        { //necesito modular los dos metodos anteriores, elem [0] y del resto
          //para trabajar la excepcion con try

            try
            {
                pbxPokemon.Load(imagen);
            }
            catch (Exception)
            {
                pbxPokemon.Load("https://t3.ftcdn.net/jpg/02/48/42/64/360_F_248426448_NVKLywWqArG2ADUxDq6QprtIzsF82dMF.jpg");
                //busco una imagen de error, le doy copiar direccion imagen y pego enlace
            }

        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            //creo un objeto OpenFileDialog
            OpenFileDialog archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg;|png|*.png"; //el |* son para las extensiones
          
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtUrlImagen.Text = archivo.FileName; //esto me guarda la ruta completa del archivo que estoy seleccionando
                cargarImagen(archivo.FileName);

                //guardar la imagen
                //  File.Copy(archivo.FileName, "C:");  //toma la ruta del archivo y lo otro es su destino
                /* DENTRO DE APP.CONFIG en winform-app1, cargo la siguiente config
                <appSettings>
	            	<add key="images-folder" value="C:\poke-app"/>
	            </appSettings>

                luego en REFERENCIAS, Agregar Referencias, Ensamblados(Assembles), System.Configuration eso me agrega la ref a archivo de la config
                y cargo using System.Configuration;
                 
                Hago esto para leerlo directamente de este archivo de configuracion, sin hacer el cofigo de acceso
                 */

               // File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName); //aca guardo la ruta y el nombre original del archivo

            }

        }
    }
}
