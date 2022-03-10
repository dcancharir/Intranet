using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.SeguridadIntranet
{
    public class SEG_RolUsuarioEntidad
    {
        public int WEB_RUsuID { get; set; }
        public int WEB_RolID { get; set; }
        public int UsuarioID { get; set; }
        public DateTime WEB_RUsuFechaRegistro { get; set; }
    }
}