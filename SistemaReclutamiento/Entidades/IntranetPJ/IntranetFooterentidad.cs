using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.IntranetPJ
{
    public class IntranetFooterEntidad
    {
        public int foot_id { get; set; }
        public string foot_descripcion { get; set; }
        public string foot_estado { get; set; }
        public string foot_imagen { get; set; }
        public string foot_posicion { get; set; }
        public string ruta_anterior { get; set; }
    }
}