using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio //viene con namespace winform_app1 y se lo cambiamos a dominio
{
    public class Elemento
    {

        public int id { get; set; }
        public string Descripcion { get; set; }

        public override string ToString() // esto para modificar el metodo tostring
        {
            return Descripcion; //para que me traiga la descripcion
        }
    }
}
