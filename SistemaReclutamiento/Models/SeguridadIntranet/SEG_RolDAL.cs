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
    public class SEG_RolDAL
    {
        string _conexion = string.Empty;
        public SEG_RolDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public (bool respuesta,claseError error) GuardarRol(SEG_RolEntidad rol)
        {
            claseError error = new claseError();
            bool respuesta = false;
            string consulta = @"INSERT INTO intranet.seg_rol
           (WEB_RolNombre,WEB_RolDescripcion,WEB_RolEstado,WEB_RolFechaRegistro)
            VALUES(@p0,@p1,@p2,@p3)";

            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(rol.WEB_RolNombre));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(rol.WEB_RolDescripcion));
                    query.Parameters.AddWithValue("@p2", 1);
                    query.Parameters.AddWithValue("@p3", DateTime.Now);
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

        public (List<SEG_RolEntidad> lista,claseError error) GetRoles()
        {
            claseError error = new claseError();
            List<SEG_RolEntidad> lista = new List<SEG_RolEntidad>();
            string consulta = @"SELECT WEB_RolID,WEB_RolNombre,WEB_RolDescripcion,WEB_RolEstado,WEB_RolFechaRegistro
                                FROM intranet.seg_rol order by WEB_RolID Desc";
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
                            var webRol = new SEG_RolEntidad
                            {
                                WEB_RolID = ManejoNulos.ManageNullInteger(dr["WEB_RolID"]),
                                WEB_RolNombre = ManejoNulos.ManageNullStr(dr["WEB_RolNombre"]),
                                WEB_RolDescripcion = ManejoNulos.ManageNullStr(dr["WEB_RolDescripcion"]),
                                WEB_RolEstado = ManejoNulos.ManageNullStr(dr["WEB_RolEstado"]),
                                WEB_RolFechaRegistro = ManejoNulos.ManageNullDate(dr["WEB_RolFechaRegistro"])
                            };

                            lista.Add(webRol);
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

        public (List<SEG_RolEntidad> lista,claseError error) GetRolesActivos()
        {
            claseError error = new claseError();
            List<SEG_RolEntidad> lista = new List<SEG_RolEntidad>();
            string consulta = @"SELECT WEB_RolID,WEB_RolNombre,WEB_RolDescripcion,WEB_RolEstado,WEB_RolFechaRegistro
                                FROM intranet.seg_rol
                                where WEB_RolEstado=1
                                order by WEB_RolID Desc";
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
                            var webRol = new SEG_RolEntidad
                            {
                                WEB_RolID = ManejoNulos.ManageNullInteger(dr["WEB_RolID"]),
                                WEB_RolNombre = ManejoNulos.ManageNullStr(dr["WEB_RolNombre"]),
                                WEB_RolDescripcion = ManejoNulos.ManageNullStr(dr["WEB_RolDescripcion"]),
                                WEB_RolEstado = ManejoNulos.ManageNullStr(dr["WEB_RolEstado"]),
                                WEB_RolFechaRegistro = ManejoNulos.ManageNullDate(dr["WEB_RolFechaRegistro"])
                            };

                            lista.Add(webRol);
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
        public (SEG_RolEntidad webRol,claseError error) GetRolId(int rolid)
        {
            claseError error = new claseError();
            SEG_RolEntidad webRol = new SEG_RolEntidad();
            string consulta = @"SELECT WEB_RolID,WEB_RolNombre,WEB_RolDescripcion,WEB_RolEstado,WEB_RolFechaRegistro
                                FROM intranet.seg_rol where WEB_RolID =@p0";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", rolid);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {

                                webRol.WEB_RolID = ManejoNulos.ManageNullInteger(dr["WEB_RolID"]);
                                webRol.WEB_RolNombre = ManejoNulos.ManageNullStr(dr["WEB_RolNombre"]);
                                webRol.WEB_RolDescripcion = ManejoNulos.ManageNullStr(dr["WEB_RolDescripcion"]);
                                webRol.WEB_RolEstado = ManejoNulos.ManageNullStr(dr["WEB_RolEstado"]);
                                webRol.WEB_RolFechaRegistro =
                                    ManejoNulos.ManageNullDate(dr["WEB_RolFechaRegistro"]);

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

            return (webRol,error);
        }
        public (bool respuesta,claseError error) ActualizarRol(SEG_RolEntidad rol)
        {
            claseError error = new claseError();
            bool respuesta = false;
            string consulta = @"UPDATE intranet.seg_rol
                            SET WEB_RolNombre = @p1,WEB_RolDescripcion = @p2
                            WHERE WEB_RolID = @p0";

            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", rol.WEB_RolID);
                    query.Parameters.AddWithValue("@p1", rol.WEB_RolNombre);
                    query.Parameters.AddWithValue("@p2", rol.WEB_RolDescripcion);
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

        public (bool respuesta,claseError error) ActualizarEstadoRol(int rolid, int estado)
        {
            claseError error = new claseError();
            bool respuesta = false;
            string consulta = @"UPDATE intranet.seg_rol
                            SET WEB_RolEstado = @p1
                            WHERE WEB_RolID = @p0";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", rolid);
                    query.Parameters.AddWithValue("@p1", estado);
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

        public (bool respuesta,claseError error) EliminarRol(int rolid)
        {
            claseError error = new claseError();
            bool respuesta = false;
            string consulta = @"DELETE FROM intranet.seg_rol WHERE WEB_RolID = @p0";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", rolid);
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