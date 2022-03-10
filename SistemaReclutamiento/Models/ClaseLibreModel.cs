using SistemaReclutamiento.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models
{
    public class ClaseLibreModel
    {
    }
    public class ReporteOfertaLaboral
    {
        public string ola_nombre { get; set; }
        public string ola_cod_empresa { get; set; }
        public string ola_cod_cargo { get; set; }
        public int fk_ubigeo { get; set; }
        public string busqueda { get; set; }
        public string ubi_pais_id { get; set; }
        public string ubi_departamento_id { get; set; }
        public string ubi_provincia_id { get; set; }
        public string ubi_distrito_id { get; set; }
        public string ola_rango_fecha { get; set; }
        public DateTime ola_fecha_ini { get; set; }
        public int pos_id { get; set; }
        public UbigeoEntidad ubigeo { get; set; }
        public ReporteOfertaLaboral()
        {
            this.ubigeo = new UbigeoEntidad();
        }
    }
    public class postulacionEntidad
    {
        public int ppo_id { get; set; }
        public string ppo_tipo_direcion { get; set; }
        public string ppo_direccion { get; set; }
        public string ppo_tipo_calle { get; set; }
        public string ppo_numero_casa { get; set; }
        public string ppo_tipo_casa { get; set; }
        public string ppo_celular { get; set; }
        public string ppo_estado_civil { get; set; }
        public bool ppo_brevete { get; set; }
        public string ppo_num_brevete { get; set; }
        public bool ppo_referido { get; set; }
        public string ppo_nombre_referido { get; set; }
        public string ppo_cv { get; set; }
        public string ppo_foto { get; set; }
        public string ppo_situacion { get; set; }
        public DateTime ppo_fecha_reg { get; set; }
        public DateTime ppo_fecha_act { get; set; }
        public string ppo_estado { get; set; }
        public int fk_postulante { get; set; }
        public int fk_oferta_laboral { get; set; }
    }
    public class claseError {
        public bool Respuesta { get; set; }
        public string Mensaje { get; set; }
        public claseError() {
            Respuesta = true;
            Mensaje = string.Empty;
        }
    }
    public class TMEMPR {
        public string CO_EMPR { get; set; }
        public string DE_NOMB { get; set; }
        public string DE_NOMB_CORT { get; set; }
        public string NO_DEPA { get; set; }
        public string NO_PROV { get; set; }
        public string NU_RUCS { get; set; }
        public string NO_REPR_LEGA { get; set; }
        public string NO_PAIS { get; set; }
    }
    public class TTPUES_TRAB {
        public string CO_EMPR { get; set; }
        public string CO_PUES_TRAB { get; set; }
        public string DE_PUES_TRAB { get; set; }
    }
    public class SEG_Usuario
    {
        public string NombreEmpleado { get; set; }
        public int UsuarioID { get; set; }
        public int EmpleadoID { get; set; }
        public string UsuarioNombre { get; set; }
        public string Estado { get; set; }
        public string Token { get; set; }
        public string DOI { get; set; }
        public string TokenPostgres { get; set; }
    }
    public class TTSEDE{
        public string CO_EMPR { get; set; }
        public string DE_NOMB { get; set; }
        public string CO_SEDE { get; set; }
        public string DE_SEDE { get; set; }
        public string ST_DOMI_FISC { get; set; }
        public string TI_SEDE_RTPS { get; set; }
        public string NU_RUCS_SEDE { get; set; }
        public string NO_DIRE_SEDE { get; set; }
        public string NU_CASA { get; set; }
        public string NU_INTE { get; set; }
        public string NO_ZONA { get; set; }
    }

}