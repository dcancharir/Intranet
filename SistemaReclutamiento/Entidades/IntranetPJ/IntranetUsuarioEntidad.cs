using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.IntranetPJ
{
    public class IntranetUsuarioEntidad
    {
        public int usu_id { get; set; }
        public string usu_nombre { get; set; }
        public string usu_password { get; set; }
        public string usu_estado { get; set; }
        public string usu_tipo { get; set; }
    }
}