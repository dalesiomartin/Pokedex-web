using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;

namespace winform_app1
{
    public partial class frmPokemons : Form
    {
        private List<Pokemon> listaPokemon;//creo un atributo
                
        public frmPokemons() //constructor
        {
            InitializeComponent();
        }

        private void frmPokemons_Load(object sender, EventArgs e)
        {

            cargar();

            //despues de cargar la lisa, cargo los desplegables de la busqueda avanzada
            cboCampo.Items.Add("Número");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Descripción");



        }

        private void cargar() {

            //aca voy a invocar la lectora de la base de datos que ya hice en las clases
            PokemonNegocio negocio = new PokemonNegocio();

            //Para trabajar con los null, uso TRY

            try
            {
                listaPokemon = negocio.listar();
                dgwPokemons.DataSource = listaPokemon; //modifico para poder utilizar la lista para mas cosas
                                                       //pbxPokemon.Load(listaPokemon[0].UrlImagen); //traigo el primer elementos de la lista

                //para AJUSTAR LA IMAGEN es con ZiseMode: strechImag

                ocultarColumnas();

                cargarImagen(listaPokemon[0].UrlImagen);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void ocultarColumnas() {
            //para ocultar una columna que no me interesa que se vea
            dgwPokemons.Columns["UrlImagen"].Visible = false;
            dgwPokemons.Columns["Id"].Visible = false;

        }

        //ESTO SALE DE EVENTOS DE frmPOKEMONS.Diseño
        private void dgwPokemons_SelectionChanged(object sender, EventArgs e)
        { //cuando cambie la seleccion de la grilla, quiero que cambie la imagen, el resto de los elementos

            // dgwPokemons.CurrentRow.DataBoundItem;
            /*me sale error porque es un objet, pero yo estoy manejando una lista Pokemon,
            entonces voy a forzar y le dire es un objeto Pokemon, lo transformo a pokemon */


            // cuando filtro se me rompio por la seleccion del renglon de la lista
            if (dgwPokemons.CurrentRow != null)
            {
                Pokemon seleccionado = (Pokemon)dgwPokemons.CurrentRow.DataBoundItem;
                //pbxPokemon.Load(seleccionado.UrlImagen); //ahora habilito que cada vez que cambie, de la imagen corresp
                cargarImagen(seleccionado.UrlImagen);
            }
           
        }

        private void cargarImagen(string imagen) { //necesito modular los dos metodos anteriores, elem [0] y del resto
            //para trabajar la excepcion con try
           
            try
            {
                pbxPokemon.Load(imagen);
            }
            catch (Exception )
            {
                pbxPokemon.Load("https://t3.ftcdn.net/jpg/02/48/42/64/360_F_248426448_NVKLywWqArG2ADUxDq6QprtIzsF82dMF.jpg");
                //busco una imagen de error, le doy copiar direccion imagen y pego enlace
            }

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaPokemon alta = new frmAltaPokemon();
            alta.ShowDialog();
            //Para actualizar la carga en la misma pantalla, debo actualizar la base de datos
            cargar(); //con esto actualizo la tabla

        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            //la diferencia con agregar, Es que voy a traer los datos del pokemon que quiero modificar
            Pokemon seleccionado;
            seleccionado = (Pokemon)dgwPokemons.CurrentRow.DataBoundItem;

            //lo que hare ahora, es pasarselo por parametros al constructor de la clase(frmAltaPokemon)
            //de este tengo uno vacio (va para la ALTA), pero hare otro recargandolo (MODIFACACION)

            frmAltaPokemon modificar = new frmAltaPokemon(seleccionado);
            modificar.ShowDialog();
            cargar();

            //para ello debere cargar un atributo privado de pokemon=null, para poder realizar un pasaje entre ventanas

        }

        private void btnEliminaFisica_Click(object sender, EventArgs e)
        {
            eliminar();

        }

        private void btnEliminarLogico_Click(object sender, EventArgs e)
        {
            eliminar(true);

        }

        //como tengo dos eliminar, voy a crear el metodo eliminar para no escribir tanto lo mismo
        public void eliminar(bool logico = false) {
            //con esto estoy mandando un valor opcional, es una bandera
            
            PokemonNegocio negocio = new PokemonNegocio();
            Pokemon seleccionado;

            try
            {
                DialogResult respuesta = MessageBox.Show("Seguro que quieres eliminar????", "Elimiando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                //me guardo el metodo para cargar un condicional
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Pokemon)dgwPokemons.CurrentRow.DataBoundItem;

                    if (logico)
                    {
                        negocio.eliminarLogico(seleccionado.Id);
                    }
                    else {
                        negocio.eliminar(seleccionado.Id);
                    }
                    
                  cargar();
                }



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


        }


        private bool validarFiltro() {
            if (cboCampo.SelectedIndex <0)  //SelectedIndex >= 0 significa que algo selecciono
            {
                MessageBox.Show("Por favor, seleccione el CAMPO para filtrar");
                return true;
            }
            if (cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el CRITERIO para filtrar");
                return true;
            }

            if (cboCampo.SelectedItem.ToString() == "Número") {

                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text))
                {
                    MessageBox.Show("Debes cargar el filtro para numericos...");
                    return true;
                }
                if (!(soloNumeros(txtFiltroAvanzado.Text)))
                {
                    MessageBox.Show("Solo nros para filtrar por un campo numerico.....");
                    return true;
                }
            }

            return false;   
        }

        private bool soloNumeros(string cadena) {

            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                {
                    return false;
                }
                
            }


            return true;    
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            //ACA DEBO BUSCAR LOS TRES CAMPOS

            PokemonNegocio negocio = new PokemonNegocio();
            try
            {
                if (validarFiltro())
                {
                    return;
                }
                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;
                dgwPokemons.DataSource = negocio.filtrar(campo, criterio, filtro);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }



            /*
            // La lista cuando se carga el formulario private List<Pokemon> listaPokemon; la usare aca
          // lo que hare es de lo que venga de la caja de texto, lo aplicare sobre la lista

            //creo otra lista, para vincularlo a la lista pokemon, recordar que es una coleccion
            List<Pokemon> listaFiltrada; //voy a usar FindAll, que requiere de unos parametros
            string filtro = txtFiltro.Text;

            if (filtro != "")
            {
                // listaFiltrada = listaPokemon.FindAll(x => x.Nombre.ToUpper() == filtro.ToUpper());
                //toUpper lo que hace es pasar todo a mayuscula
                // y para buscar similitudes pero con letras y no palabra exacta, uso Contains
                listaFiltrada = listaPokemon.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Descripcion.ToUpper().Contains(filtro.ToUpper()));

            }
            else
            {
                listaFiltrada = listaPokemon; //en caso de no encotrar, me traiga la lista completa
            }
 

            //El FindAll hara una especie de For, donde en cada vuelta, va a alojar un objeto y 
             //despues el siguiente, va evaluar en caso de coincidir el nombre del objeto con el 
            // filtro que aplique, todos los que coincidan los va a guardar en listaFiltrada



            dgwPokemons.DataSource = null; //debo limpiar previamente con el null
            dgwPokemons.DataSource = listaFiltrada; //esto para darle un nuevo origen

            //con esto asi, filtra por palabra exacta
            ocultarColumnas();

            */
        }

        private void txtFiltro_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {  // ES UN FILTRO RAPIDO QUE NO AFECTA A LA BD SINO QUE LO HACE DIRECTAMENTE EN LA APP
            // La lista cuando se carga el formulario private List<Pokemon> listaPokemon; la usare aca
            // lo que hare es de lo que venga de la caja de texto, lo aplicare sobre la lista

            //creo otra lista, para vincularlo a la lista pokemon, recordar que es una coleccion
            List<Pokemon> listaFiltrada; //voy a usar FindAll, que requiere de unos parametros
            string filtro = txtFiltro.Text;

            if (filtro.Length >= 3 )
            {
                // listaFiltrada = listaPokemon.FindAll(x => x.Nombre.ToUpper() == filtro.ToUpper());
                //toUpper lo que hace es pasar todo a mayuscula
                // y para buscar similitudes pero con letras y no palabra exacta, uso Contains
                listaFiltrada = listaPokemon.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Descripcion.ToUpper().Contains(filtro.ToUpper()));

            }
            else
            {
                listaFiltrada = listaPokemon; //en caso de no encotrar, me traiga la lista completa
            }


            /*El FindAll hara una especie de For, donde en cada vuelta, va a alojar un objeto y 
             despues el siguiente, va evaluar en caso de coincidir el nombre del objeto con el 
            filtro que aplique, todos los que coincidan los va a guardar en listaFiltrada*/



            dgwPokemons.DataSource = null; //debo limpiar previamente con el null
            dgwPokemons.DataSource = listaFiltrada; //esto para darle un nuevo origen

            //con esto asi, filtra por palabra exacta
            ocultarColumnas();
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {

            string opcion = cboCampo.SelectedItem.ToString();
            if (opcion == "Número")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");

            }
            else {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Empieza con");
                cboCriterio.Items.Add("Igual a");

            }



        }





        /* ANOTACIONES 
        * SelectionMode : FullRowSelect ==> sirve para que me seleccione toda la fila
        * EditMode : Programmatically : solo puedo modificar con programacion
        * MultiSelect : False : solo dejo que seleccione una linea y no multiple lineas
        * DropDownStyle= DropDownList para que no escriban en el desplegable
        */
    }
}
