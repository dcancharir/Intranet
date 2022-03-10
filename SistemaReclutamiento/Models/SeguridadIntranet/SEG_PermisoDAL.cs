using Npgsql;
using SistemaReclutamiento.Entidades.SeguridadIntranet;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models.SeguridadIntranet
{
    public class SEG_PermisoDAL
    {
        string _conexion = string.Empty;
        public SEG_PermisoDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public (bool respuesta, claseError error) GuardarPermiso(SEG_PermisoEntidad permiso)

        {
            claseError error = new claseError();
            bool respuesta = false;

            string consulta = $@"do $$
	                    declare
	                    selected_permiso intranet.seg_permiso%rowtype;
	                    PermNombre intranet.seg_permiso.WEB_PermNombre%type := '{ManejoNulos.ManageNullStr(permiso.WEB_PermNombre)}';
	                    PermTipo intranet.seg_permiso.WEB_PermTipo%type := '{ManejoNulos.ManageNullStr(permiso.WEB_PermTipo)}';
	                    PermControlador intranet.seg_permiso.WEB_PermControlador%type := '{ManejoNulos.ManageNullStr(permiso.WEB_PermControlador)}';
	                    PermDescripcion intranet.seg_permiso.WEB_PermDescripcion%type := '{ ManejoNulos.ManageNullStr(permiso.WEB_PermDescripcion == null ? "" : permiso.WEB_PermDescripcion)}';
	                    PermEstado intranet.seg_permiso.WEB_PermEstado%type := '{ManejoNulos.ManageNullStr(permiso.WEB_PermEstado)}';
	                    PermFechaRegistro intranet.seg_permiso.WEB_PermFechaRegistro%type := (select now());
	                    ModuloNombre intranet.seg_permiso.WEB_ModuloNombre%type := '{ManejoNulos.ManageNullStr(permiso.WEB_ModuloNombre)}';
	                    begin  
	                    select * from intranet.seg_permiso  
	                    into selected_permiso
	                    where WEB_PermNombre=PermNombre 
	                    AND Web_PermControlador=PermControlador;
	                      if not found then
		                    INSERT INTO intranet.seg_permiso
			                       (WEB_PermNombre,WEB_PermTipo,WEB_PermControlador,WEB_PermDescripcion,
				                    WEB_PermEstado,WEB_PermFechaRegistro,
				                    WEB_ModuloNombre)
				                     VALUES(PermNombre,PermTipo,PermControlador,PermDescripcion,PermEstado,PermFechaRegistro,ModuloNombre);
	                      else
		                     update intranet.seg_permiso
				                      SET
						                    WEB_ModuloNombre=ModuloNombre,
					                       WEB_PermDescripcion=PermDescripcion,
						                    WEB_PermTipo=PermTipo
					                    WHERE WEB_PermNombre=PermNombre AND Web_PermControlador=PermControlador;
	                      end if;
	                    end $$
                ";

            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.ExecuteNonQuery();

                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }

            return (respuesta, error);
        }

        public (bool respuesta,claseError error) BorrarPermiso(string permisonombre, string controlador)
        {
            claseError error = new claseError();
            bool respuesta = false;
            string consulta = @"
                    Delete from intranet.seg_permiso WHERE WEB_PermNombre = @p0 AND Web_PermControlador=@p1;
Delete from intranet.seg_permiso WHERE WEB_PermID = 
(select  WEB_PermID from  intranet.seg_permiso   WHERE WEB_PermNombre =@p0 AND Web_PermControlador=@p1)";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", permisonombre);
                    query.Parameters.AddWithValue("@p1", controlador);
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
        public (SEG_PermisoEntidad webPermisoRol,claseError error) GetPermisoId(string controlador, string permisonombre)
        {
            claseError error = new claseError();
            SEG_PermisoEntidad webPermisoRol = new SEG_PermisoEntidad();

            string consulta = @"SELECT WEB_PermID
                              ,WEB_PermNombre
                              ,WEB_PermNombreR
                              ,WEB_PermTipo
                              ,WEB_PermControlador
                              ,WEB_PermDescripcion
                              ,WEB_PermEstado
                              ,WEB_PermFechaRegistro
                              FROM intranet.seg_permiso where WEB_PermNombre = @p0 and WEB_PermControlador=@p1";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", permisonombre);
                    query.Parameters.AddWithValue("@p1", controlador);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            webPermisoRol.WEB_PermID = ManejoNulos.ManageNullInteger(dr["WEB_PermID"]);
                            webPermisoRol.WEB_PermNombre = ManejoNulos.ManageNullStr(dr["WEB_PermNombre"]);
                            webPermisoRol.WEB_PermNombreR = ManejoNulos.ManageNullStr(dr["WEB_PermNombreR"]);
                            webPermisoRol.WEB_PermControlador = ManejoNulos.ManageNullStr(dr["WEB_PermControlador"]);
                            webPermisoRol.WEB_PermFechaRegistro = ManejoNulos.ManageNullDate(dr["WEB_PermFechaRegistro"]);

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

        public (List<SEG_PermisoEntidad> lista,claseError error) GetPermisos()
        {
            claseError error = new claseError();
            List<SEG_PermisoEntidad> lista = new List<SEG_PermisoEntidad>();
            string consulta = @"SELECT WEB_PermID
                              ,WEB_PermNombre
                              ,WEB_PermNombreR
                              ,WEB_PermTipo
                              ,WEB_PermControlador
                              ,WEB_PermDescripcion
                              ,WEB_PermEstado
                              ,WEB_PermFechaRegistro
                              FROM intranet.seg_permiso order by WEB_PermControlador,WEB_PermNombre ASC";
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
                            var webPermiso = new SEG_PermisoEntidad
                            {
                                WEB_PermID = ManejoNulos.ManageNullInteger(dr["WEB_PermID"]),
                                WEB_PermNombre = ManejoNulos.ManageNullStr(dr["WEB_PermNombre"]),
                                WEB_PermNombreR = ManejoNulos.ManageNullStr(dr["WEB_PermNombreR"]),
                                WEB_PermTipo = ManejoNulos.ManageNullStr(dr["WEB_PermTipo"]),
                                WEB_PermControlador = ManejoNulos.ManageNullStr(dr["WEB_PermControlador"]),
                                WEB_PermDescripcion = ManejoNulos.ManageNullStr(dr["WEB_PermDescripcion"]),
                                WEB_PermEstado = ManejoNulos.ManageNullStr(dr["WEB_PermEstado"]),
                                WEB_PermFechaRegistro = ManejoNulos.ManageNullDate(dr["WEB_PermFechaRegistro"])
                            };

                            lista.Add(webPermiso);
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

        public (List<SEG_PermisoEntidad> lista,claseError error) GetPermisosActivos()
        {
            claseError error = new claseError();
            List<SEG_PermisoEntidad> lista = new List<SEG_PermisoEntidad>();
            string consulta = @"SELECT WEB_PermID
                              ,WEB_PermNombre
                              ,WEB_PermNombreR
                              ,WEB_PermTipo
                              ,WEB_PermControlador
                              ,WEB_PermDescripcion
                              ,WEB_PermEstado
                              ,WEB_PermFechaRegistro
                              FROM intranet.seg_permiso
                            where WEB_PermEstado = '1'
                            order by WEB_PermControlador,WEB_PermNombre ASC";
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
                            var webPermiso = new SEG_PermisoEntidad
                            {
                                WEB_PermID = ManejoNulos.ManageNullInteger(dr["WEB_PermID"]),
                                WEB_PermNombre = ManejoNulos.ManageNullStr(dr["WEB_PermNombre"]),
                                WEB_PermNombreR = ManejoNulos.ManageNullStr(dr["WEB_PermNombreR"]),
                                WEB_PermTipo = ManejoNulos.ManageNullStr(dr["WEB_PermTipo"]),
                                WEB_PermControlador = ManejoNulos.ManageNullStr(dr["WEB_PermControlador"]),
                                WEB_PermDescripcion = ManejoNulos.ManageNullStr(dr["WEB_PermDescripcion"]),
                                WEB_PermEstado = ManejoNulos.ManageNullStr(dr["WEB_PermEstado"]),
                                WEB_PermFechaRegistro = ManejoNulos.ManageNullDate(dr["WEB_PermFechaRegistro"])
                            };

                            lista.Add(webPermiso);
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

        public (bool respuesta,claseError error) ActualizarEstadoPermiso(int web_PermId, int estado)
        {
            claseError error = new claseError();
            bool respuesta = false;
            string consulta = @"UPDATE intranet.seg_permiso
                        SET WEB_PermEstado =@p1
                       WHERE WEB_PermID = @p0";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", web_PermId);
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
            return (respuesta, error) ;
        }

        public (bool respuesta,claseError error) ActualizarDescripcionPermiso(int web_PermId, string descripcion)
        {
            claseError error = new claseError();
            bool respuesta = false;
            string consulta = @"UPDATE intranet.seg_permiso
                        SET WEB_PermDescripcion =@p1
                       WHERE WEB_PermID = @p0";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", web_PermId);
                    query.Parameters.AddWithValue("@p1", descripcion);
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

        public (bool respuesta,claseError error) ActualizarNombrePermiso(int web_PermId, string nombre)
        {
            claseError error = new claseError();
            bool respuesta = false;
            string consulta = @"UPDATE intranet.seg_permiso
                        SET WEB_PermNombreR =@p1
                       WHERE WEB_PermID = @p0";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", web_PermId);
                    query.Parameters.AddWithValue("@p1", nombre);
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