using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.IntranetPJ
{
    public class IntranetActividadesEntidad
    {
        public int act_id { get; set; }
        public string act_descripcion { get; set; }
        public string act_imagen { get; set; }
        public DateTime act_fecha { get; set; }
        public string act_estado { get; set; }
        //imagen
        public string img_ubicacion { get; set; }
 
    }
}