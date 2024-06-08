using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{   //si quiero traer un listado de elementos, cargo using dominio
    public class ElementoNegocio
    { // todo lo que hice en listar en pokemonNegocio, lo hare en elementosNegocio
        public List<Elemento> listar () {

            List<Elemento> lista = new List<Elemento>();
            AccesoDatos datos = new AccesoDatos ();
            //nace un objeto, que tiene lector, un comando y una conexion. Esta preparado para conectarse

            try
            {
                //debo setear la consulta que quiero realizar
                datos.setearConsulta("Select Id, Descripcion From ELEMENTOS");

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Elemento aux = new Elemento();
                    aux.id = (int)datos.Lector["Id"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    lista.Add(aux);

                }

                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally {
                datos.cerrarConexion();
            }


       
        }
        //en este caso debo agrgar lo mismo que hice en pokemonNegocio,
        // para no hacer lo mismo, creo otra clase de acceso a datos

    }
}
