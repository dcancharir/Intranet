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
    public class IntranetSeccionElementoModel
    {
        string _conexion;
        public IntranetSeccionElementoModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<IntranetSeccionElementoEntidad> intranetSeccionElementoLista, claseError error) IntranetSeccionElementoListarJson()
        {
            List<IntranetSeccionElementoEntidad> lista = new List<IntranetSeccionElementoEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT sele_id, sele_orden, sele_estado
	                                FROM intranet.int_seccion_elemento
                                        order by sele_orden;";
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
                                var SeccionElemento = new IntranetSeccionElementoEntidad
                                {

                                    sele_id = ManejoNulos.ManageNullInteger(dr["sele_id"]),
                                    sele_orden = ManejoNulos.ManageNullInteger(dr["sele_orden"]),
                                    sele_estado = ManejoNulos.ManageNullStr(dr["sele_estado"]),
                                };

                                lista.Add(SeccionElemento);
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
            return (intranetSeccionElementoLista: lista, error: error);
        }
        public (IntranetSeccionElementoEntidad intranetSeccionElemento, claseError error) IntranetSeccionElementoIdObtenerJson(int sele_id)
        {
            IntranetSeccionElementoEntidad intranetSeccionElemento = new IntranetSeccionElementoEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT sele_id, sele_orden, sele_estado
	                                FROM intranet.int_seccion_elemento where sele_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", sele_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                intranetSeccionElemento.sele_id = ManejoNulos.ManageNullInteger(dr["sele_id"]);
                                intranetSeccionElemento.sele_orden = ManejoNulos.ManageNullInteger(dr["sele_orden"]);
                                intranetSeccionElemento.sele_estado = ManejoNulos.ManageNullStr(dr["sele_estado"]);
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
            return (intranetSeccionElemento: intranetSeccionElemento, error: error);
        }
        public (int idIntranetSeccionElementoInsertado, claseError error) IntranetSeccionElementoInsertarJson(IntranetSeccionElementoEntidad intranetSeccionElemento)
        {
            //bool response = false;
            int idIntranetSeccionElementoInsertado = 0;
            string consulta = @"INSERT INTO intranet.int_seccion_elemento(
	                            sele_orden, sele_estado)
	                            VALUES (@p0, @p1)
                                returning sele_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(intranetSeccionElemento.sele_orden));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(intranetSeccionElemento.sele_estado));
                    idIntranetSeccionElementoInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (idIntranetSeccionElementoInsertado: idIntranetSeccionElementoInsertado, error: error);
        }

        public (bool intranetSeccionElementoEditado, claseError error) IntranetSeccionElementoEditarJson(IntranetSeccionElementoEntidad intranetSeccionElemento)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE intranet.int_seccion_elemento
	                            SET  sele_orden=@p0, sele_estado=@p1
	                            WHERE sele_id=@p3;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(intranetSeccionElemento.sele_orden));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(intranetSeccionElemento.sele_estado));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(intranetSeccionElemento.sele_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (intranetSeccionElementoEditado: response, error: error);
        }
        public (bool intranetSeccionElementoEliminado, claseError error) IntranetSeccionElementoEliminarJson(int sele_id)
        {
            bool response = false;
            string consulta = @"DELETE FROM intranet.int_seccion_elemento
	                                WHERE sele_id=@p0;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();

                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(sele_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }

            return (intranetSeccionElementoEliminado: response, error: error);
        }
    }
}