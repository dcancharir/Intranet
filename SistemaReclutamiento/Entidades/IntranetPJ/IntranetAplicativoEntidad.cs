using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.IntranetPJ
{
    public class IntranetAplicativoEntidad
    {
        public int apl_id { get; set; }
        public string apl_descripcion { get; set; }
        public int fk_imagen { get; set; }
        public string apl_estado { get; set; }
        public string apl_url { get; set; }
        public bool apl_blank { get; set; }
        public string apl_tipo { get; set; }
    }
}