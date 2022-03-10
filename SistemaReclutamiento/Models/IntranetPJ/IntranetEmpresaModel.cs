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
    public class IntranetEmpresaModel
    {
        string _conexion;
        public IntranetEmpresaModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<IntranetEmpresaEntidad> intranetEmpresasLista, claseError error) IntranetEmpresasListarJson()
        {
            List<IntranetEmpresaEntidad> lista = new List<IntranetEmpresaEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT emp_id, emp_codigo, emp_nombre, emp_estado
	                            FROM intranet.int_empresa;
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
                                var empresas = new IntranetEmpresaEntidad
                                {

                                    emp_id = ManejoNulos.ManageNullInteger(dr["emp_id"]),
                                    emp_codigo = ManejoNulos.ManageNullStr(dr["emp_codigo"]),
                                    emp_nombre = ManejoNulos.ManageNullStr(dr["emp_nombre"]),
                                    emp_estado = ManejoNulos.ManageNullStr(dr["emp_estado"]),
                                };

                                lista.Add(empresas);
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
            return (intranetEmpresasLista: lista, error: error);
        }
    }
}