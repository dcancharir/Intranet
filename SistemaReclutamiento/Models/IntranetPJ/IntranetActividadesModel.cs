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
    public class IntranetActividadesModel
    {
        string _conexion;
        public IntranetActividadesModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<IntranetActividadesEntidad> intranetActividadesLista, claseError error) IntranetActividadesListarJson()
        {
            List<IntranetActividadesEntidad> lista = new List<IntranetActividadesEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT act.act_id, act.act_descripcion, act.act_imagen, 
                                 act.act_estado, act.act_fecha
	                                FROM intranet.int_actividades as act 
	                                where extract(day from act.act_fecha)>=extract(day from (select current_date))
	                                and extract(month from act.act_fecha)=extract(month from(select current_date))
	                                and act.act_estado='A'
                                    order by act.act_fecha asc
	                                limit 8
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
                                var actividades = new IntranetActividadesEntidad
                                {

                                    act_id = ManejoNulos.ManageNullInteger(dr["act_id"]),
                                    act_descripcion = ManejoNulos.ManageNullStr(dr["act_descripcion"]),
                                    act_imagen = ManejoNulos.ManageNullStr(dr["act_imagen"]),
                                    act_fecha = ManejoNulos.ManageNullDate(dr["act_fecha"]),
                                    act_estado = ManejoNulos.ManageNullStr(dr["act_estado"]),
                                };

                                lista.Add(actividades);
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
            return (intranetActividadesLista: lista, error: error);
        }
        public (IntranetActividadesEntidad intranetActividades, claseError error) IntranetActividadesIdObtenerJson(int apl_id)
        {
            IntranetActividadesEntidad intranetActividades = new IntranetActividadesEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT act_id, act_descripcion, act_imagen, act_fecha, act_estado
	                                FROM intranet.int_actividades
                                     where act_id=@p0;";
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

                                intranetActividades.act_id = ManejoNulos.ManageNullInteger(dr["act_id"]);
                                intranetActividades.act_descripcion = ManejoNulos.ManageNullStr(dr["act_descripcion"]);
                                intranetActividades.act_imagen = ManejoNulos.ManageNullStr(dr["act_imagen"]);
                                intranetActividades.act_fecha = ManejoNulos.ManageNullDate(dr["act_fecha"]);
                                intranetActividades.act_estado = ManejoNulos.ManageNullStr(dr["act_estado"]);
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
            return (intranetActividades: intranetActividades, error: error);
        }
        public (int idIntranetActividadesInsertado, claseError error) IntranetActividadesInsertarJson(IntranetActividadesEntidad intranetActividades)
        {
            //bool response = false;
            int idIntranetActividadesInsertado = 0;
            string consulta = @"
            INSERT INTO intranet.int_actividades(
	                             act_descripcion, act_imagen, act_fecha, act_estado)
	                            VALUES (@p0, @p1, @p2, @p4)
                                returning act_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(intranetActividades.act_descripcion));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(intranetActividades.act_imagen));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(intranetActividades.act_fecha));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(intranetActividades.act_estado));
                    idIntranetActividadesInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (idIntranetActividadesInsertado: idIntranetActividadesInsertado, error: error);
        }

        public (bool intranetActividadesEditado, claseError error) IntranetActividadesEditarJson(IntranetActividadesEntidad intranetActividades)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE intranet.int_actividades
	                            SET act_descripcion=@p0, act_imagen=@p1, act_fecha=@p2,  act_estado=@p4
	                            WHERE act_id=@p5;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(intranetActividades.act_descripcion));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(intranetActividades.act_imagen));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(intranetActividades.act_fecha));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(intranetActividades.act_estado));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullInteger(intranetActividades.act_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (intranetActividadesEditado: response, error: error);
        }
        public (bool intranetActividadesEliminado, claseError error) IntranetActividadesEliminarJson(int act_id)
        {
            bool response = false;
            string consulta = @"DELETE FROM intranet.int_actividades
	                                WHERE act_id=@p0;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();

                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(act_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }

            return (intranetActividadesEliminado: response, error: error);
        }
        public (List<IntranetActividadesEntidad> intranetActividadesLista, claseError error) IntranetActividadesListarTodoJson() {
            List<IntranetActividadesEntidad> lista = new List<IntranetActividadesEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT act.act_id, act.act_descripcion, act.act_imagen, 
                                 act.act_estado, act.act_fecha
	                                FROM intranet.int_actividades as act 
                                    order by act.act_fecha desc";
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
                                var actividades = new IntranetActividadesEntidad
                                {

                                    act_id = ManejoNulos.ManageNullInteger(dr["act_id"]),
                                    act_descripcion = ManejoNulos.ManageNullStr(dr["act_descripcion"]),
                                    act_imagen = ManejoNulos.ManageNullStr(dr["act_imagen"]),
                                    act_fecha = ManejoNulos.ManageNullDate(dr["act_fecha"]),
                                    act_estado = ManejoNulos.ManageNullStr(dr["act_estado"]),
                                };

                                lista.Add(actividades);
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
            return (intranetActividadesLista: lista, error: error);
        }
    }
}