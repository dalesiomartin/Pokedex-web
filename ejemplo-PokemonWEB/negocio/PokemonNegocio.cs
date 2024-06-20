using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominio;

namespace negocio
{
    public class PokemonNegocio
    {
        //es la clase que creare los metodos de acceso a datos para los pokemon
        //TENER EN CUENTA, QUE CADA CLASE, DEBE TENER SU CLASE DE METODO DE ACCESO A DATOS

        //ESTO ES UN FUNCION PARA LISTAR POKEMON  public List<Pokemon> listar() { }
        public List<Pokemon> listar(string id = "") //le mando un parametro opcional
        {

            List<Pokemon> lista = new List<Pokemon>();

            //para crear el metodo a base de dato, necesito manejo excepc
            /*para establecer la conexcion necesito:
                # agregar libreria using System.Data.SqlClient
                # despues crear objetos, ej: SqlConnection
                # otro para realizar acciones SqlComand
                # tendre como resultado un set de datos y los voy a albergar en un lector
            + generado todos los objetos, luego debo configurarlos
                1) cadena de conexion + Base de datos + Forma de conexion
            en la forma de conexion, es a travez del motor sql server, se me conectara de otra ip, 
            integrated security=false; user=pepe; password=asdf1234
                2) comanto (sirve para realizar la accion): hay tres tipos (CommandType): texto, proced almacenado y enlace directo con la tabla 
            2.1= Texto (CommandText): es escribiendo la consulta de sql en el ambiente de programacion
            2.2= Comando conexion: que lo ejecute en este ambiente

                3) abro la conexion, realizo la lectura (executeReader)

                4) Lectura del lector, si pudo leer si hay conexion y me posiciona un puntero de la fila de la tabla
             */

            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector; //no genero instancia, porq cdo realice la lectura, obtendre un objeto

            //estructura ideal
            try
            {
                //1) cadena de conexion, copio la conexion de sql, la direccion del motor de base dato
                //en la direcc de entorno local, puedo usar LOCAL\\SQLEXPRESS; .\\SQLEXPRESS ; DESKTOPMTM\\SQLEXPRESS

                //conexion.ConnectionString = "server=DESKTOPMTM\\SQLEXPRESS;database=POKEDEX_DB;Integrated Security=false;user=martin;password=martin;"; //NO FUNCIONA
                //conexion.ConnectionString = "server=DESKTOPMTM\\SQLEXPRESS;database=POKEDEX_DB;Integrated Security=True;"; //NO FUNCIONA

                conexion.ConnectionString = "Data Source=DESKTOP-G8FBE6Q\\SQLEXPRESS;Initial Catalog=POKEDEX_DB;Integrated Security=True";
                /* LA FORMA DE COMPROBACION SEGURA ES
                 * 1- PROYECTO(windform-app1), PROPIEDADES
                 * 2- cn, cadena de conexion, aplicacion, tres puntos
                 * 3- cargar SERVIDOR, nombre de la Base de datos  ==> PROBAR CONEXION
                 * 4- genero un enlace. es el que pegue arriba entre comillas ""
                 */


                //2) comando
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "select Numero, Nombre, p.Descripcion, UrlImagen, e.Descripcion Tipo, d.Descripcion Debilidad, p.IdTipo, p.IdDebilidad, p.Id, p.Activo from POKEMONS p, ELEMENTOS e, ELEMENTOS d where p.IdTipo=e.Id  and p.IdDebilidad=d.Id  ";
                if (id != null)
                {
                    comando.CommandText += "and P.Id= " + id;
                }

                comando.Connection = conexion;

                //3) abro la conexion
                conexion.Open();
                lector = comando.ExecuteReader(); //realiza la lectura


                //4) lectura del lector
                while (lector.Read())
                {
                    Pokemon aux = new Pokemon();
                    aux.Id = (int)lector["Id"];
                    aux.Numero = lector.GetInt32(0); //el int32, ver tipo dato en la tabla y lleva cero por el orden
                    aux.Nombre = (string)lector["Nombre"];
                    aux.Descripcion = (string)lector["Descripcion"];

                    //HAY DOS FORMAS DE TRABAJAR EL NULL
                    //Primera:
                    // if(!(lector.IsDBNull(lector.GetOrdinal("UrlImagen"))))
                    //   aux.UrlImagen = (string)lector["UrlImagen"];
                    //Segunda opcion:
                    if (!(lector["UrlImagen"] is DBNull))
                        aux.UrlImagen = (string)lector["UrlImagen"];

                    //necesito modelar una clase Elemento, porque uni dos tablas
                    //ERROR COMUN: me falta crear una instancia, porque sino me dara referencia nula
                    aux.Tipo = new Elemento();
                    aux.Tipo.id = (int)lector["IdTipo"];
                    aux.Tipo.Descripcion = (string)lector["Tipo"];

                    aux.Debilidad = new Elemento(); //porque es un objeto de tipo elemento
                    aux.Debilidad.id = (int)lector["IdDebilidad"];
                    aux.Debilidad.Descripcion = (string)lector["Debilidad"];

                    aux.Activo = bool.Parse(lector["Activo"].ToString());


                    lista.Add(aux);
                }

                conexion.Close();
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }




        }


        //ESTO ES UNA FUNCION DE AGREGAR POKEMON, por ende debe conectarse a la BD
        public List<Pokemon> listarConSP()
        {
            List<Pokemon> lista = new List<Pokemon>(); // voy a usar la misma consulta de listar()
            AccesoDatos datos = new AccesoDatos();
            try
            {
                //a mi consulta que la traje directamente de listar, lo agrego un filtro o campo al where 
                //string consulta = "select Numero, Nombre, p.Descripcion, UrlImagen, e.Descripcion Tipo, d.Descripcion Debilidad, p.IdTipo, p.IdDebilidad, p.Id from POKEMONS p, ELEMENTOS e, ELEMENTOS d where p.IdTipo=e.Id  and p.IdDebilidad=d.Id and p.Activo=1 and ";
                //datos.setearConsulta(consulta);
                datos.setearProcedimiento("storedListar");

                datos.ejecutarLectura();

                //4) lectura del lector
                while (datos.Lector.Read())
                {
                    Pokemon aux = new Pokemon();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Numero = datos.Lector.GetInt32(0); //el int32, ver tipo dato en la tabla y lleva cero por el orden
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    if (!(datos.Lector["UrlImagen"] is DBNull))
                        aux.UrlImagen = (string)datos.Lector["UrlImagen"];


                    aux.Tipo = new Elemento();
                    aux.Tipo.id = (int)datos.Lector["IdTipo"];
                    aux.Tipo.Descripcion = (string)datos.Lector["Tipo"];

                    aux.Debilidad = new Elemento(); //porque es un objeto de tipo elemento
                    aux.Debilidad.id = (int)datos.Lector["IdDebilidad"];
                    aux.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];

                    aux.Activo = bool.Parse(datos.Lector["Activo"].ToString());

                    lista.Add(aux);
                }




                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }


        public void agregar(Pokemon nuevo)
        {   // aca necesito conectarme a la base de datos, mediante una clase de acceso a BD
            AccesoDatos datos = new AccesoDatos(); //con esto ya puedo conectarme
                                                   // NECESITO UNA LISTA PARA DEVOLVER?? NOOO, PORQUE NO DEVUELVE REGISTRO, LOS VA A INSERTAR

            try
            { //Debo setear la consulta
              //OJO, las "" son para C# y las '' son para SQL
              // datos.setearConsulta(" insert into POKEMONS (Numero,Nombre, Descripcion,Activo) values (" + nuevo.Numero + ",' " + nuevo.Nombre + "',' " + nuevo.Descripcion + "',1)");
                datos.setearConsulta(" insert into POKEMONS (Numero,Nombre, Descripcion,Activo, IdTipo, IdDebilidad, UrlImagen) values (" + nuevo.Numero + ",'" + nuevo.Nombre + "','" + nuevo.Descripcion + "',1, @IdTipo, @IdDebilidad, @UrlImagen)");
                //con @IdTipo, @IdDebilidad estoy creando una especie de variable pero se llaman PARAMETROS
                // Y se los debo agregar al comando, pero no puedo hacerlo directam porq tengo el metodo encapsulado
                // entonces voy a AccesoDatos, y creo un Public void setearParametros

                datos.setearParametro("@IdTipo", nuevo.Tipo.id);
                datos.setearParametro("@IdDebilidad", nuevo.Debilidad.id);
                //ahora la consulta cuenta con dos parametros
                datos.setearParametro("@UrlImagen", nuevo.UrlImagen);



                //AHORA DEBO EJECUTAR LA CONSULTA
                //NO PUEDO HACER  datos.ejecutarLectura; porque lo de arriba es un INSERT,
                //debo usar un ExecuteNonQuery(), ==> genero el metodo en accesoDatos 
                datos.ejecutarAccion();


                //con esto asi, cargo el pokemon pero no me lo mostrara el programa, al ser una consulta relacionada,
                // falla y no carga la linea en el programa, En SQL, lo veo con los null en los campos vacios
                //por el momento la aplic no admite null y falla. Por el momento para que funciones, a los campos que deben ir datos
                // le cargaremos datos vacio con ''.
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

        }

        public void agregarConSP(Pokemon nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearProcedimiento("storedAltaPokemon");

                datos.setearParametro("@numero", nuevo.Numero);
                datos.setearParametro("@nombre", nuevo.Nombre);
                datos.setearParametro("@desc", nuevo.Descripcion);
                datos.setearParametro("@img", nuevo.UrlImagen);
                datos.setearParametro("@idTipo", nuevo.Tipo.id);
                datos.setearParametro("@idDebilidad", nuevo.Debilidad.id);
                //datos.setearParametro("@idEvolucion", nuevo.Descripcion);

                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

        }

        public void modificar(Pokemon poke)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(" update POKEMONS set Numero=@numero, Nombre=@nombre, Descripcion=@desc, UrlImagen=@imag, IdTipo=@idTipo, IdDebilidad=@idDebilidad where id=@id");

                datos.setearParametro("@numero", poke.Numero);
                datos.setearParametro("@nombre", poke.Nombre);
                datos.setearParametro("@desc", poke.Descripcion);
                datos.setearParametro("@imag", poke.UrlImagen);
                datos.setearParametro("@idTipo", poke.Tipo.id);
                datos.setearParametro("@idDebilidad", poke.Debilidad.id);
                datos.setearParametro("@id", poke.Id);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void modificarConSP(Pokemon poke)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("storedModificarPokemon");

                datos.setearParametro("@numero", poke.Numero);
                datos.setearParametro("@nombre", poke.Nombre);
                datos.setearParametro("@desc", poke.Descripcion);
                datos.setearParametro("@imag", poke.UrlImagen);
                datos.setearParametro("@idTipo", poke.Tipo.id);
                datos.setearParametro("@idDebilidad", poke.Debilidad.id);
                datos.setearParametro("@id", poke.Id);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public void eliminar(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("delete from POKEMONS where id=@id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void eliminarLogico(int id, bool activo = false)
        {
            //en este caso no eliminare el registro, solo ACTUALIZARE el estado del mismo
            try
            {
                AccesoDatos datos = new AccesoDatos();

                datos.setearConsulta("update POKEMONS set Activo= @activo where id=@id;");
                datos.setearParametro("@id", id);
                datos.setearParametro("@activo", activo);
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<Pokemon> filtrar(string campo, string criterio, string filtro)
        {
            List<Pokemon> lista = new List<Pokemon>(); // voy a usar la misma consulta de listar()
            AccesoDatos datos = new AccesoDatos();
            try
            {
                //a mi consulta que la traje directamente de listar, lo agrego un filtro o campo al where 
                string consulta = "select Numero, Nombre, p.Descripcion, UrlImagen, e.Descripcion Tipo, d.Descripcion Debilidad, p.IdTipo, p.IdDebilidad, p.Id from POKEMONS p, ELEMENTOS e, ELEMENTOS d where p.IdTipo=e.Id  and p.IdDebilidad=d.Id and p.Activo=1 and ";

                if (campo == "Número")
                {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "Numero > " + filtro;
                            break;
                        case "Menor a":
                            consulta += "Numero < " + filtro;
                            break;
                        default: //case "Igual a":
                            consulta += "Numero = " + filtro;
                            break;
                    }
                }
                else if (campo == "Nombre")
                {

                    switch (criterio)
                    {
                        case "Empieza con":
                            consulta += "Nombre like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "Nombre like '%" + filtro + "'";
                            break;
                        default: //case "igual a":
                            consulta += "Nombre '%" + filtro + "%'";
                            break;
                    }

                }
                else
                {
                    switch (criterio)
                    {
                        case "Empieza con":
                            consulta += "p.Descripcion like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "p.Descripcion like '%" + filtro + "'";
                            break;
                        default: //case "igual a":
                            consulta += "p.Descripcion '%" + filtro + "%'";
                            break;
                    }

                }

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                //4) lectura del lector
                while (datos.Lector.Read())
                {
                    Pokemon aux = new Pokemon();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Numero = datos.Lector.GetInt32(0); //el int32, ver tipo dato en la tabla y lleva cero por el orden
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    if (!(datos.Lector["UrlImagen"] is DBNull))
                        aux.UrlImagen = (string)datos.Lector["UrlImagen"];


                    aux.Tipo = new Elemento();
                    aux.Tipo.id = (int)datos.Lector["IdTipo"];
                    aux.Tipo.Descripcion = (string)datos.Lector["Tipo"];

                    aux.Debilidad = new Elemento(); //porque es un objeto de tipo elemento
                    aux.Debilidad.id = (int)datos.Lector["IdDebilidad"];
                    aux.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];


                    lista.Add(aux);
                }




                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
    }
}

