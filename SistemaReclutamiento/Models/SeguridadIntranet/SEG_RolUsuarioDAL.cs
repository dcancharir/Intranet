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
    public class SEG_RolUsuarioDAL
    {
        string _conexion = string.Empty;
        public SEG_RolUsuarioDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public (bool respuesta,claseError error) GuardarRolUsuario(SEG_RolUsuarioEntidad rolUsuario)
        {
            claseError error = new claseError();
            bool respuesta = false;
            string consulta = @"INSERT INTO intranet.seg_rolusuario
           (WEB_RolID,UsuarioID,WEB_RUsuFechaRegistro)VALUES(@p0,@p1,@p2)";

            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(rolUsuario.WEB_RolID));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(rolUsuario.UsuarioID));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(rolUsuario.WEB_RUsuFechaRegistro));
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

        public (List<SEG_RolUsuarioEntidad> lista,claseError error) GetRolUsuario()
        {
            claseError error = new claseError();
            List<SEG_RolUsuarioEntidad> lista = new List<SEG_RolUsuarioEntidad>();
            string consulta = @"SELECT WEB_RUsuID
                              ,WEB_RolID
                              ,UsuarioID
                              ,WEB_RUsuFechaRegistro
                                FROM intranet.seg_rolusuario order by WEB_RUsuID Desc";
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
                            var webRolUsuario = new SEG_RolUsuarioEntidad
                            {
                                WEB_RUsuID = ManejoNulos.ManageNullInteger(dr["WEB_RUsuID"]),
                                WEB_RolID = ManejoNulos.ManageNullInteger(dr["WEB_RolID"]),
                                UsuarioID = ManejoNulos.ManageNullInteger(dr["UsuarioID"]),
                                WEB_RUsuFechaRegistro = ManejoNulos.ManageNullDate(dr["WEB_RUsuFechaRegistro"])
                            };

                            lista.Add(webRolUsuario);
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

        public (List<SEG_RolUsuarioEntidad> lista,claseError error)GetRolUsuariorol_id(int rol_id)
        {
            claseError error = new claseError();
            List<SEG_RolUsuarioEntidad> lista = new List<SEG_RolUsuarioEntidad>();
            string consulta = @"SELECT [WEB_RUsuID
                              ,WEB_RolID
                              ,UsuarioID
                              ,WEB_RUsuFechaRegistro
                                FROM intranet.seg_rolusuario where WEB_RolID=@p0 order by WEB_RUsuID Desc";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", rol_id);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var webRolUsuario = new SEG_RolUsuarioEntidad
                            {
                                WEB_RUsuID = ManejoNulos.ManageNullInteger(dr["WEB_RUsuID"]),
                                WEB_RolID = ManejoNulos.ManageNullInteger(dr["WEB_RolID"]),
                                UsuarioID = ManejoNulos.ManageNullInteger(dr["UsuarioID"]),
                                WEB_RUsuFechaRegistro = ManejoNulos.ManageNullDate(dr["WEB_RUsuFechaRegistro"])
                            };

                            lista.Add(webRolUsuario);
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

        public (SEG_RolUsuarioEntidad webRolUsuario,claseError error) GetRolUsuarioId(int Usuarioid)
        {
            claseError error = new claseError();
            SEG_RolUsuarioEntidad webRolUsuario = new SEG_RolUsuarioEntidad();
            string consulta = @"SELECT WEB_RUsuID
                                  ,WEB_RolID
                                  ,UsuarioID
                                  ,WEB_RUsuFechaRegistro
                              FROM intranet.seg_rolusuario where UsuarioID = @p0";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", Usuarioid);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                webRolUsuario.WEB_RUsuID = ManejoNulos.ManageNullInteger(dr["WEB_RUsuID"]);
                                webRolUsuario.WEB_RolID = ManejoNulos.ManageNullInteger(dr["WEB_RolID"]);
                                webRolUsuario.UsuarioID = ManejoNulos.ManageNullInteger(dr["UsuarioID"]);
                                webRolUsuario.WEB_RUsuFechaRegistro =
                                    ManejoNulos.ManageNullDate(dr["WEB_RUsuFechaRegistro"]);
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

            return (webRolUsuario,error);
        }

        public (bool respuesta,claseError error)ActualizarRolUsuario(SEG_RolUsuarioEntidad rolUsuario)
        {
            claseError error = new claseError();
            bool respuesta = false;
            string consulta = @"UPDATE intranet.seg_rolusuario
                            SET WEB_RolID = @p1,UsuarioID = @p2
                            WHERE WEB_RUsuID = @p0";

            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", rolUsuario.WEB_RUsuID);
                    query.Parameters.AddWithValue("@p1", rolUsuario.WEB_RolID);
                    query.Parameters.AddWithValue("@p2", rolUsuario.UsuarioID);
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

        public (bool respuesta,claseError error) EliminarRolUsuario(int RolUsuid)
        {
            claseError error = new claseError();
            bool respuesta = false;
            string consulta = @"DELETE FROM intranet.seg_rolusuario WHERE UsuarioID = @p0";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", RolUsuid);
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