using Npgsql;
using SistemaReclutamiento.Entidades.SeguridadIntranet;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models.SeguridadIntranet
{
    public class SEG_PermisoMenuDAL
    {
        string _conexion = string.Empty;
        public SEG_PermisoMenuDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public (bool respuesta,claseError error) GuardarPermisoMenu(SEG_PermisoMenuEntidad permisoMENU)
        {
            claseError error = new claseError();
            bool respuesta = false;
            string consulta = @"INSERT INTO intranet.seg_permisomenu
           (WEB_PMeNombre,WEB_PMeFechaRegistro,WEB_PMeDataMenu,WEB_RolID,WEB_PMeEstado,WEB_ModuloNombre)VALUES(@p0,@p1,@p2,@p3,@p4,@p5)";

            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(permisoMENU.WEB_PMeNombre));
                    query.Parameters.AddWithValue("@p1", DateTime.Now);
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(permisoMENU.WEB_PMeDataMenu));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(permisoMENU.WEB_RolID));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(permisoMENU.WEB_PMeEstado));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(permisoMENU.WEB_ModuloNombre));
                    query.ExecuteNonQuery();

                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }

            return (respuesta,error);
        }

        public (List<SEG_PermisoMenuEntidad> lista,claseError error) GetPermisoMenu()
        {
            claseError error = new claseError();
            List<SEG_PermisoMenuEntidad> lista = new List<SEG_PermisoMenuEntidad>();
            string consulta = @"SELECT WEB_PMeID
                                  ,WEB_PMeNombre
                                  ,WEB_PMeFechaRegistro
                                  ,WEB_PMeDataMenu
                                  ,WEB_RolID
                              FROM intranet.seg_permisomenu";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var webPermisoMenu = new SEG_PermisoMenuEntidad
                            {
                                WEB_PMeID = ManejoNulos.ManageNullInteger(dr["WEB_PMeID"]),
                                WEB_PMeNombre = ManejoNulos.ManageNullStr(dr["WEB_PMeNombre"]),
                                WEB_RolID = ManejoNulos.ManageNullInteger(dr["WEB_RolID"]),
                                WEB_PMeFechaRegistro = ManejoNulos.ManageNullDate(dr["WEB_PMeFechaRegistro"]),
                                WEB_PMeDataMenu = ManejoNulos.ManageNullStr(dr["WEB_PMeDataMenu"])
                            };

                            lista.Add(webPermisoMenu);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }

            return (lista,error);
        }

        public (SEG_PermisoMenuEntidad webPermisoMenu,claseError error) GetPermisoMenuId(int permisoMenuId)
        {
            claseError error = new claseError();
            SEG_PermisoMenuEntidad webPermisoMenu = new SEG_PermisoMenuEntidad();
            string consulta = @"SELECT WEB_PMeID
                                  ,WEB_PMeNombre
                                  ,WEB_PMeFechaRegistro
                                  ,WEB_PMeDataMenu
                                  ,WEB_RolID
                              FROM intranet.seg_permisomenu where WEB_PMeID =@p0";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", permisoMenuId);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                webPermisoMenu.WEB_PMeID = ManejoNulos.ManageNullInteger(dr["WEB_PMeID"]);
                                webPermisoMenu.WEB_PMeNombre = ManejoNulos.ManageNullStr(dr["WEB_PMeNombre"]);
                                webPermisoMenu.WEB_RolID = ManejoNulos.ManageNullInteger(dr["WEB_RolID"]);
                                webPermisoMenu.WEB_PMeFechaRegistro = ManejoNulos.ManageNullDate(dr["WEB_PMeFechaRegistro"]);
                                webPermisoMenu.WEB_PMeDataMenu = ManejoNulos.ManageNullStr(dr["WEB_PMeDataMenu"]);
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

            return (webPermisoMenu,error);
        }

        public (List<SEG_PermisoMenuEntidad> lista,claseError error) GetPermisoMenuRolId(int rolId)
        {
            claseError error = new claseError();
            List<SEG_PermisoMenuEntidad> lista = new List<SEG_PermisoMenuEntidad>();
            string consulta = @"SELECT WEB_PMeID
                                  ,WEB_PMeNombre
                                  ,WEB_PMeFechaRegistro
                                  ,WEB_PMeDataMenu
                                  ,WEB_RolID
                              FROM intranet.seg_permisomenu where WEB_RolID =@p0";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", rolId);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var webPermisoMenu = new SEG_PermisoMenuEntidad
                                {
                                    WEB_PMeID = ManejoNulos.ManageNullInteger(dr["WEB_PMeID"]),
                                    WEB_PMeNombre = ManejoNulos.ManageNullStr(dr["WEB_PMeNombre"]),
                                    WEB_RolID = ManejoNulos.ManageNullInteger(dr["WEB_RolID"]),
                                    WEB_PMeFechaRegistro = ManejoNulos.ManageNullDate(dr["WEB_PMeFechaRegistro"]),
                                    WEB_PMeDataMenu = ManejoNulos.ManageNullStr(dr["WEB_PMeDataMenu"])
                                };

                                lista.Add(webPermisoMenu);
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

            return (lista,error);
        }
        public (List<SEG_PermisoMenuEntidad> lista,claseError error) GetPermisoFechaMax()
        {
            claseError error = new claseError();
            List<SEG_PermisoMenuEntidad> lista = new List<SEG_PermisoMenuEntidad>();
            string consulta = @"SELECT 
                                         (select Max(b.WEB_PMeFechaRegistro) from intranet.seg_permisomenu b) fecha
                                FROM intranet.seg_permisomenu a
                                group by a.WEB_ModuloNombre";
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
                                var webPermisoMenu = new SEG_PermisoMenuEntidad
                                {
                                    WEB_ModuloNombre = ManejoNulos.ManageNullStr(dr["WEB_ModuloNombre"]),
                                    WEB_PMeFechaRegistro = Convert.ToDateTime(dr["fecha"]),
                                };

                                lista.Add(webPermisoMenu);
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

            return (lista,error);
        }

        public (List<SEG_PermisoMenuEntidad> lista,claseError error) GetPermisoMenuIn(string datamenu)
        {
            claseError error = new claseError();
            List<SEG_PermisoMenuEntidad> lista = new List<SEG_PermisoMenuEntidad>();
            string consulta = @"SELECT WEB_PMeID
                                  ,WEB_PMeNombre
                                  ,WEB_PMeFechaRegistro
                                  ,WEB_PMeDataMenu
                                  ,WEB_RolID
                              FROM intranet.seg_permisomenu " + datamenu;
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
                                var webPermisoMenu = new SEG_PermisoMenuEntidad
                                {
                                    WEB_PMeID = ManejoNulos.ManageNullInteger(dr["WEB_PMeID"]),
                                    WEB_PMeNombre = ManejoNulos.ManageNullStr(dr["WEB_PMeNombre"]),
                                    WEB_RolID = ManejoNulos.ManageNullInteger(dr["WEB_RolID"]),
                                    WEB_PMeFechaRegistro = Convert.ToDateTime(dr["WEB_PMeFechaRegistro"]),
                                    WEB_PMeDataMenu = ManejoNulos.ManageNullStr(dr["WEB_PMeDataMenu"])
                                };

                                lista.Add(webPermisoMenu);
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

            return (lista,error);
        }

        public (bool respuesta,claseError error) ActualizarPermisoMenu(SEG_PermisoMenuEntidad permisoMenu)
        {
            claseError error = new claseError();
            bool respuesta = false;
            string consulta = @"UPDATE intranet.seg_permisomenu
                            SET WEB_PMeNombre = @p1
                                ,WEB_PMeFechaRegistro = @p2
                                ,WEB_PMeDataMenu = @p3
                                ,WEB_RolID = @p4
                            WHERE WEB_PMeID = @p0";

            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", permisoMenu.WEB_PMeID);
                    query.Parameters.AddWithValue("@p1", permisoMenu.WEB_PMeNombre);
                    query.Parameters.AddWithValue("@p2", permisoMenu.WEB_PMeFechaRegistro);
                    query.Parameters.AddWithValue("@p3", permisoMenu.WEB_PMeDataMenu);
                    query.Parameters.AddWithValue("@p4", permisoMenu.WEB_RolID);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }

            return (respuesta,error);
        }


        public (bool respuesta,claseError error) EliminarPermisoMenu(string permisoDataMenu, int rolid)
        {
            claseError error = new claseError();
            bool respuesta = false;
            string consulta = @"DELETE FROM intranet.seg_permisomenu WHERE WEB_PMeDataMenu = @p0 and WEB_RolID = @p1";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", permisoDataMenu);
                    query.Parameters.AddWithValue("@p1", rolid);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }

            return (respuesta,error);
        }
    }
}