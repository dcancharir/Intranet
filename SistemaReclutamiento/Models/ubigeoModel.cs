using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Npgsql;
using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Utilitarios;

namespace SistemaReclutamiento.Models
{
    public class UbigeoModel
    {
        string _conexion;
        public UbigeoModel() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<UbigeoEntidad> UbigeoListarPaisesJson() {
            List<UbigeoEntidad> lista = new List<UbigeoEntidad>();
            string consulta = @"SELECT 
                                ubi_id, 
                                ubi_pais_id, 
                                ubi_departamento_id, 
                                ubi_provincia_id, 
                                ubi_distrito_id, 
                                ubi_nombre, 
                                ubi_estado
	                            FROM marketing.cpj_ubigeo
                                where ubi_departamento_id=@p0 and ubi_estado='A'
                                order by ubi_nombre asc;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0" , "0");
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var ubigeo = new UbigeoEntidad
                            {
                                ubi_id = ManejoNulos.ManageNullInteger(dr["ubi_id"]),
                                ubi_departamento_id = ManejoNulos.ManageNullStr(dr["ubi_departamento_id"]),
                                ubi_distrito_id = ManejoNulos.ManageNullStr(dr["ubi_distrito_id"]),
                                ubi_estado = ManejoNulos.ManageNullStr(dr["ubi_estado"]),
                                ubi_pais_id = ManejoNulos.ManageNullStr(dr["ubi_pais_id"]),
                                ubi_provincia_id = ManejoNulos.ManageNullStr(dr["ubi_provincia_id"]),
                                ubi_nombre = ManejoNulos.ManageNullStr(dr["ubi_nombre"])
                            };
                            lista.Add(ubigeo);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return lista;
        }

        public List<UbigeoEntidad> UbigeoListarTodoslosPaisesJson()
        {
            List<UbigeoEntidad> lista = new List<UbigeoEntidad>();
            string consulta = @"SELECT 
                                ubi_id, 
                                ubi_pais_id, 
                                ubi_departamento_id, 
                                ubi_provincia_id, 
                                ubi_distrito_id, 
                                ubi_nombre, 
                                ubi_estado
	                                FROM marketing.cpj_ubigeo
                                where ubi_departamento_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", "0");
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var ubigeo = new UbigeoEntidad
                            {
                                ubi_id = ManejoNulos.ManageNullInteger(dr["ubi_id"]),
                                ubi_departamento_id = ManejoNulos.ManageNullStr(dr["ubi_departamento_id"]),
                                ubi_distrito_id = ManejoNulos.ManageNullStr(dr["ubi_distrito_id"]),
                                ubi_estado = ManejoNulos.ManageNullStr(dr["ubi_estado"]),
                                ubi_pais_id = ManejoNulos.ManageNullStr(dr["ubi_pais_id"]),
                                ubi_provincia_id = ManejoNulos.ManageNullStr(dr["ubi_provincia_id"]),
                                ubi_nombre = ManejoNulos.ManageNullStr(dr["ubi_nombre"])
                            };
                            lista.Add(ubigeo);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return lista;
        }

        public List<UbigeoEntidad> UbigeoListarDepartamentosporPaisJson(string id_pais) {
            List<UbigeoEntidad> lista = new List<UbigeoEntidad>();
            string consulta = @"SELECT 
                                ubi_id, 
                                ubi_pais_id, 
                                ubi_departamento_id, 
                                ubi_provincia_id, 
                                ubi_distrito_id, 
                                ubi_nombre, 
                                ubi_estado
	                            FROM marketing.cpj_ubigeo 
                                where ubi_pais_id=@p0
                                and ubi_provincia_id='0'
                                and ubi_distrito_id='0'
                                and ubi_departamento_id<>'0'
                                ;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", id_pais);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var ubigeo = new UbigeoEntidad
                            {
                                ubi_id = ManejoNulos.ManageNullInteger(dr["ubi_id"]),
                                ubi_departamento_id = ManejoNulos.ManageNullStr(dr["ubi_departamento_id"]),
                                ubi_distrito_id = ManejoNulos.ManageNullStr(dr["ubi_distrito_id"]),
                                ubi_estado = ManejoNulos.ManageNullStr(dr["ubi_estado"]),
                                ubi_pais_id = ManejoNulos.ManageNullStr(dr["ubi_pais_id"]),
                                ubi_provincia_id = ManejoNulos.ManageNullStr(dr["ubi_provincia_id"]),
                                ubi_nombre = ManejoNulos.ManageNullStr(dr["ubi_nombre"])
                            };
                            lista.Add(ubigeo);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return lista;
        }
        public List<UbigeoEntidad> UbigeoListarProvinciasporDepartamentoJson(string id_pais, string id_departamento)
        {
            List<UbigeoEntidad> lista = new List<UbigeoEntidad>();
            string consulta = @"SELECT 
                                ubi_id, 
                                ubi_pais_id, 
                                ubi_departamento_id, 
                                ubi_provincia_id, 
                                ubi_distrito_id, 
                                ubi_nombre, 
                                ubi_estado
	                            FROM marketing.cpj_ubigeo 
                                where ubi_pais_id=@p0
                                and ubi_provincia_id<>'0'
                                and ubi_distrito_id='0'
                                and ubi_departamento_id=@p1
                                order by ubi_nombre asc;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", id_pais);
                    query.Parameters.AddWithValue("@p1", id_departamento);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var ubigeo = new UbigeoEntidad
                            {
                                ubi_id = ManejoNulos.ManageNullInteger(dr["ubi_id"]),
                                ubi_departamento_id = ManejoNulos.ManageNullStr(dr["ubi_departamento_id"]),
                                ubi_distrito_id = ManejoNulos.ManageNullStr(dr["ubi_distrito_id"]),
                                ubi_estado = ManejoNulos.ManageNullStr(dr["ubi_estado"]),
                                ubi_pais_id = ManejoNulos.ManageNullStr(dr["ubi_pais_id"]),
                                ubi_provincia_id = ManejoNulos.ManageNullStr(dr["ubi_provincia_id"]),
                                ubi_nombre = ManejoNulos.ManageNullStr(dr["ubi_nombre"])
                            };
                            lista.Add(ubigeo);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return lista;
        }
        public List<UbigeoEntidad> UbigeoListarDistritosporProvinciaJson(string ubi_pais_id, string ubi_departamento_id,string ubi_provincia_id)
        {
            List<UbigeoEntidad> lista = new List<UbigeoEntidad>();
            string consulta = @"SELECT 
                                ubi_id, 
                                ubi_pais_id, 
                                ubi_departamento_id, 
                                ubi_provincia_id, 
                                ubi_distrito_id, 
                                ubi_nombre, 
                                ubi_estado
	                            FROM marketing.cpj_ubigeo 
                                where ubi_pais_id=@p0
                                and ubi_provincia_id=@p1
                                and ubi_distrito_id<>'0'
                                and ubi_departamento_id=@p2
                                order by ubi_nombre asc;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ubi_pais_id);
                    query.Parameters.AddWithValue("@p1", ubi_provincia_id);
                    query.Parameters.AddWithValue("@p2", ubi_departamento_id);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var ubigeo = new UbigeoEntidad
                            {
                                ubi_id = ManejoNulos.ManageNullInteger(dr["ubi_id"]),
                                ubi_departamento_id = ManejoNulos.ManageNullStr(dr["ubi_departamento_id"]),
                                ubi_distrito_id = ManejoNulos.ManageNullStr(dr["ubi_distrito_id"]),
                                ubi_estado = ManejoNulos.ManageNullStr(dr["ubi_estado"]),
                                ubi_pais_id = ManejoNulos.ManageNullStr(dr["ubi_pais_id"]),
                                ubi_provincia_id = ManejoNulos.ManageNullStr(dr["ubi_provincia_id"]),
                                ubi_nombre = ManejoNulos.ManageNullStr(dr["ubi_nombre"])
                            };
                            lista.Add(ubigeo);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return lista;
        }
        public UbigeoEntidad UbigeoIdObtenerJson(string ubi_pais_id, string ubi_departamento_id, string ubi_provincia_id, string ubi_distrito_id)
        {
            UbigeoEntidad ubigeo = new UbigeoEntidad();
            string consulta = @"SELECT 
                                ubi_id, 
                                ubi_pais_id, 
                                ubi_departamento_id, 
                                ubi_provincia_id, 
                                ubi_distrito_id, 
                                ubi_nombre, 
                                ubi_estado
	                            FROM marketing.cpj_ubigeo 
                                where ubi_pais_id=@p0
                                and ubi_provincia_id=@p1
                                and ubi_distrito_id=@p2
                                and ubi_departamento_id=@p3;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ubi_pais_id);
                    query.Parameters.AddWithValue("@p1", ubi_provincia_id);
                    query.Parameters.AddWithValue("@p2", ubi_distrito_id);
                    query.Parameters.AddWithValue("@p3", ubi_departamento_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                                
                            {
                                ubigeo.ubi_id = ManejoNulos.ManageNullInteger(dr["ubi_id"]);
                                ubigeo.ubi_departamento_id = ManejoNulos.ManageNullStr(dr["ubi_departamento_id"]);
                                ubigeo.ubi_distrito_id = ManejoNulos.ManageNullStr(dr["ubi_distrito_id"]);
                                ubigeo.ubi_estado = ManejoNulos.ManageNullStr(dr["ubi_estado"]);
                                ubigeo.ubi_pais_id = ManejoNulos.ManageNullStr(dr["ubi_pais_id"]);
                                ubigeo.ubi_provincia_id = ManejoNulos.ManageNullStr(dr["ubi_provincia_id"]);
                                ubigeo.ubi_nombre = ManejoNulos.ManageNullStr(dr["ubi_nombre"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return ubigeo;
        }

        public UbigeoEntidad UbigeoObtenerDatosporIdJson(int ubi_id)
        {
            UbigeoEntidad ubigeo = new UbigeoEntidad();
            string consulta = @"SELECT 
                                ubi_id, 
                                ubi_pais_id, 
                                ubi_departamento_id, 
                                ubi_provincia_id, 
                                ubi_distrito_id, 
                                ubi_nombre, 
                                ubi_estado
	                            FROM marketing.cpj_ubigeo 
                                where ubi_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ubi_id);                 
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())

                            {
                                ubigeo.ubi_id = ManejoNulos.ManageNullInteger(dr["ubi_id"]);
                                ubigeo.ubi_departamento_id = ManejoNulos.ManageNullStr(dr["ubi_departamento_id"]);
                                ubigeo.ubi_distrito_id = ManejoNulos.ManageNullStr(dr["ubi_distrito_id"]);
                                ubigeo.ubi_estado = ManejoNulos.ManageNullStr(dr["ubi_estado"]);
                                ubigeo.ubi_pais_id = ManejoNulos.ManageNullStr(dr["ubi_pais_id"]);
                                ubigeo.ubi_provincia_id = ManejoNulos.ManageNullStr(dr["ubi_provincia_id"]);
                                ubigeo.ubi_nombre = ManejoNulos.ManageNullStr(dr["ubi_nombre"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return ubigeo;
        }
    }
}