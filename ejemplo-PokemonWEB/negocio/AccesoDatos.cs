using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace negocio
{
    public class AccesoDatos
    { //debo declarar los atributos para establecer los objetos para la conexion
      // agrgar using System.Data.SqlClient;
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;

        public SqlDataReader Lector
        { // con esto puedo leer a lector desde el exterior
            get { return lector; }
        }


        //necesito crear una conexion, 
        public AccesoDatos()
        { //cuando nace el objeto a acceso datos. Al momento de la conexion le paso por parametro la consulta, recargando el metodo
            //es otra forma a la que vi en pokemonNegocio
            conexion = new SqlConnection("Data Source=DESKTOP-G8FBE6Q\\SQLEXPRESS;Initial Catalog=POKEDEX_DB;Integrated Security=True");
            comando = new SqlCommand();

        }

        public void setearConsulta(string consulta) {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
            //de este modo, estoy encapsulando de darle un tipo y la consulta de la query

        }

        public void setearProcedimiento(string sp) {
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            comando.CommandText = sp;
        }

        public void ejecutarLectura() {
            comando.Connection = conexion;
            
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ejecutarAccion() {
            comando.Connection = conexion;           
            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void setearParametro(string nombre, object valor) 
        {
            comando.Parameters.AddWithValue(nombre, valor);  //AddWithValue(): me pide cargar un nombre y un valor
        }


        public void cerrarConexion() { 
            if  (lector != null)  //cierro el lector
                lector.Close(); // cierro la conexion
        
        }
    }
}
