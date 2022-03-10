using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.SeguridadIntranet
{
    public class SEG_RolEntidad
    {
        public int WEB_RolID { get; set; }
        public string WEB_RolNombre { get; set; }
        public string WEB_RolDescripcion { get; set; }
        public string WEB_RolEstado { get; set; }
        public DateTime WEB_RolFechaRegistro { get; set; }
    }
}