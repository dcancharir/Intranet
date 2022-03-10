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
    public class IntranetSeccionModel
    {
        string _conexion;
        public IntranetSeccionModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<IntranetSeccionEntidad> intranetSeccionLista, claseError error) IntranetSeccionListarJson()
        {
            List<IntranetSeccionEntidad> lista = new List<IntranetSeccionEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT sec_id, sec_orden, sec_estado, fk_menu
	                                FROM intranet.int_seccion
                                        order by sec_orden;";
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
                                var Seccion = new IntranetSeccionEntidad
                                {

                                    sec_id = ManejoNulos.ManageNullInteger(dr["sec_id"]),
                                    sec_orden = ManejoNulos.ManageNullInteger(dr["sec_orden"]),
                                    sec_estado = ManejoNulos.ManageNullStr(dr["sec_estado"]),
                                    fk_menu = ManejoNulos.ManageNullInteger(dr["fk_menu"]),
                                };

                                lista.Add(Seccion);
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
            return (intranetSeccionLista: lista, error: error);
        }

        public (List<IntranetSeccionEntidad> intranetSeccionListaxMenuID, claseError error) IntranetSeccionListarxMenuIDJson(int menu_id)
        {
            List<IntranetSeccionEntidad> lista = new List<IntranetSeccionEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT sec_id, sec_orden, sec_estado, fk_menu
	                                FROM intranet.int_seccion
                                    where fk_menu=@p0
                                        order by sec_orden;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", menu_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var Seccion = new IntranetSeccionEntidad
                                {

                                    sec_id = ManejoNulos.ManageNullInteger(dr["sec_id"]),
                                    sec_orden = ManejoNulos.ManageNullInteger(dr["sec_orden"]),
                                    sec_estado = ManejoNulos.ManageNullStr(dr["sec_estado"]),
                                    fk_menu = ManejoNulos.ManageNullInteger(dr["fk_menu"]),
                                };

                                lista.Add(Seccion);
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
            return (intranetSeccionListaxMenuID: lista, error: error);
        }

        public (List<IntranetSeccionEntidad> intranetSeccionListaTodoxMenuID, claseError error) IntranetSeccionListarTodoxMenuIDJson(int menu_id)
        {
            List<IntranetSeccionEntidad> lista = new List<IntranetSeccionEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT sec_id, sec_orden, sec_estado, fk_menu, int_menu.menu_titulo
	                                FROM intranet.int_seccion join intranet.int_menu
									on intranet.int_seccion.fk_menu=intranet.int_menu.menu_id
                                    where fk_menu=@p0
                                        order by sec_orden;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", menu_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var Seccion = new IntranetSeccionEntidad
                                {

                                    sec_id = ManejoNulos.ManageNullInteger(dr["sec_id"]),
                                    sec_orden = ManejoNulos.ManageNullInteger(dr["sec_orden"]),
                                    sec_estado = ManejoNulos.ManageNullStr(dr["sec_estado"]),
                                    fk_menu = ManejoNulos.ManageNullInteger(dr["fk_menu"]),
                                    menu_titulo=ManejoNulos.ManageNullStr(dr["menu_titulo"]),
                                };

                                lista.Add(Seccion);
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
            return (intranetSeccionListaTodoxMenuID: lista, error: error);
        }
        public (IntranetSeccionEntidad intranetSeccion, claseError error) IntranetSeccionIdObtenerJson(int sec_id)
        {
            IntranetSeccionEntidad intranetSeccion = new IntranetSeccionEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT sec_id, sec_orden, sec_estado, fk_menu
	                                FROM intranet.int_seccion where sec_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", sec_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                intranetSeccion.sec_id = ManejoNulos.ManageNullInteger(dr["sec_id"]);
                                intranetSeccion.sec_orden = ManejoNulos.ManageNullInteger(dr["sec_orden"]);
                                intranetSeccion.sec_estado = ManejoNulos.ManageNullStr(dr["sec_estado"]);
                                intranetSeccion.fk_menu = ManejoNulos.ManageNullInteger(dr["fk_menu"]);
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
            return (intranetSeccion: intranetSeccion, error: error);
        }
        public (int idIntranetSeccionInsertado, claseError error) IntranetSeccionInsertarJson(IntranetSeccionEntidad intranetSeccion)
        {
            //bool response = false;
            int idIntranetSeccionInsertado = 0;
            string consulta = @"
                                INSERT INTO intranet.int_seccion(
	                            sec_orden, sec_estado, fk_menu)
	                            VALUES ( @p0, @p1, @p2)
                                returning sec_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(intranetSeccion.sec_orden));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(intranetSeccion.sec_estado));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(intranetSeccion.fk_menu));
                    idIntranetSeccionInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (idIntranetSeccionInsertado: idIntranetSeccionInsertado, error: error);
        }

        public (bool intranetSeccionEditado, claseError error) IntranetSeccionEditarJson(IntranetSeccionEntidad intranetSeccion)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE UPDATE intranet.int_seccion
	                        SET  sec_orden=@p0, sec_estado=@p1, fk_menu=@p2
	                        WHERE sec_id=@p3;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(intranetSeccion.sec_orden));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(intranetSeccion.sec_estado));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(intranetSeccion.fk_menu));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(intranetSeccion.sec_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (intranetSeccionEditado: response, error: error);
        }

        public (bool intranetSeccionEditado, claseError error) IntranetSeccionEditarEstadoJson(IntranetSeccionEntidad intranetSeccion)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE intranet.int_seccion
	                            SET sec_estado=@p0
	                            WHERE sec_id=@p1;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(intranetSeccion.sec_estado));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(intranetSeccion.sec_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (intranetSeccionEditado: response, error: error);
        }
        public (bool intranetSeccionEliminado, claseError error) IntranetSeccionEliminarJson(int sec_id)
        {
            bool response = false;
            string consulta = @"DELETE FROM intranet.int_seccion
	                                WHERE sec_id=@p0;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();

                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(sec_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }

            return (intranetSeccionEliminado: response, error: error);
        }
        public (bool intranetSeccionReordenado, claseError error) IntranetSeccionEditarOrdenJson(IntranetSeccionEntidad intranetSeccion)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE intranet.int_seccion
	                            SET sec_orden=@p0
	                            WHERE sec_id=@p1;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(intranetSeccion.sec_orden));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(intranetSeccion.sec_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (intranetSeccionReordenado: response, error: error);
        }
        public (int intranetSeccionTotal, claseError error) IntranetSeccionObtenerTotalRegistrosJson(int fk_menu)
        {
            int intranetSeccionTotal = 0;
            claseError error = new claseError();
            string consulta = @"select count(*) as total from intranet.int_seccion where fk_menu=@p0";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(fk_menu));
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                intranetSeccionTotal = ManejoNulos.ManageNullInteger(dr["total"]);
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
            return (intranetSeccionTotal: intranetSeccionTotal, error: error);
        }
    }
}