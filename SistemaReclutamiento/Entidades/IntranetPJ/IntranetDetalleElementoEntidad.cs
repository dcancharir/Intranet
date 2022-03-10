using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Entidades.IntranetPJ
{
    public class IntranetDetalleElementoEntidad
    {
        public int detel_id { get; set; }
        [AllowHtml]
        public string detel_descripcion { get; set; }
        public string detel_nombre { get; set; }
        public string detel_extension { get; set; }
        public string detel_ubicacion { get; set; }
        public string detel_estado { get; set; }
        public int fk_elemento { get; set; }
        public int detel_orden { get; set; }
        public string detel_posicion { get; set; }
        public int fk_seccion_elemento { get; set; }
        public string detel_nombre_imagen { get; set; }
        public string detel_url { get; set; }
        public bool detel_blank { get; set; }
        public int fk_tipo_elemento { get; set; }
        public string detel_hash { get; set; }
    }
}