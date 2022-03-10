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
    public class IntranetDetalleElementoModalModel
    {
        string _conexion;
        public IntranetDetalleElementoModalModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<IntranetDetalleElementoModalEntidad> intranetDetalleElementoModalLista, claseError error) IntranetDetalleElementoModalListarJson()
        {
            List<IntranetDetalleElementoModalEntidad> lista = new List<IntranetDetalleElementoModalEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT detelm_id, detelm_descripcion, detelm_nombre, 
                                detelm_extension, detelm_ubicacion, detelm_estado, fk_elemento_modal,detelm_orden,detelm_posicion
	                                FROM intranet.int_detalle_elemento_modal;";
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
                                var detalle = new IntranetDetalleElementoModalEntidad
                                {

                                    detelm_id = ManejoNulos.ManageNullInteger(dr["detelm_id"]),
                                    detelm_descripcion = ManejoNulos.ManageNullStr(dr["detelm_descripcion"]),
                                    detelm_nombre = ManejoNulos.ManageNullStr(dr["detelm_nombre"]),
                                    detelm_extension = ManejoNulos.ManageNullStr(dr["detelm_extension"]),
                                    detelm_ubicacion = ManejoNulos.ManageNullStr(dr["detelm_ubicacion"]),
                                    detelm_estado = ManejoNulos.ManageNullStr(dr["detelm_estado"]),
                                    fk_elemento_modal = ManejoNulos.ManageNullInteger(dr["fk_elemento_modal"]),
                                    detelm_orden = ManejoNulos.ManageNullInteger(dr["detelm_orden"]),
                                    detelm_posicion = ManejoNulos.ManageNullStr(dr["detelm_posicion"]),
                                };

                                lista.Add(detalle);
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
            return (intranetDetalleElementoModalLista: lista, error: error);
        }

        public (List<IntranetDetalleElementoModalEntidad> intranetDetalleElementoModalListaxElementoID, claseError error) IntranetDetalleElementoModalListarxElementoIDJson(int elemento_id)
        {
            List<IntranetDetalleElementoModalEntidad> lista = new List<IntranetDetalleElementoModalEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT detelm_id, detelm_descripcion,detelm_nombre, 
                                detelm_extension, detelm_ubicacion, detelm_estado, fk_elemento_modal,detelm_orden,detelm_posicion
	                                FROM intranet.int_detalle_elemento_modal 
                                    where fk_elemento_modal=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", elemento_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var detalle = new IntranetDetalleElementoModalEntidad
                                {
                                    detelm_id = ManejoNulos.ManageNullInteger(dr["detelm_id"]),
                                    detelm_descripcion = ManejoNulos.ManageNullStr(dr["detelm_descripcion"]),
                                    detelm_nombre = ManejoNulos.ManageNullStr(dr["detelm_nombre"]),
                                    detelm_extension = ManejoNulos.ManageNullStr(dr["detelm_extension"]),
                                    detelm_ubicacion = ManejoNulos.ManageNullStr(dr["detelm_ubicacion"]),
                                    detelm_estado = ManejoNulos.ManageNullStr(dr["detelm_estado"]),
                                    fk_elemento_modal = ManejoNulos.ManageNullInteger(dr["fk_elemento_modal"]),
                                    detelm_orden = ManejoNulos.ManageNullInteger(dr["detelm_orden"]),
                                    detelm_posicion = ManejoNulos.ManageNullStr(dr["detelm_posicion"]),

                                };

                                lista.Add(detalle);
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
            return (intranetDetalleElementoModalListaxElementoID: lista, error: error);
        }

        public (IntranetDetalleElementoModalEntidad intranetDetalleElementoModal, claseError error) IntranetDetalleElementoModalIdObtenerJson(int detelm_id)
        {
            IntranetDetalleElementoModalEntidad intranetdetalleElementoModal = new IntranetDetalleElementoModalEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT detelm_id, 
detelm_descripcion, detelm_nombre, detelm_extension,
detelm_ubicacion, detelm_estado, fk_elemento_modal, 
detelm_orden, detelm_posicion
	FROM intranet.int_detalle_elemento_modal where detelm_id=@p0;";

            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", detelm_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {

                                intranetdetalleElementoModal.detelm_id = ManejoNulos.ManageNullInteger(dr["detelm_id"]);
                                intranetdetalleElementoModal.detelm_descripcion = ManejoNulos.ManageNullStr(dr["detelm_descripcion"]);
                                intranetdetalleElementoModal.detelm_nombre = ManejoNulos.ManageNullStr(dr["detelm_nombre"]);
                                intranetdetalleElementoModal.detelm_extension = ManejoNulos.ManageNullStr(dr["detelm_extension"]);
                                intranetdetalleElementoModal.detelm_ubicacion = ManejoNulos.ManageNullStr(dr["detelm_ubicacion"]);
                                intranetdetalleElementoModal.detelm_estado = ManejoNulos.ManageNullStr(dr["detelm_estado"]);
                                intranetdetalleElementoModal.fk_elemento_modal = ManejoNulos.ManageNullInteger(dr["fk_elemento_modal"]);
                                intranetdetalleElementoModal.detelm_orden = ManejoNulos.ManageNullInteger(dr["detelm_orden"]);
                                intranetdetalleElementoModal.detelm_posicion = ManejoNulos.ManageNullStr(dr["detelm_posicion"]);
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
            return (intranetDetalleElementoModal: intranetdetalleElementoModal, error: error);
        }
        public (int idIntranetDetalleElementoModalInsertado, claseError error) IntranetDetalleElementoModalInsertarJson(IntranetDetalleElementoModalEntidad intranetDetalleElementoModal)
        {
            //bool response = false;
            int idIntranetDetalleElementoModalInsertado = 0;
            string consulta = @"
            INSERT INTO intranet.int_detalle_elemento_Modal(
	            detelm_descripcion, detelm_nombre, detelm_extension, detelm_ubicacion, detelm_estado,fk_elemento_modal, detelm_orden, detelm_posicion)
	            VALUES ( @p0, @p1, @p2,@p3, @p4,@p5,@p6,@p7)
                returning detelm_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(intranetDetalleElementoModal.detelm_descripcion));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(intranetDetalleElementoModal.detelm_nombre));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(intranetDetalleElementoModal.detelm_extension));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(intranetDetalleElementoModal.detelm_ubicacion));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(intranetDetalleElementoModal.detelm_estado));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullInteger(intranetDetalleElementoModal.fk_elemento_modal));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullInteger(intranetDetalleElementoModal.detelm_orden));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullStr(intranetDetalleElementoModal.detelm_posicion));
  
                    idIntranetDetalleElementoModalInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (idIntranetDetalleElementoModalInsertado: idIntranetDetalleElementoModalInsertado, error: error);
        }

        public (bool intranetDetalleElementoModalEditado, claseError error) IntranetDetalleElementoModalEditarJson(IntranetDetalleElementoModalEntidad intranetDetalleElementoModal)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE intranet.int_detalle_elemento_modal
	                    SET  detelm_descripcion=@p0, detelm_nombre=@p1, detelm_extension=@p2, 
detelm_ubicacion=@p3,  detelm_estado=@p5,fk_elemento_modal=@p7, detelm_posicion=@p8,detelm_orden=@p9
	                    WHERE detelm_id=@p6;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(intranetDetalleElementoModal.detelm_descripcion));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(intranetDetalleElementoModal.detelm_nombre));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(intranetDetalleElementoModal.detelm_extension));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(intranetDetalleElementoModal.detelm_ubicacion));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(intranetDetalleElementoModal.detelm_estado));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullInteger(intranetDetalleElementoModal.detelm_id));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(intranetDetalleElementoModal.fk_elemento_modal));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullStr(intranetDetalleElementoModal.detelm_posicion));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullInteger(intranetDetalleElementoModal.detelm_orden));

                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (intranetDetalleElementoModalEditado: response, error: error);
        }
        public (bool intranetDetalleElementoModalEliminado, claseError error) IntranetDetalleElementoModalEliminarJson(int detelm_id)
        {
            bool response = false;
            string consulta = @"DELETE FROM intranet.int_detalle_elemento_modal
	                            WHERE detelm_id=@p0;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();

                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(detelm_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }

            return (intranetDetalleElementoModalEliminado: response, error: error);
        }

        public (int intranetDetalleElementoModalTotal, claseError error) IntranetDetalleElementoModalObtenerTotalRegistrosxElementoJson(int fk_elemento_modal)
        {
            int intranetDetalleElementoModalTotal = 0;
            claseError error = new claseError();
            string consulta = @"select count(*) as total from intranet.int_detalle_elemento_modal where fk_elemento_modal=@p0";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(fk_elemento_modal));
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                intranetDetalleElementoModalTotal = ManejoNulos.ManageNullInteger(dr["total"]);
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
            return (intranetDetalleElementoModalTotal: intranetDetalleElementoModalTotal, error: error);
        }
        public (bool intranetDetalleElementoModalEliminado, claseError error) IntranetDetalleElementoModalEliminarxElementoModalJson(int fk_elemento_modal)
        {
            bool response = false;
            string consulta = @"DELETE FROM intranet.int_detalle_elemento_modal
	                            WHERE fk_elemento_modal=@p0;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();

                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(fk_elemento_modal));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }

            return (intranetDetalleElementoModalEliminado: response, error: error);
        }
        public (bool intranetDetElementoModalReordenado, claseError error) IntranetDetalleElementoModalEditarOrdenJson(IntranetDetalleElementoModalEntidad intranetDetalleElementoModal)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE intranet.int_detalle_elemento_modal
	                            SET detelm_orden=@p0
	                            WHERE detelm_id=@p1;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(intranetDetalleElementoModal.detelm_orden));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(intranetDetalleElementoModal.detelm_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (intranetDetElementoModalReordenado: response, error: error);
        }
        public (bool intranetDetalleElementoModalEditado, claseError error) IntranetDetalleElementoModalEditarHashJson(IntranetDetalleElementoModalEntidad intranetDetalleElementoModal)
        {
            bool response = false;
            claseError error = new claseError();
            string consulta = @"UPDATE intranet.int_detalle_elemento_modal
	                            SET detelm_hash=@p0
	                            WHERE detelm_id=@p1;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(intranetDetalleElementoModal.detelm_hash));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(intranetDetalleElementoModal.detelm_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (response, error);
        }
    }
}