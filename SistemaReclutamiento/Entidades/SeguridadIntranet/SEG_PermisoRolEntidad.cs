using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.SeguridadIntranet
{
    public class SEG_PermisoRolEntidad
    {
        public int WEB_PRolID { get; set; }
        public int WEB_PermID { get; set; }
        public int WEB_RolID { get; set; }
        public DateTime WEB_PRolFechaRegistro { get; set; }
        public string WEB_PermControlador { get; set; }
        public string WEB_PermNombre { get; set; }
        public string WEB_PermNombreR { get; set; }
        public string WEB_PermDescripcion { get; set; }
    }
}