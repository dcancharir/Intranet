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
    public class SEG_PermisoRolDAL
    {
        string _conexion = string.Empty;
        public SEG_PermisoRolDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public (bool respuesta,claseError error) GuardarPermisoRol(SEG_PermisoRolEntidad permisoRol)
        {
            claseError error = new claseError();
            bool respuesta = false;
            string consulta = @"INSERT INTO intranet.seg_permisorol
           (WEB_PermID,WEB_RolID,WEB_PRolFechaRegistro)VALUES(@p0,@p1,@p2)";

            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(permisoRol.WEB_PermID));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(permisoRol.WEB_RolID));
                    query.Parameters.AddWithValue("@p2", DateTime.Now);
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

        public (List<SEG_PermisoRolEntidad> lista,claseError error) GetPermisoRol()
        {
            claseError error = new claseError();
            List<SEG_PermisoRolEntidad> lista = new List<SEG_PermisoRolEntidad>();
            string consulta = @"select WEB_PRolID
                                      ,WEB_PermID
                                      ,WEB_RolID
                                      ,WEB_PRolFechaRegistro
                                  FROM intranet.seg_permisorol";
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
                            var webPermisoRol = new SEG_PermisoRolEntidad
                            {
                                WEB_PRolID = ManejoNulos.ManageNullInteger(dr["WEB_PRolID"]),
                                WEB_PermID = ManejoNulos.ManageNullInteger(dr["WEB_PermID"]),
                                WEB_RolID = ManejoNulos.ManageNullInteger(dr["WEB_RolID"]),
                                WEB_PRolFechaRegistro = ManejoNulos.ManageNullDate(dr["WEB_PRolFechaRegistro"]),
                            };

                            lista.Add(webPermisoRol);
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

        public (SEG_PermisoRolEntidad webPermisoRol,claseError error) GetPermisoRolId(int permisoRolid)
        {
            claseError error = new claseError();
            SEG_PermisoRolEntidad webPermisoRol = new SEG_PermisoRolEntidad();
            string consulta = @"SELECT WEB_PRolID
                                  ,WEB_PermID
                                  ,WEB_RolID
                                  ,WEB_PRolFechaRegistro
                              FROM intranet.seg_permisorol where WEB_PRolID =@p0";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", permisoRolid);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                webPermisoRol.WEB_PRolID = ManejoNulos.ManageNullInteger(dr["WEB_PRolID"]);
                                webPermisoRol.WEB_PermID = ManejoNulos.ManageNullInteger(dr["WEB_PermID"]);
                                webPermisoRol.WEB_RolID = ManejoNulos.ManageNullInteger(dr["WEB_RolID"]);
                                webPermisoRol.WEB_PRolFechaRegistro = ManejoNulos.ManageNullDate(dr["WEB_PRolFechaRegistro"]);
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

            return (webPermisoRol,error);
        }

        public (SEG_PermisoRolEntidad webPermisoRol,claseError error) GetseguridadPermisoRol(int rol_id, string controlador, string permiso)
        {
            claseError error = new claseError();
            SEG_PermisoRolEntidad webPermisoRol = new SEG_PermisoRolEntidad();
            string consulta = @"SELECT pr.WEB_PRolID
                                  ,pr.WEB_PermID
                                  ,pr.WEB_RolID
                                  ,p.WEB_PermNombre
                                   ,p.WEB_PermNombreR
                                    ,p.WEB_PermControlador
                                  ,pr.WEB_PRolFechaRegistro
                              FROM intranet.seg_permisorol pr
                                left join intranet.seg_permiso p on p.WEB_PermID=pr.WEB_PermID
                            where pr.WEB_RolID =@p0 and lower(p.WEB_PermControlador)=@p1 and lower(p.WEB_PermNombre)=@p2";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0",ManejoNulos.ManageNullInteger(rol_id));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(controlador.Trim().ToLower()));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(permiso.Trim().ToLower()));
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                webPermisoRol.WEB_PRolID = ManejoNulos.ManageNullInteger(dr["WEB_PRolID"]);
                                webPermisoRol.WEB_PermID = ManejoNulos.ManageNullInteger(dr["WEB_PermID"]);
                                webPermisoRol.WEB_RolID = ManejoNulos.ManageNullInteger(dr["WEB_RolID"]);
                                webPermisoRol.WEB_PermNombre = ManejoNulos.ManageNullStr(dr["WEB_PermNombre"]);
                                webPermisoRol.WEB_PermNombreR = ManejoNulos.ManageNullStr(dr["WEB_PermNombreR"]);
                                webPermisoRol.WEB_PermControlador = ManejoNulos.ManageNullStr(dr["WEB_PermControlador"]);
                                webPermisoRol.WEB_PRolFechaRegistro = ManejoNulos.ManageNullDate(dr["WEB_PRolFechaRegistro"]);
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

            return (webPermisoRol,error);
        }

        public (List<SEG_PermisoRolEntidad> lista,claseError error) GetPermisoRolrolid(int rolid)
        {
            claseError error = new claseError();
            List<SEG_PermisoRolEntidad> lista = new List<SEG_PermisoRolEntidad>();
            string consulta = @"SELECT WEB_PRolID
                                  ,WEB_PermID
                                  ,WEB_RolID
                                  ,WEB_PRolFechaRegistro
                              FROM intranet.seg_permisorol where WEB_RolID =@p0";
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
                                var webPermisoRol = new SEG_PermisoRolEntidad
                                {
                                    WEB_PRolID = ManejoNulos.ManageNullInteger(dr["WEB_PRolID"]),
                                    WEB_PermID = ManejoNulos.ManageNullInteger(dr["WEB_PermID"]),
                                    WEB_RolID = ManejoNulos.ManageNullInteger(dr["WEB_RolID"]),
                                    WEB_PRolFechaRegistro = ManejoNulos.ManageNullDate(dr["WEB_PRolFechaRegistro"]),
                                };

                                lista.Add(webPermisoRol);
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

        public (bool respuesta,claseError error) ActualizarPermisoRol(SEG_PermisoRolEntidad permisoRol)
        {
            claseError error = new claseError();
            bool respuesta = false;
            string consulta = @"UPDATE intranet.seg_permisorol
                            SET WEB_PermID = @p1,WEB_RolID = @p2
                            WHERE WEB_PRolID = @p0";

            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", permisoRol.WEB_PRolID);
                    query.Parameters.AddWithValue("@p1", permisoRol.WEB_PermID);
                    query.Parameters.AddWithValue("@p2", permisoRol.WEB_RolID);
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

        public (bool respuesta,claseError error) EliminarPermisoRol(int permisoid, int rolid)
        {
            claseError error = new claseError();
            bool respuesta = false;
            string consulta = @"DELETE FROM intranet.seg_permisorol WHERE WEB_PermID = @p0 and WEB_RolID = @p1";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", permisoid);
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