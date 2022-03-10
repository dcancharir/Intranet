using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.IntranetPJ
{
    public class IntranetTipoElementoEntidad
    {
        public int tipo_id { get; set; }
        public string tipo_nombre { get; set; }
        public string tipo_descripcion { get; set; }
        public string tipo_estado { get; set; }
        public int tipo_orden { get; set; }

    }
}