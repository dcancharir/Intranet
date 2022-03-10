using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.IntranetPJ
{
    public class IntranetElementoEntidad
    {
        public int elem_id { get; set; }
        public string elem_titulo{ get; set; }
        public string elem_descripcion { get; set; }
        public string elem_contenido { get; set; }
        public int elem_orden { get; set; }
        public string elem_estado { get; set; }
        public int fk_seccion { get; set; }
        public int fk_tipo_elemento { get; set; }
        public string tipo_nombre { get; set; }
    }
}