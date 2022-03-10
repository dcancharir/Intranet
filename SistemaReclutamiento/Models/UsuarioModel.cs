using System;
using System.Collections.Generic;
using System.Configuration;
//using System.Data.SqlClient;
using Npgsql;
using System.Linq;
using System.Web;
using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Utilitarios;

namespace SistemaReclutamiento.Models
{
    public class UsuarioModel
    {
        string _conexion = string.Empty;
        public UsuarioModel() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public UsuarioEntidad UsuarioObtenerxID(int id)
        {
            UsuarioEntidad usuario = new UsuarioEntidad();
            string consulta = @"SELECT usu_id,usu_nombre,usu_estado,usu_clave_temp,fk_persona, usu_token,usu_exp_token 
	                            FROM seguridad.seg_usuario where usu_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                usuario.usu_id = ManejoNulos.ManageNullInteger(dr["usu_id"]);
                                usuario.usu_nombre = ManejoNulos.ManageNullStr(dr["usu_nombre"]);
                                //usuario.usu_contrasenia = ManejoNulos.ManageNullStr(dr["usu_contraseña"]);
                                usuario.usu_estado = ManejoNulos.ManageNullStr(dr["usu_estado"]);
                                usuario.usu_clave_temp = ManejoNulos.ManageNullStr(dr["usu_clave_temp"]);
                                usuario.fk_persona = ManejoNulos.ManageNullInteger(dr["fk_persona"]);
                                usuario.usu_token = ManejoNulos.ManageNullStr(dr["usu_token"]);
                                usuario.usu_exp_token = ManejoNulos.ManageNullStr(dr["usu_exp_token"]);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return usuario;
        }

        #region UsuarioItranet
        public (List<UsuarioPersonaEntidad> listaUsuarios, claseError error) IntranetListarUsuariosTokenJson()
        {
            List<UsuarioPersonaEntidad> listaUsuarios = new List<UsuarioPersonaEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT usu_nombre,
                                per_id, usu_id, usu_estado,
                                usu_token, usu_exp_token,
                                per_nombre,
                                per_apellido_pat, per_apellido_mat, per_numdoc
	                                FROM marketing.cpj_persona
	                                join seguridad.seg_usuario
	                                on marketing.cpj_persona.per_id=seguridad.seg_usuario.fk_persona
	                                where per_tipo='EMPLEADO' order by per_apellido_pat;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion)) {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    using (var dr = query.ExecuteReader()) {
                        if (dr.HasRows) {
                            while (dr.Read()) {
                                var usuario = new UsuarioPersonaEntidad()
                                {
                                    usu_nombre = ManejoNulos.ManageNullStr(dr["usu_nombre"]),
                                    usu_estado = ManejoNulos.ManageNullStr(dr["usu_estado"]),
                                    per_apellido_mat = ManejoNulos.ManageNullStr(dr["per_apellido_mat"]),
                                    per_nombre = ManejoNulos.ManageNullStr(dr["per_nombre"]),
                                    per_numdoc = ManejoNulos.ManageNullStr(dr["per_numdoc"]),
                                    per_apellido_pat = ManejoNulos.ManageNullStr(dr["per_apellido_pat"]),
                                    usu_token = ManejoNulos.ManageNullStr(dr["usu_token"]),
                                    usu_exp_token = ManejoNulos.ManageNullDate(dr["usu_exp_token"]),
                                    per_id = ManejoNulos.ManageNullInteger(dr["per_id"]),
                                    usu_id = ManejoNulos.ManageNullInteger(dr["usu_id"]),
                                };
                                listaUsuarios.Add(usuario);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex) {
                error.Respuesta = false;
                error.Mensaje = Ex.Message;
            }
            return (listaUsuarios: listaUsuarios, error: error);
        }
        public (List<UsuarioPersonaEntidad> listaUsuarios, claseError error) IntranetListarUsuariosJson()
        {
            List<UsuarioPersonaEntidad> listaUsuarios = new List<UsuarioPersonaEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT usu_nombre,
		                        per_id, usu_id,
		                        per_nombre,
		                        per_apellido_pat, per_apellido_mat
			                        FROM marketing.cpj_persona
			                        join seguridad.seg_usuario
			                        on marketing.cpj_persona.per_id=seguridad.seg_usuario.fk_persona
			                        where usu_tipo='EMPLEADO';";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var usuario = new UsuarioPersonaEntidad()
                                {
                                    usu_nombre = ManejoNulos.ManageNullStr(dr["usu_nombre"]),
                                    per_apellido_mat = ManejoNulos.ManageNullStr(dr["per_apellido_mat"]),
                                    per_nombre = ManejoNulos.ManageNullStr(dr["per_nombre"]),
                                    per_apellido_pat = ManejoNulos.ManageNullStr(dr["per_apellido_pat"]),
                                    per_id = ManejoNulos.ManageNullInteger(dr["per_id"]),
                                    usu_id = ManejoNulos.ManageNullInteger(dr["usu_id"]),
                                };
                                listaUsuarios.Add(usuario);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                error.Respuesta = false;
                error.Mensaje = Ex.Message;
            }
            return (listaUsuarios: listaUsuarios, error: error);
        }
        #endregion
    }
}