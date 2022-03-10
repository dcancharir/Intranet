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
    public class IntranetCPJLocalModel
    {
        string _conexion;
        public IntranetCPJLocalModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (int intranetLocalCantidadSalas,int intranetLocalCantidadApuestasDeportivas, claseError error) IntranetCPJLocalListarCantidadLocalesJson()
        {
            claseError error = new claseError();
            int intranetLocalCantidadSalas = 0;
            int intranetLocalCantidadApuestasDeportivas = 0;
            string consulta = @"SELECT (select count(*) from marketing.cpj_local where loc_tipo='Salas' and loc_estado='A') AS cantidad_salas,
                                (select count(*) from marketing.cpj_local where loc_tipo='ApuestasDeportivas' and loc_estado='A') AS cantidad_apuesta_deportiva
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
                                intranetLocalCantidadSalas = ManejoNulos.ManageNullInteger(dr["cantidad_salas"]);
                                intranetLocalCantidadApuestasDeportivas = ManejoNulos.ManageNullInteger(dr["cantidad_apuesta_deportiva"]);
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
            return (intranetLocalCantidadSalas: intranetLocalCantidadSalas,intranetLocalCantidadApuestasDeportivas:intranetLocalCantidadApuestasDeportivas, error: error);
        }
        public (List<IntranetCPJLocalEntidad> intranetCPJLocalesLista, claseError error) IntranetCPJLocalListarporNombreJson(string tipo,string nombre) {
            List<IntranetCPJLocalEntidad> lista = new List<IntranetCPJLocalEntidad>();
            claseError error = new claseError();
            string consulta = @"select loc_nombre,loc_alias, loc_id,loc_latitud,loc_longitud,loc_direccion,fk_ubigeo,ubi_nombre
	                                from marketing.cpj_local join marketing.cpj_ubigeo on
	                                marketing.cpj_local.fk_ubigeo=marketing.cpj_ubigeo.ubi_id
	                                where lower(ubi_nombre) like '%"+nombre+"%' and loc_tipo='"+tipo+"' and loc_estado='A';";
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
                                var locales = new IntranetCPJLocalEntidad
                                {

                                    loc_nombre = ManejoNulos.ManageNullStr(dr["loc_nombre"]),
                                    loc_alias = ManejoNulos.ManageNullStr(dr["loc_alias"]),
                                    loc_id = ManejoNulos.ManageNullInteger(dr["loc_id"]),
                                    loc_latitud = ManejoNulos.ManageNullDouble(dr["loc_latitud"]),
                                    loc_longitud = ManejoNulos.ManageNullDouble(dr["loc_longitud"]),
                                    loc_direccion = ManejoNulos.ManageNullStr(dr["loc_direccion"]),
                                    fk_ubigeo = ManejoNulos.ManageNullInteger(dr["fk_ubigeo"]),
                                    ubi_nombre = ManejoNulos.ManageNullStr(dr["ubi_nombre"]),
                                };

                                lista.Add(locales);
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
            return (intranetCPJLocalesLista: lista, error: error);
        }
    }
}