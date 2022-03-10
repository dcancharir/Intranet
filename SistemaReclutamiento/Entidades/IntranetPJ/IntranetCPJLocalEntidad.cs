using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.IntranetPJ
{
    public class IntranetCPJLocalEntidad
    {
        public string loc_nombre { get; set; }
        public string loc_estado { get; set; }
        public int loc_id { get; set; }
        public DateTime loc_fecha_reg { get; set; }
        public DateTime loc_fecha_asct { get; set; }
        public int fk_usuario { get; set; }
        public int fk_ubigeo { get; set; }
        public string loc_url { get; set; }
        public string loc_img { get; set; }
        public string loc_alias { get; set; }
        public int fk_empresa { get; set; }
        public string loc_direccion { get; set; }
        public string loc_correo { get; set; }
        public double loc_latitud { get; set; }
        public double loc_longitud { get; set; }
        public string loc_host { get; set; }
        public string loc_enable { get; set; }
        public string loc_puerto { get; set; }
        public string loc_autentica { get; set; }
        public string loc_user { get; set; }
        public string loc_password { get; set; }
        public int loc_cod_s3000 { get; set; }
        public string loc_tipo { get; set; }
        public string ubi_nombre { get;set; }
    }
}