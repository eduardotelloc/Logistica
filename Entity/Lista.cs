using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Lista
    {
        public string id { get; set; }
        public string name { get; set; }
        public string mesa { get; set; }
        public string nombre { get; set; }
        public string codigo { get; set; }
        public string titulo { get; set; }
        public string texto { get; set; }
        public string imagen { get; set; }

    }


    public class ListaReporte
    {
        public string name { get; set; }
        public decimal amount { get; set; }

    }
    
}
