using Npgsql;
using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models.IntranetPJ
{
    public class IntranetAccesoModel
    {
        string _conexion = "";
        public IntranetAccesoModel() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        
        #region Region Acceso a Intranet
        public (UsuarioEntidad intranetUsuarioEncontrado, claseError error) UsuarioIntranetValidarCredenciales(string usu_login)
        {
            UsuarioEntidad usuario = new UsuarioEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT lower(usu_nombre) as usu_nombre, 
                                usu_contraseña, 
                                usu_estado,
                                usu_tipo,
                                usu_id,
                                fk_persona
                                FROM seguridad.seg_usuario
                                where usu_nombre = @p0
                               ; ";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", usu_login);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                usuario.usu_id = ManejoNulos.ManageNullInteger(dr["usu_id"]);
                                usuario.usu_nombre = ManejoNulos.ManageNullStr(dr["usu_nombre"]);
                                usuario.usu_contrasenia = ManejoNulos.ManageNullStr(dr["usu_contraseña"]);
                                usuario.usu_estado = ManejoNulos.ManageNullStr(dr["usu_estado"]);
                                usuario.fk_persona = ManejoNulos.ManageNullInteger(dr["fk_persona"]);
                                usuario.usu_tipo = ManejoNulos.ManageNullStr(dr["usu_tipo"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (intranetUsuarioEncontrado:usuario,error:error);
        }
        #endregion

        #region Region Acceso a SGC de Intranet

        public (UsuarioEntidad intranetUsuarioSGCEncontrado, claseError error) UsuarioIntranetSGCValidarCredenciales(string usu_login)
        {
            UsuarioEntidad usuario = new UsuarioEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT lower(usu_nombre) as usu_nombre, 
                                usu_contraseña, 
                                usu_estado,
                                usu_tipo,
                                usu_id,
                                fk_persona
                                FROM seguridad.seg_usuario
                                where usu_nombre = @p0
                               ; ";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", usu_login);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                usuario.usu_id = ManejoNulos.ManageNullInteger(dr["usu_id"]);
                                usuario.usu_nombre = ManejoNulos.ManageNullStr(dr["usu_nombre"]);
                                usuario.usu_contrasenia = ManejoNulos.ManageNullStr(dr["usu_contraseña"]);
                                usuario.usu_estado = ManejoNulos.ManageNullStr(dr["usu_estado"]);
                                usuario.fk_persona = ManejoNulos.ManageNullInteger(dr["fk_persona"]);
                                usuario.usu_tipo = ManejoNulos.ManageNullStr(dr["usu_tipo"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (intranetUsuarioSGCEncontrado:usuario,error:error);
        }
        #endregion
    }
}