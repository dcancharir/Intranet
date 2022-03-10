using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.IntranetPJ
{
    public class IntranetElementoModalEntidad
    {
        public int emod_id { get; set; }
        public string emod_titulo { get; set; }
        public string emod_descripcion { get; set; }
        public string emod_contenido { get; set; }
        public int emod_orden { get; set; }
        public int fk_seccion_elemento { get; set; }
        public int fk_tipo_elemento { get; set; }
        public string emod_estado { get; set; }
        public string tipo_nombre { get; set; }
    }
}