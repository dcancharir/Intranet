using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Utilitarios;
//using System.Data.SqlClient;
using Npgsql;
using System.Diagnostics;
using System.Collections;

namespace SistemaReclutamiento.Models
{
    public class PersonaModel
    {
        string _conexion;
        public PersonaModel() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
     
        public PersonaEntidad PersonaIdObtenerJson(int per_id)
        {
            PersonaEntidad persona = new PersonaEntidad();
            string consulta = @"SELECT 
                                    per_nombre, 
                                    per_apellido_pat, 
                                    per_direccion, 
                                    per_fechanacimiento, 
                                    per_correoelectronico, 
                                    per_tipo, 
                                    per_estado, 
                                    per_id, 
                                    per_apellido_mat, 
                                    per_telefono, 
                                    per_celular, 
                                    per_tipodoc, 
                                    per_numdoc, 
                                    fk_ubigeo, 
                                    per_sexo, 
                                    per_fecha_reg, 
                                    per_fecha_act, 
                                    fk_cargo, 
                                    per_foto
	                                    FROM marketing.cpj_persona
                                            where per_id=@p0;";         
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", per_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                persona.per_nombre = ManejoNulos.ManageNullStr(dr["per_nombre"]);
                                persona.per_apellido_pat = ManejoNulos.ManageNullStr(dr["per_apellido_pat"]);
                                persona.per_direccion = ManejoNulos.ManageNullStr(dr["per_direccion"]);
                                persona.per_fechanacimiento = ManejoNulos.ManageNullDate(dr["per_fechanacimiento"]);
                                persona.per_correoelectronico = ManejoNulos.ManageNullStr(dr["per_correoelectronico"]);
                                persona.per_tipo = ManejoNulos.ManageNullStr(dr["per_tipo"]);
                                persona.per_estado = ManejoNulos.ManageNullStr(dr["per_estado"]);
                                persona.per_id = ManejoNulos.ManageNullInteger(dr["per_id"]);
                                persona.per_apellido_mat = ManejoNulos.ManageNullStr(dr["per_apellido_mat"]);
                                persona.per_telefono = ManejoNulos.ManageNullStr(dr["per_telefono"]);
                                persona.per_celular = ManejoNulos.ManageNullStr(dr["per_celular"]);
                                persona.per_tipodoc = ManejoNulos.ManageNullStr(dr["per_tipodoc"]);
                                persona.per_numdoc = ManejoNulos.ManageNullStr(dr["per_numdoc"]);
                                persona.fk_ubigeo = ManejoNulos.ManageNullInteger(dr["fk_ubigeo"]);
                                persona.per_sexo = ManejoNulos.ManageNullStr(dr["per_sexo"]);
                                persona.per_fecha_reg = ManejoNulos.ManageNullDate(dr["per_fecha_reg"]);
                                persona.per_fecha_act = ManejoNulos.ManageNullDate(dr["per_fecha_act"]); 
                                persona.fk_cargo = ManejoNulos.ManageNullInteger(dr["fk_cargo"]);
                                persona.per_foto = ManejoNulos.ManageNullStr(dr["per_foto"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return persona;
        }
        public (List<PersonaEntidad> listaPersonas, claseError error) PersonaListarEmpleadosJson() {
            List<PersonaEntidad> listaPersonas = new List<PersonaEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT per_nombre, per_apellido_pat, per_direccion, per_fechanacimiento,
                                per_correoelectronico, per_tipo, per_estado, per_id, per_apellido_mat, 
                                per_telefono, per_celular, per_tipodoc, per_numdoc, 
                                fk_ubigeo, per_sexo, per_fecha_reg, per_fecha_act, fk_cargo, per_foto
	                            FROM marketing.cpj_persona
                                where per_tipo='EMPLEADO';";
            try
            {
                using (var con = new NpgsqlConnection(_conexion)) {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    using (var dr = query.ExecuteReader()) {
                        if (dr.HasRows) {
                            while (dr.Read()) {
                                var Persona = new PersonaEntidad
                                {
                                    per_nombre = ManejoNulos.ManageNullStr(dr["per_nombre"]),
                                    per_apellido_pat = ManejoNulos.ManageNullStr(dr["per_apellido_pat"]),
                                    per_direccion = ManejoNulos.ManageNullStr(dr["per_direccion"]),
                                    per_fechanacimiento = ManejoNulos.ManageNullDate(dr["per_fechanacimiento"]),
                                    per_correoelectronico = ManejoNulos.ManageNullStr(dr["per_correoelectronico"]),
                                    per_tipo = ManejoNulos.ManageNullStr(dr["per_tipo"]),
                                    per_estado = ManejoNulos.ManageNullStr(dr["per_estado"]),
                                    per_id = ManejoNulos.ManageNullInteger(dr["per_id"]),
                                    per_apellido_mat = ManejoNulos.ManageNullStr(dr["per_apellido_mat"]),
                                    per_telefono = ManejoNulos.ManageNullStr(dr["per_telefono"]),
                                    per_celular = ManejoNulos.ManageNullStr(dr["per_celular"]),
                                    per_tipodoc = ManejoNulos.ManageNullStr(dr["per_tipodoc"]),
                                    per_numdoc = ManejoNulos.ManageNullStr(dr["per_numdoc"]),
                                    fk_ubigeo = ManejoNulos.ManageNullInteger(dr["fk_ubigeo"]),
                                    per_sexo = ManejoNulos.ManageNullStr(dr["per_sexo"]),
                                    per_fecha_reg = ManejoNulos.ManageNullDate(dr["per_fecha_reg"]),
                                    per_fecha_act = ManejoNulos.ManageNullDate(dr["per_fecha_act"]),
                                    fk_cargo = ManejoNulos.ManageNullInteger(dr["fk_cargo"]),
                                    per_foto = ManejoNulos.ManageNullStr(dr["per_foto"]),
                                };
                                listaPersonas.Add(Persona);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (listaPersonas: listaPersonas, error: error);
        }
    }
}