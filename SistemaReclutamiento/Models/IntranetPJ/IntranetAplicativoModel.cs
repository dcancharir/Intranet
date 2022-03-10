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
    public class IntranetAplicativoModel
    {
        string _conexion;
        public IntranetAplicativoModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<IntranetAplicativoEntidad> intranetAplicativoLista, claseError error) IntranetAplicativoListarJson()
        {
            List<IntranetAplicativoEntidad> lista = new List<IntranetAplicativoEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT 
                                    apl_id, 
                                    apl_descripcion, 
                                    fk_imagen, 
                                    apl_estado, 
                                    apl_url, 
                                    apl_blank, 
                                    apl_tipo, 
	                                    FROM intranet.int_aplicativo";
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
                                var aplicativo = new IntranetAplicativoEntidad
                                {

                                    apl_id = ManejoNulos.ManageNullInteger(dr["apl_id"]),
                                    apl_descripcion = ManejoNulos.ManageNullStr(dr["apl_descripcion"]),
                                    fk_imagen = ManejoNulos.ManageNullInteger(dr["fk_imagen"]),
                                    apl_estado = ManejoNulos.ManageNullStr(dr["apl_estado"]),
                                    apl_url = ManejoNulos.ManageNullStr(dr["apl_url"]),
                                    apl_blank = ManejoNulos.ManegeNullBool(dr["apl_blank"]),
                                    apl_tipo = ManejoNulos.ManageNullStr(dr["apl_tipo"]),

                                };

                                lista.Add(aplicativo);
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
            return (intranetAplicativoLista: lista, error: error);
        }
        public (IntranetAplicativoEntidad intranetAplicativo, claseError error) IntranetAplicativoIdObtenerJson(int apl_id)
        {
            IntranetAplicativoEntidad intranetAplicativo = new IntranetAplicativoEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT 
                                    apl_id, 
                                    apl_descripcion, 
                                    fk_imagen, 
                                    apl_estado, 
                                    apl_url, 
                                    apl_blank, 
                                    apl_tipo, 
	                                    FROM intranet.int_aplicativo where apl_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", apl_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {

                                intranetAplicativo.apl_id = ManejoNulos.ManageNullInteger(dr["apl_id"]);
                                intranetAplicativo.apl_descripcion = ManejoNulos.ManageNullStr(dr["apl_descripcion"]);
                                intranetAplicativo.fk_imagen = ManejoNulos.ManageNullInteger(dr["fk_imagen"]);
                                intranetAplicativo.apl_estado = ManejoNulos.ManageNullStr(dr["apl_estado"]);
                                intranetAplicativo.apl_url = ManejoNulos.ManageNullStr(dr["apl_url"]);
                                intranetAplicativo.apl_blank = ManejoNulos.ManegeNullBool(dr["apl_blank"]);
                                intranetAplicativo.apl_tipo = ManejoNulos.ManageNullStr(dr["apl_tipo"]);
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
            return (intranetAplicativo: intranetAplicativo, error: error);
        }
        public (int idIntranetAplicativoInsertado, claseError error) IntranetAplicativoInsertarJson(IntranetAplicativoEntidad intranetAplicativo)
        {
            //bool response = false;
            int idIntranetAplicativoInsertado = 0;
            string consulta = @"
            INSERT INTO intranet.int_aplicativo(
	            apl_descripcion, fk_imagen, apl_url, apl_tipo, apl_estado, apl_blank)
	            VALUES (@p0, @p1, @p2, @p3, @p5, @p6)
                returning apl_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(intranetAplicativo.apl_descripcion));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(intranetAplicativo.fk_imagen));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(intranetAplicativo.apl_url));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(intranetAplicativo.apl_tipo));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(intranetAplicativo.apl_estado));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManegeNullBool(intranetAplicativo.apl_blank));
                    idIntranetAplicativoInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (idIntranetAplicativoInsertado: idIntranetAplicativoInsertado, error: error);
        }

        public (bool intranetAplicativoEditado, claseError error) IntranetAplicativoEditarJson(IntranetAplicativoEntidad intranetAplicativo)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE intranet.int_aplicativo
	        SET apl_descripcion=@p0, fk_imagen=@p1, apl_url=@p2, apl_tipo=@p3, apl_estado=@p5, apl_blank=@p6
	        WHERE apl_id=@p7;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(intranetAplicativo.apl_descripcion));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(intranetAplicativo.fk_imagen));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(intranetAplicativo.apl_url));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(intranetAplicativo.apl_tipo));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(intranetAplicativo.apl_estado));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManegeNullBool(intranetAplicativo.apl_blank));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(intranetAplicativo.apl_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (intranetAplicativoEditado: response, error: error);
        }
        public (bool intranetAplicativoEliminado, claseError error) IntranetAplicativoEliminarJson(int apl_id)
        {
            bool response = false;
            string consulta = @"DELETE FROM intranet.int_aplicativo
	                                WHERE apl_id=@p0;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();

                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(apl_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }

            return (intranetAplicativoEliminado: response, error: error);
        }
    }
}