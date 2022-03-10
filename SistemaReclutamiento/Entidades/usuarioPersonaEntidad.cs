using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class UsuarioPersonaEntidad
    {
        public int usu_id { get; set; }
        public string usu_token { get; set; }
        public DateTime usu_exp_token { get; set; }
        public int per_id { get; set; }
        public string usu_nombre { get; set; }
        public string usu_contrasenia { get; set; }
        public string usu_estado { get; set; }
        public string per_tipodoc { get; set; }
        public string usu_tipo { get; set; }
        public string per_numdoc { get; set; }
        public string per_nombre { get; set; }
        public string per_apellido_pat { get; set; }
        public string per_apellido_mat { get; set; }
        public string per_correoelectronico { get; set; }
        public DateTime per_fechanacimiento { get; set; }
        public string per_telefono { get; set; }
        public string per_celular { get; set; }
        public string per_sexo { get; set; }
        //Datos de Ubigeo
        public string ubi_departamento_id { get; set; }
        public string ubi_distrito_id { get; set; }
        public string ubi_provincia_id { get; set; }
        public string ubi_pais_id { get; set; }
        //Datos de Postulante
        public int pos_id { get; set; }
        public string pos_condicion_viv { get; set; }
        public string pos_direccion { get; set; }
        public string pos_tipo_calle { get; set; }
        public string pos_numero_casa { get; set; }
        public string pos_tipo_casa { get; set; }
        public string pos_celular { get; set; }
        public string pos_estado_civil { get; set; }
        public bool pos_brevete { get; set; }
        public string pos_num_brevete { get; set; }
        public bool pos_referido { get; set; }
        public string pos_nombre_referido { get; set; }
        public string pos_cv { get; set; }
        public string pos_foto { get; set; }
        public string pos_situacion { get; set; }
        public DateTime pos_fecha_reg { get; set; }
        public DateTime pos_fecha_act { get; set; }
        public string pos_estado { get; set; }
        public string pos_url_perfil { get; set; }
        public string fk_nacionalidad { get; set; }
        public bool pos_familia_amigos { get; set; }
        public string pos_fam_ami_desc { get; set; }
        public bool pos_trabajo_pj { get; set; }
        public string pos_trab_pj_desc { get; set; }
        public string pos_trab_otra_empresa {get;set;}
        //En que BD se encuentra el postulante en el formulario de Registro
        public string busqueda { get; set; }
    }
}