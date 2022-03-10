using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class PersonaEntidad
    {
        public string per_nombre { get; set; }
        public string per_apellido_pat { get; set; }
        public string per_direccion { get; set; }
        public DateTime per_fechanacimiento { get; set; }
        public string per_correoelectronico { get; set; }
        public string per_tipo { get; set; }
        public string per_estado { get; set; }
        public int per_id { get; set; }
        public string per_apellido_mat { get; set; }
        public string per_telefono { get; set; }    
        public string per_celular { get; set; }
        public string per_tipodoc { get; set; }
        public string per_numdoc { get; set; }
        public int fk_ubigeo { get; set; }   
        public string per_sexo { get; set; }
        public DateTime per_fecha_reg { get; set; }
        public DateTime per_fecha_act { get; set; }
        public int fk_cargo { get; set; }
        public string per_foto { get; set; }
        //Datos de ubigeo
        public string ubi_id { get; set; }
        public string ubi_pais_id { get; set; }
        public string ubi_departamento_id { get; set; }
        public string ubi_provincia_id { get; set; }
        public string ubi_distrito_id { get; set; }

    }
}