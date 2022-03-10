using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.IntranetPJ
{
    public class IntranetSaludoCumpleanioEntidad
    {
        public int sld_id { get; set; }
        public string sld_cuerpo { get; set; }
        public string sld_estado { get; set; }
        public DateTime sld_fecha_envio { get; set; }
        public int fk_persona_que_saluda { get; set; }
        public int fk_persona_saludada { get; set; }
        public string sld_avatar { get; set; }
        public string direccion_envio { get; set; }
        //nombres
        public string per_saluda{ get; set; }
        public string apelpat_per_saluda { get; set; }
        public string apelmat_per_saluda { get; set; }
        public string per_saludada { get; set; }
        public string apelpat_per_saludada { get; set; }
        public string apelmat_per_saludada { get; set; }
        
    }
}