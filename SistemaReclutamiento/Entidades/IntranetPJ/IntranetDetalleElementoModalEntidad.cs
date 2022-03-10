using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Entidades.IntranetPJ
{
    public class IntranetDetalleElementoModalEntidad
    {
        public int detelm_id { get; set; }
        [AllowHtml]
        public string detelm_descripcion { get; set; }
        public string detelm_nombre { get; set; }
        public string detelm_extension { get; set; }
        public string detelm_ubicacion { get; set; }
        public string detelm_estado { get; set; }
        public int fk_elemento_modal { get; set; }
        public int detelm_orden { get; set; }
        public string detelm_posicion { get; set; }
        public int fk_seccion_elemento { get; set; }
        public string detelm_nombre_imagen { get; set; }
        public string detelm_hash { get; set; }
    }
}