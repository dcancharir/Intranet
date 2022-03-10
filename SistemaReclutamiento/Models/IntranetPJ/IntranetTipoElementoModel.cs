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
    public class IntranetTipoElementoModel
    {
        string _conexion;
        public IntranetTipoElementoModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<IntranetTipoElementoEntidad> intranetTipoElementoLista, claseError error) IntranetTipoElementoListarJson()
        {
            List<IntranetTipoElementoEntidad> lista = new List<IntranetTipoElementoEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT tipo_id, tipo_nombre, tipo_descripcion, tipo_estado,tipo_orden
	                    FROM intranet.int_tipo_elemento
                                 order by tipo_id;";
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
                                var TipoElemento = new IntranetTipoElementoEntidad
                                {

                                    tipo_id = ManejoNulos.ManageNullInteger(dr["tipo_id"]),
                                    tipo_descripcion = ManejoNulos.ManageNullStr(dr["tipo_descripcion"]),
                                    tipo_estado = ManejoNulos.ManageNullStr(dr["tipo_estado"]),
                                    tipo_nombre = ManejoNulos.ManageNullStr(dr["tipo_nombre"]),
                                    tipo_orden = ManejoNulos.ManageNullInteger(dr["tipo_orden"]),

                                };

                                lista.Add(TipoElemento);
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
            return (intranetTipoElementoLista: lista, error: error);
        }
        public (IntranetTipoElementoEntidad intranetTipoElemento, claseError error) IntranetTipoElementoIdObtenerJson(int tipo_id)
        {
            IntranetTipoElementoEntidad intranetTipoElemento = new IntranetTipoElementoEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT tipo_id, tipo_nombre, tipo_descripcion, tipo_estado, tipo_orden
	                    FROM intranet.int_tipo_elemento
                                where tipo_id=@p0
                            order by tipo_id;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", tipo_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                intranetTipoElemento.tipo_id = ManejoNulos.ManageNullInteger(dr["tipo_id"]);
                                intranetTipoElemento.tipo_nombre = ManejoNulos.ManageNullStr(dr["tipo_nombre"]);
                                intranetTipoElemento.tipo_descripcion = ManejoNulos.ManageNullStr(dr["tipo_descripcion"]);
                                intranetTipoElemento.tipo_estado = ManejoNulos.ManageNullStr(dr["tipo_estado"]);
                                intranetTipoElemento.tipo_orden = ManejoNulos.ManageNullInteger(dr["tipo_orden"]);
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
            return (intranetTipoElemento: intranetTipoElemento, error: error);
        }
        public (int idIntranetTipoElementoInsertado, claseError error) IntranetTipoElementoInsertarJson(IntranetTipoElementoEntidad intranetTipoElemento)
        {
            //bool response = false;
            int idIntranetTipoElementoInsertado = 0;
            string consulta = @"
            INSERT INTO intranet.int_TipoElemento(tipo_nombre, tipo_descripcion, tipo_estado,tipo_orden)
	            VALUES (@p0, @p1, @p2,@p3)
                returning tipo_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(intranetTipoElemento.tipo_nombre));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(intranetTipoElemento.tipo_descripcion));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(intranetTipoElemento.tipo_estado));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(intranetTipoElemento.tipo_orden));
                    idIntranetTipoElementoInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (idIntranetTipoElementoInsertado: idIntranetTipoElementoInsertado, error: error);
        }
        public (bool intranetTipoElementoEditado, claseError error) IntranetTipoElementoEditarJson(IntranetTipoElementoEntidad intranetTipoElemento)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE intranet.int_tipo_elemento
	                            SET tipo_nombre=@p1,tipo_descripcion=@p2, tipo_estado=@p3
	                            WHERE tipo_id=@p4;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(intranetTipoElemento.tipo_nombre));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(intranetTipoElemento.tipo_descripcion));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(intranetTipoElemento.tipo_estado));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(intranetTipoElemento.tipo_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (intranetTipoElementoEditado: response, error: error);
        }
        public (bool intranetTipoElementoEliminado, claseError error) IntranetTipoElementoEliminarJson(int tipo_id)
        {
            bool response = false;
            string consulta = @"DELETE FROM intranet.int_tipo_elemento
	                                WHERE tipo_id=@p0;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();

                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(tipo_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }

            return (intranetTipoElementoEliminado: response, error: error);
        }
    }
}