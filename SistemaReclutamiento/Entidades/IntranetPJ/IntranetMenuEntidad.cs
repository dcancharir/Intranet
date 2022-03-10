using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.IntranetPJ
{
    public class IntranetMenuEntidad
    {
        public int menu_id { get; set; }
        public string menu_titulo { get; set; }
        public string menu_url { get; set; }
        public string menu_estado { get; set; }
        public int menu_orden { get; set; }
        public bool menu_blank { get; set; }
    }
}