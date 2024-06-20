using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio //viene con namespace winform_app1 y se lo cambiamos a dominio
{
    public class Pokemon
    {

        public int Id { get; set; }
        //con DisplayName: puedo agregar tildes o espacios a los campos de la tabla
        [DisplayName("Número")]
        public int Numero { get; set; }
        public string Nombre { get; set; }
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        public string UrlImagen { get; set; }

        public Elemento Tipo { get; set; } //tipo pokemento es un tipo elemento

        public Elemento Debilidad { get; set; }

        public bool Activo { get; set; }


        //para conectarme a la base de dato, necesito otra clase
    }
}
