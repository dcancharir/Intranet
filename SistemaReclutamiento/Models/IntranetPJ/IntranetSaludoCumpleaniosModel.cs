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
    public class IntranetSaludoCumpleaniosModel
    {
        string _conexion;
        public IntranetSaludoCumpleaniosModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<IntranetSaludoCumpleanioEntidad> intranetSaludoCumpleanioLista, claseError error) IntranetSaludoCumpleanioListarJson()
        {
            List<IntranetSaludoCumpleanioEntidad> lista = new List<IntranetSaludoCumpleanioEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT sal.sld_id, sal.sld_cuerpo, sal.sld_estado, sal.sld_fecha_envio, 
		                        per.per_nombre as per_saluda, per.per_apellido_pat as apelpat_per_saluda, per.per_apellido_mat as apelmat_per_saluda,
		                        per2.per_nombre as per_saludada, per2.per_apellido_pat as apelpat_per_saludada,per2.per_apellido_mat as apelmat_per_saludada
	                            FROM intranet.int_saludos_cumpleanio as sal join marketing.cpj_persona as per
	                            on sal.fk_persona_que_saluda=per.per_id
	                            join marketing.cpj_persona  as per2
	                            on sal.fk_persona_saludada=per2.per_id
	                            order by sld_fecha_envio desc
	                            ;";
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
                                var SaludoCumpleanio = new IntranetSaludoCumpleanioEntidad
                                {

                                    sld_id = ManejoNulos.ManageNullInteger(dr["sld_id"]),
                                    sld_cuerpo = ManejoNulos.ManageNullStr(dr["sld_cuerpo"]),
                                    sld_estado = ManejoNulos.ManageNullStr(dr["sld_estado"]),
                                    sld_fecha_envio = ManejoNulos.ManageNullDate(dr["sld_fecha_envio"]),
                                    per_saluda = ManejoNulos.ManageNullStr(dr["per_saluda"]),
                                    apelpat_per_saluda = ManejoNulos.ManageNullStr(dr["apelpat_per_saluda"]),
                                    apelmat_per_saluda = ManejoNulos.ManageNullStr(dr["apelmat_per_saluda"]),
                                    per_saludada = ManejoNulos.ManageNullStr(dr["per_saludada"]),
                                    apelpat_per_saludada = ManejoNulos.ManageNullStr(dr["apelpat_per_saludada"]),
                                    apelmat_per_saludada = ManejoNulos.ManageNullStr(dr["apelmat_per_saludada"]),

                                };

                                lista.Add(SaludoCumpleanio);
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
            return (intranetSaludoCumpleanioLista: lista, error: error);
        }

        public (List<IntranetSaludoCumpleanioEntidad> intranetSaludoCumpleanioLista, claseError error) IntranetSaludoCumpleanioActivosListarJson()
        {
            List<IntranetSaludoCumpleanioEntidad> lista = new List<IntranetSaludoCumpleanioEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT sal.sld_id, sal.sld_cuerpo, sal.sld_estado, sal.sld_fecha_envio,sal.sld_avatar, 
		                        per.per_nombre as per_saluda, per.per_apellido_pat as apelpat_per_saluda, per.per_apellido_mat as apelmat_per_saluda,
		                        per2.per_nombre as per_saludada, per2.per_apellido_pat as apelpat_per_saludada,per2.per_apellido_mat as apelmat_per_saludada
	                            FROM intranet.int_saludos_cumpleanio as sal join marketing.cpj_persona as per
	                            on sal.fk_persona_que_saluda=per.per_id
	                            join marketing.cpj_persona  as per2
	                            on sal.fk_persona_saludada=per2.per_id
                                where sal.sld_estado='A'
	                            order by sld_fecha_envio desc;";
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
                                var SaludoCumpleanio = new IntranetSaludoCumpleanioEntidad
                                {

                                    sld_id = ManejoNulos.ManageNullInteger(dr["sld_id"]),
                                    sld_cuerpo = ManejoNulos.ManageNullStr(dr["sld_cuerpo"]),
                                    sld_estado = ManejoNulos.ManageNullStr(dr["sld_estado"]),
                                    sld_fecha_envio = ManejoNulos.ManageNullDate(dr["sld_fecha_envio"]),
                                    per_saluda = ManejoNulos.ManageNullStr(dr["per_saluda"]),
                                    apelpat_per_saluda = ManejoNulos.ManageNullStr(dr["apelpat_per_saluda"]),
                                    apelmat_per_saluda = ManejoNulos.ManageNullStr(dr["apelmat_per_saluda"]),
                                    per_saludada = ManejoNulos.ManageNullStr(dr["per_saludada"]),
                                    apelpat_per_saludada = ManejoNulos.ManageNullStr(dr["apelpat_per_saludada"]),
                                    apelmat_per_saludada = ManejoNulos.ManageNullStr(dr["apelmat_per_saludada"]),
                                    sld_avatar = ManejoNulos.ManageNullStr(dr["sld_avatar"]),
                                };

                                lista.Add(SaludoCumpleanio);
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
            return (intranetSaludoCumpleanioLista: lista, error: error);
        }
        public (IntranetSaludoCumpleanioEntidad intranetSaludoCumpleanio, claseError error) IntranetSaludoCumpleanioIdObtenerJson(int sld_id)
        {
            IntranetSaludoCumpleanioEntidad intranetSaludoCumpleanio = new IntranetSaludoCumpleanioEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT sld_id, sld_cuerpo, sld_estado, sld_fecha_envio, fk_persona
	                            FROM intranet.int_saludos_cumpleanio where sld_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", sld_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {

                                intranetSaludoCumpleanio.sld_id = ManejoNulos.ManageNullInteger(dr["sld_id"]);
                                intranetSaludoCumpleanio.sld_cuerpo = ManejoNulos.ManageNullStr(dr["sld_cuerpo"]);
                                intranetSaludoCumpleanio.sld_estado = ManejoNulos.ManageNullStr(dr["sld_estado"]);
                                intranetSaludoCumpleanio.sld_fecha_envio = ManejoNulos.ManageNullDate(dr["sld_fecha_envio"]);
                                intranetSaludoCumpleanio.per_saluda = ManejoNulos.ManageNullStr(dr["per_saluda"]);
                                intranetSaludoCumpleanio.apelpat_per_saluda = ManejoNulos.ManageNullStr(dr["apelpat_per_saluda"]);
                                intranetSaludoCumpleanio.apelmat_per_saluda = ManejoNulos.ManageNullStr(dr["apelmat_per_saluda"]);
                                intranetSaludoCumpleanio.per_saludada = ManejoNulos.ManageNullStr(dr["per_saludada"]);
                                intranetSaludoCumpleanio.apelpat_per_saludada = ManejoNulos.ManageNullStr(dr["apelpat_per_saludada"]);
                                intranetSaludoCumpleanio.apelmat_per_saludada = ManejoNulos.ManageNullStr(dr["apelmat_per_saludada"]);
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
            return (intranetSaludoCumpleanio: intranetSaludoCumpleanio, error: error);
        }
        public (int idIntranetSaludoCumpleanioInsertado, claseError error) IntranetSaludoCumpleanioInsertarJson(IntranetSaludoCumpleanioEntidad intranetSaludoCumpleanio)
        {
            //bool response = false;
            int idIntranetSaludoCumpleanioInsertado = 0;
            string consulta = @"
                            INSERT INTO intranet.int_saludos_cumpleanio(
	                        sld_cuerpo, sld_estado, sld_fecha_envio, fk_persona_que_saluda,fk_persona_saludada,sld_avatar)
	                        VALUES ( @p0, @p1, @p2, @p3,@p4,@p5)
                            returning sld_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(intranetSaludoCumpleanio.sld_cuerpo));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(intranetSaludoCumpleanio.sld_estado));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(intranetSaludoCumpleanio.sld_fecha_envio));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(intranetSaludoCumpleanio.fk_persona_que_saluda));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(intranetSaludoCumpleanio.fk_persona_saludada));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(intranetSaludoCumpleanio.sld_avatar));
                    idIntranetSaludoCumpleanioInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (idIntranetSaludoCumpleanioInsertado: idIntranetSaludoCumpleanioInsertado, error: error);
        }
        public (bool intranetSaludoCumpleanioEditado, claseError error) IntranetSaludoCumpleanioEditarJson(IntranetSaludoCumpleanioEntidad intranetSaludoCumpleanio)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE intranet.int_saludos_cumpleanio
	                            SET sld_estado=@p0
	                            WHERE sld_id=@p1;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(intranetSaludoCumpleanio.sld_estado));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(intranetSaludoCumpleanio.sld_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (intranetSaludoCumpleanioEditado: response, error: error);
        }
        public (bool intranetSaludoCumpleanioEliminado, claseError error) IntranetSaludoCumpleanioEliminarJson(int sld_id)
        {
            bool response = false;
            string consulta = @"DELETE FROM intranet.int_saludos_cumpleanio
	                                WHERE sld_id=@p0;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();

                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(sld_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }

            return (intranetSaludoCumpleanioEliminado: response, error: error);
        }
    }
}