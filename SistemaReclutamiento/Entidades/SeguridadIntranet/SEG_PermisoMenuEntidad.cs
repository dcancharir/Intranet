using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.SeguridadIntranet
{
    public class SEG_PermisoMenuEntidad
    {
        public int WEB_PMeID { get; set; }
        public string WEB_PMeNombre { get; set; }
        public string WEB_PMeEstado { get; set; }
        public DateTime WEB_PMeFechaRegistro { get; set; }
        public string WEB_PMeDataMenu { get; set; }
        public int WEB_RolID { get; set; }
        public string WEB_ModuloNombre { get; set; }
    }
}