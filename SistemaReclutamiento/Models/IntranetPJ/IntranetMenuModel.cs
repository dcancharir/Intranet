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
    public class IntranetMenuModel
    {
        string _conexion;
        public IntranetMenuModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<IntranetMenuEntidad> intranetMenuLista, claseError error) IntranetMenuListarJson()
        {
            List<IntranetMenuEntidad> lista = new List<IntranetMenuEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT 
                                menu_id, 
                                menu_titulo, 
                                menu_url, 
                                menu_estado, 
                                menu_orden, 
                                menu_blank
	                            FROM intranet.int_menu
                                where menu_estado='A'
                                 order by menu_orden;";
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
                                var menu = new IntranetMenuEntidad
                                {

                                    menu_id = ManejoNulos.ManageNullInteger(dr["menu_id"]),
                                    menu_titulo = ManejoNulos.ManageNullStr(dr["menu_titulo"]),
                                    menu_url = ManejoNulos.ManageNullStr(dr["menu_url"]),
                                    menu_estado = ManejoNulos.ManageNullStr(dr["menu_estado"]),
                                    menu_orden = ManejoNulos.ManageNullInteger(dr["menu_orden"]),
                                    menu_blank = ManejoNulos.ManegeNullBool(dr["menu_blank"])

                                };

                                lista.Add(menu);
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
            return (intranetMenuLista: lista, error: error);
        }

        public (List<IntranetMenuEntidad> intranetMenuLista, claseError error) IntranetMenuListarTodoJson()
        {
            List<IntranetMenuEntidad> lista = new List<IntranetMenuEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT 
                                menu_id, 
                                menu_titulo, 
                                menu_url, 
                                menu_estado, 
                                menu_orden, 
                                menu_blank
	                            FROM intranet.int_menu
                                 order by menu_orden;";
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
                                var menu = new IntranetMenuEntidad
                                {

                                    menu_id = ManejoNulos.ManageNullInteger(dr["menu_id"]),
                                    menu_titulo = ManejoNulos.ManageNullStr(dr["menu_titulo"]),
                                    menu_url = ManejoNulos.ManageNullStr(dr["menu_url"]),
                                    menu_estado = ManejoNulos.ManageNullStr(dr["menu_estado"]),
                                    menu_orden = ManejoNulos.ManageNullInteger(dr["menu_orden"]),
                                    menu_blank = ManejoNulos.ManegeNullBool(dr["menu_blank"])
                                };

                                lista.Add(menu);
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
            return (intranetMenuLista: lista, error: error);
        }
        public (IntranetMenuEntidad intranetMenu, claseError error) IntranetMenuIdObtenerJson(int menu_id)
        {
            IntranetMenuEntidad intranetMenu = new IntranetMenuEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT 
                                menu_id, 
                                menu_titulo, 
                                menu_url, 
                                menu_estado, 
                                menu_orden, 
                                menu_blank
	                            FROM intranet.int_menu
                                where menu_id=@p0;";
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

                                intranetMenu.menu_id = ManejoNulos.ManageNullInteger(dr["menu_id"]);
                                intranetMenu.menu_titulo = ManejoNulos.ManageNullStr(dr["menu_titulo"]);
                                intranetMenu.menu_url = ManejoNulos.ManageNullStr(dr["menu_url"]);
                                intranetMenu.menu_estado = ManejoNulos.ManageNullStr(dr["menu_estado"]);
                                intranetMenu.menu_orden = ManejoNulos.ManageNullInteger(dr["menu_orden"]);
                                intranetMenu.menu_blank = ManejoNulos.ManegeNullBool(dr["menu_blank"]);
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
            return (intranetMenu: intranetMenu, error: error);
        }
        public (int idIntranetMenuInsertado, claseError error) IntranetMenuInsertarJson(IntranetMenuEntidad intranetMenu)
        {
            //bool response = false;
            int idIntranetMenuInsertado = 0;
            string consulta = @"
            INSERT INTO intranet.int_menu(menu_titulo, menu_url, menu_orden, menu_estado, menu_blank)
	            VALUES (@p0, @p1, @p2, @p3, @p4)
                returning menu_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(intranetMenu.menu_titulo));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(intranetMenu.menu_url));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(intranetMenu.menu_orden));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(intranetMenu.menu_estado));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManegeNullBool(intranetMenu.menu_blank));
                    idIntranetMenuInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (idIntranetMenuInsertado: idIntranetMenuInsertado, error: error);
        }
        public (bool intranetMenuEditado, claseError error) IntranetMenuEditarJson(IntranetMenuEntidad intranetMenu)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE intranet.int_menu
	                            SET menu_titulo=@p1, menu_url=@p2, menu_estado=@p3, menu_blank=@p4
	                            WHERE menu_id=@p5;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(intranetMenu.menu_titulo));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(intranetMenu.menu_url));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(intranetMenu.menu_estado));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManegeNullBool(intranetMenu.menu_blank));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullInteger(intranetMenu.menu_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (intranetMenuEditado: response, error: error);
        }
        public (bool intranetMenuEliminado, claseError error) IntranetMenuEliminarJson(int menu_id)
        {
            bool response = false;
            string consulta = @"DELETE FROM intranet.int_menu
	                                WHERE menu_id=@p0;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();

                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(menu_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }

            return (intranetMenuEliminado: response, error: error);
        }
        public (int intranetMenuTotal, claseError error) IntranetMenuObtenerTotalRegistrosJson() {
            int intranetMenuTotal = 0;
            claseError error = new claseError();
            string consulta = @"select count(*) as total from intranet.int_menu";
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
                                intranetMenuTotal = ManejoNulos.ManageNullInteger(dr["total"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (intranetMenuTotal: intranetMenuTotal, error: error);
        }
        public (bool intranetMenuReordenado, claseError error) IntranetMenuEditarOrdenJson(IntranetMenuEntidad intranetMenu) {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE intranet.int_menu
	                            SET menu_orden=@p0
	                            WHERE menu_id=@p1;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(intranetMenu.menu_orden));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(intranetMenu.menu_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (intranetMenuReordenado: response, error: error);
        }
    }
}