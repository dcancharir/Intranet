using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class UbigeoEntidad
    {
        public int ubi_id { get; set; }
        public string ubi_pais_id { get; set; }
        public string ubi_departamento_id { get; set; }
        public string ubi_provincia_id { get; set; }
        public string ubi_distrito_id { get; set; }
        public string ubi_nombre { get; set; }
        public string ubi_estado { get; set; }
    }
}