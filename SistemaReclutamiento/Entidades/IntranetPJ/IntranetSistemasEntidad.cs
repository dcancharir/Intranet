using SistemaReclutamiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.IntranetPJ
{
    public class IntranetSistemasEntidad
    {
        public int sist_id { get; set; }
        public string sist_nombre { get; set; }
        public string sist_ruta { get; set; }
        public string sist_descripcion { get; set; }
        public string sist_estado { get; set; }
        public List<SEG_Usuario> usuarios { get; set; }
    }
}