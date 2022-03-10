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
    public class IntranetElementoModalModel
    {
        string _conexion;
        public IntranetElementoModalModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<IntranetElementoModalEntidad> intranetElementoModalLista, claseError error) IntranetElementoModalListarJson()
        {
            List<IntranetElementoModalEntidad> lista = new List<IntranetElementoModalEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT emod_id, emod_titulo, emod_descripcion, emod_contenido, 
                                emod_orden, fk_seccion_elemento, fk_tipo_elemento, emod_estado
	                            FROM intranet.int_elemento_modal    
                                order by emod_orden;";
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
                                var ElementoModal = new IntranetElementoModalEntidad
                                {

                                    emod_id = ManejoNulos.ManageNullInteger(dr["emod_id"]),
                                    emod_titulo = ManejoNulos.ManageNullStr(dr["emod_titulo"]),
                                    emod_descripcion = ManejoNulos.ManageNullStr(dr["emod_descripcion"]),
                                    emod_contenido = ManejoNulos.ManageNullStr(dr["emod_contenido"]),
                                    emod_orden = ManejoNulos.ManageNullInteger(dr["emod_orden"]),
                                    fk_seccion_elemento = ManejoNulos.ManageNullInteger(dr["fk_seccion_elemento"]),
                                    fk_tipo_elemento = ManejoNulos.ManageNullInteger(dr["fk_tipo_elemento"]),
                                    emod_estado = ManejoNulos.ManageNullStr(dr["emod_estado"]),
                                };

                                lista.Add(ElementoModal);
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
            return (intranetElementoModalLista: lista, error: error);
        }

        public (List<IntranetElementoModalEntidad> intranetElementoModalListaxseccionelementoID, claseError error) IntranetElementoModalListarxSeccionElementoIDJson(int seccion_elemento_id)
        {
            List<IntranetElementoModalEntidad> lista = new List<IntranetElementoModalEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT emod_id, emod_titulo, emod_descripcion, emod_contenido, 
                                emod_orden, fk_seccion_elemento, fk_tipo_elemento, emod_estado,tipo_nombre
	                            FROM intranet.int_elemento_modal join intranet.int_tipo_elemento 
								on intranet.int_elemento_modal.fk_tipo_elemento=intranet.int_tipo_elemento.tipo_id
                                where fk_seccion_elemento=@p0
                                order by emod_orden;";
            //string consulta = @"SELECT emod_id, emod_titulo, emod_descripcion, emod_contenido, 
            //                    emod_orden, fk_seccion_elemento, fk_tipo_elemento, emod_estado
            //                 FROM intranet.int_elemento_modal   
            //                    where fk_seccion_elemento = @p0
            //                    order by emod_orden;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", seccion_elemento_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var ElementoModal = new IntranetElementoModalEntidad
                                {

                                    emod_id = ManejoNulos.ManageNullInteger(dr["emod_id"]),
                                    emod_titulo = ManejoNulos.ManageNullStr(dr["emod_titulo"]),
                                    emod_descripcion = ManejoNulos.ManageNullStr(dr["emod_descripcion"]),
                                    emod_contenido = ManejoNulos.ManageNullStr(dr["emod_contenido"]),
                                    emod_orden = ManejoNulos.ManageNullInteger(dr["emod_orden"]),
                                    fk_seccion_elemento = ManejoNulos.ManageNullInteger(dr["fk_seccion_elemento"]),
                                    fk_tipo_elemento = ManejoNulos.ManageNullInteger(dr["fk_tipo_elemento"]),
                                    emod_estado = ManejoNulos.ManageNullStr(dr["emod_estado"]),
                                    tipo_nombre=ManejoNulos.ManageNullStr(dr["tipo_nombre"]),
                                };

                                lista.Add(ElementoModal);
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
            return (intranetElementoModalListaxseccionelementoID: lista, error: error);
        }
        public (IntranetElementoModalEntidad intranetElementoModal, claseError error) IntranetElementoModalIdObtenerJson(int emod_id)
        {
            IntranetElementoModalEntidad intranetElementoModal = new IntranetElementoModalEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT emod_id, 
                                emod_titulo, emod_descripcion, emod_contenido,
                                emod_orden, fk_seccion_elemento, fk_tipo_elemento, 
                                emod_estado
	                                FROM intranet.int_elemento_modal where emod_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", emod_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {

                                intranetElementoModal.emod_id = ManejoNulos.ManageNullInteger(dr["emod_id"]);
                                intranetElementoModal.emod_titulo = ManejoNulos.ManageNullStr(dr["emod_titulo"]);
                                intranetElementoModal.emod_descripcion = ManejoNulos.ManageNullStr(dr["emod_descripcion"]);
                                intranetElementoModal.emod_contenido = ManejoNulos.ManageNullStr(dr["emod_contenido"]);
                                intranetElementoModal.emod_orden = ManejoNulos.ManageNullInteger(dr["emod_orden"]);
                                intranetElementoModal.fk_seccion_elemento = ManejoNulos.ManageNullInteger(dr["fk_seccion_elemento"]);
                                intranetElementoModal.fk_tipo_elemento = ManejoNulos.ManageNullInteger(dr["fk_tipo_elemento"]);
                                intranetElementoModal.emod_estado = ManejoNulos.ManageNullStr(dr["emod_estado"]);
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
            return (intranetElementoModal: intranetElementoModal, error: error);
        }
        public (int idIntranetElementoModalInsertado, claseError error) IntranetElementoModalInsertarJson(IntranetElementoModalEntidad intranetElementoModal)
        {
            //bool response = false;
            int idIntranetElementoModalInsertado = 0;
            string consulta = @"INSERT INTO intranet.int_elemento_modal(
	                            emod_titulo, emod_descripcion, emod_contenido, emod_orden, fk_seccion_elemento, fk_tipo_elemento, emod_estado)
	                            VALUES ( @p0, @p1, @p2, @p3, @p5, @p6, @p7)
                                            returning emod_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(intranetElementoModal.emod_titulo));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(intranetElementoModal.emod_descripcion));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(intranetElementoModal.emod_contenido));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(intranetElementoModal.emod_orden));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullInteger(intranetElementoModal.fk_seccion_elemento));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullInteger(intranetElementoModal.fk_tipo_elemento));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullStr(intranetElementoModal.emod_estado));
                    idIntranetElementoModalInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (idIntranetElementoModalInsertado: idIntranetElementoModalInsertado, error: error);
        }
        public (bool intranetElementoModalEditado, claseError error) IntranetElementoModalEditarJson(IntranetElementoModalEntidad intranetElementoModal)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE intranet.int_elemento_modal
	                        SET  emod_titulo=@p0, emod_descripcion=@p1, emod_contenido=@p2,
                         fk_seccion_elemento=@p5, fk_tipo_elemento=@p6, emod_estado=@p7, emod_orden=@p9
	                        WHERE emod_id=@p8;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(intranetElementoModal.emod_titulo));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(intranetElementoModal.emod_descripcion));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(intranetElementoModal.emod_contenido));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullInteger(intranetElementoModal.fk_seccion_elemento));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullInteger(intranetElementoModal.fk_tipo_elemento));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullStr(intranetElementoModal.emod_estado));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullInteger(intranetElementoModal.emod_id));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullInteger(intranetElementoModal.emod_orden));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (intranetElementoModalEditado: response, error: error);
        }
        public (bool intranetElementoModalEliminado, claseError error) IntranetElementoModalEliminarJson(int emod_id)
        {
            bool response = false;
            string consulta = @"DELETE FROM intranet.int_elemento_modal
	                                WHERE emod_id=@p0;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();

                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(emod_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }

            return (intranetElementoModalEliminado: response, error: error);
        }
        public (bool intranetElementoModalEliminado, claseError error) IntranetElementoModalEliminarxSeccionElementoJson(int fk_seccion_elemento)
        {
            bool response = false;
            string consulta = @"DELETE FROM intranet.int_elemento_modal
	                                WHERE fk_seccion_elemento=@p0;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();

                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(fk_seccion_elemento));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }

            return (intranetElementoModalEliminado: response, error: error);
        }
        public (int intranetElementoModalTotal, claseError error) IntranetElementoModalObtenerTotalRegistrosxSeccionJson(int fk_seccion_elemento)
        {
            int intranetElementoModalTotal = 0;
            claseError error = new claseError();
            string consulta = @"select count(*) as total 
                            from intranet.int_elemento_modal 
                            where fk_seccion_elemento=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(fk_seccion_elemento));
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                intranetElementoModalTotal = ManejoNulos.ManageNullInteger(dr["total"]);
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
            return (intranetElementoModalTotal: intranetElementoModalTotal, error: error);
        }
        public (bool intranetElemModalReordenado, claseError error) IntranetElementoModalEditarOrdenJson(IntranetElementoModalEntidad intranetElementoModal)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE intranet.int_elemento_modal
	                            SET emod_orden=@p0
	                            WHERE emod_id=@p1;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(intranetElementoModal.emod_orden));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(intranetElementoModal.emod_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (intranetElemModalReordenado: response, error: error);
        }
    }
}