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
    public class IntranetSistemasModel
    {
        string _conexion;
        public IntranetSistemasModel() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<IntranetSistemasEntidad> intranetSistemasLista, claseError error) IntranetSistemasListarJson()
        {
            List<IntranetSistemasEntidad> lista = new List<IntranetSistemasEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT sist_id, sist_nombre, 
                                sist_ruta, sist_descripcion, sist_estado
	                            FROM intranet.int_sistemas;";
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
                                var sistema = new IntranetSistemasEntidad
                                {

                                    sist_id = ManejoNulos.ManageNullInteger(dr["sist_id"]),
                                    sist_nombre = ManejoNulos.ManageNullStr(dr["sist_nombre"]),
                                    sist_ruta = ManejoNulos.ManageNullStr(dr["sist_ruta"]),
                                    sist_descripcion = ManejoNulos.ManageNullStr(dr["sist_descripcion"]),
                                    sist_estado = ManejoNulos.ManageNullStr(dr["sist_estado"]),

                                };

                                lista.Add(sistema);
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
            return (intranetSistemasLista: lista, error: error);
        }
        public (IntranetSistemasEntidad intranetSistema, claseError error) IntranetSistemaIdObtenerJson(int sist_id)
        {
            IntranetSistemasEntidad intranetSistema = new IntranetSistemasEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT sist_id, sist_nombre, 
                                sist_ruta, sist_descripcion, sist_estado
	                            FROM intranet.int_sistemas
                                where sist_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", sist_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {

                                intranetSistema.sist_id = ManejoNulos.ManageNullInteger(dr["sist_id"]);
                                intranetSistema.sist_nombre = ManejoNulos.ManageNullStr(dr["sist_nombre"]);
                                intranetSistema.sist_ruta = ManejoNulos.ManageNullStr(dr["sist_ruta"]);
                                intranetSistema.sist_descripcion = ManejoNulos.ManageNullStr(dr["sist_descripcion"]);
                                intranetSistema.sist_estado = ManejoNulos.ManageNullStr(dr["sist_estado"]);
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
            return (intranetSistema: intranetSistema, error: error);
        }
        public (int idIntranetSistemaInsertado, claseError error) IntranetSistemaInsertarJson(IntranetSistemasEntidad intranetSistema)
        {
            //bool response = false;
            int idIntranetSistemaInsertado = 0;
            string consulta = @"
            INSERT INTO intranet.int_sistemas(
	                            sist_nombre, sist_ruta, sist_descripcion, sist_estado)
	                            VALUES (@p0, @p1, @p2, @p3)
                returning sist_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(intranetSistema.sist_nombre));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(intranetSistema.sist_ruta));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(intranetSistema.sist_descripcion));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(intranetSistema.sist_estado));
                    idIntranetSistemaInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (idIntranetSistemaInsertado: idIntranetSistemaInsertado, error: error);
        }
        public (bool intranetSistemaEditado, claseError error) IntranetSistemaEditarJson(IntranetSistemasEntidad intranetSistema)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE intranet.int_sistemas
	                                    SET sist_nombre=@p0, sist_ruta=@p1, sist_descripcion=@p2, sist_estado=@p3
	                                    WHERE sist_id=@p4;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(intranetSistema.sist_nombre));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(intranetSistema.sist_ruta));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(intranetSistema.sist_descripcion));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(intranetSistema.sist_estado));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(intranetSistema.sist_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (intranetSistemaEditado: response, error: error);
        }
        public (bool intranetSistemaEliminado, claseError error) IntranetMenuEliminarJson(int sist_id)
        {
            bool response = false;
            string consulta = @"DELETE FROM intranet.int_sistemas
	                                WHERE sist_id=@p0;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();

                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(sist_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }

            return (intranetSistemaEliminado: response, error: error);
        }

    }
}