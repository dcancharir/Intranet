using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.IntranetPJ
{
    public class IntranetSeccionEntidad
    {
        public int sec_id { get; set; }
        public int sec_orden { get; set; }
        public string sec_estado { get; set; }
        public int fk_menu { get; set; }
        //variables para menu
        public string menu_titulo { get; set; }
    }
}