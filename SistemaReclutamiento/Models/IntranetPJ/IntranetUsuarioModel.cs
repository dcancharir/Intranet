using Npgsql;
using SistemaReclutamiento.Entidades.IntranetPJ;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models.IntranetPJ
{
    public class IntranetUsuarioModel
    {
        string _conexion = "";
        public IntranetUsuarioModel() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (IntranetUsuarioEntidad usuario,claseError error) IntranetUsuarioValidarCredenciales(string usu_login)
        {
            claseError error = new claseError();
            IntranetUsuarioEntidad usuario = new IntranetUsuarioEntidad();
            string consulta = @"SELECT usu_id, lower(usu_nombre) as usu_nombre, usu_password, usu_tipo, usu_estado
	                        FROM
                            intranet.int_usuario
                            where lower(usu_nombre)=@p0;";
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
                                usuario.usu_password = ManejoNulos.ManageNullStr(dr["usu_password"]);
                                usuario.usu_tipo = ManejoNulos.ManageNullStr(dr["usu_tipo"]);
                                usuario.usu_estado = ManejoNulos.ManageNullStr(dr["usu_estado"]);
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
            return (usuario:usuario,error:error);
        }
    }
}