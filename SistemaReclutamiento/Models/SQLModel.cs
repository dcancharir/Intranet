using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models
{
    public class SQLModel
    {
        string _conexion;
        string _conexion_concar;
        public SQLModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexionSQL"].ConnectionString;
            _conexion_concar = ConfigurationManager.ConnectionStrings["conexionSQLCONCAR"].ConnectionString;
        }
        public (List<TMEMPR> listaempresa, claseError error) EmpresaListarJson()
        {
            List<TMEMPR> lista = new List<TMEMPR>();
            claseError error = new claseError();
            string consulta = @"Select 
                                empresa.CO_EMPR,
                                empresa.DE_NOMB,
                                empresa.DE_NOMB_CORT,
                                empresa.NO_DEPA,
                                empresa.NO_PROV,
                                empresa.NU_RUCS,
                                empresa.NO_REPR_LEGA,
                                pais.NO_PAIS
                                from 
                                TMEMPR as empresa join TTPAIS as pais
								on empresa.CO_PAIS=pais.CO_PAIS
								where TI_SITU='ACT'";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    //query.Parameters.AddWithValue("@p0", per_numdoc);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var empresa = new TMEMPR()
                                {
                                    CO_EMPR = ManejoNulos.ManageNullStr(dr["CO_EMPR"]),
                                    DE_NOMB = ManejoNulos.ManageNullStr(dr["DE_NOMB"]),
                                    DE_NOMB_CORT = ManejoNulos.ManageNullStr(dr["DE_NOMB_CORT"]),
                                    NO_DEPA = ManejoNulos.ManageNullStr(dr["NO_DEPA"]),
                                    NO_PROV = ManejoNulos.ManageNullStr(dr["NO_PROV"]),
                                    NU_RUCS = ManejoNulos.ManageNullStr(dr["NU_RUCS"]),
                                    NO_REPR_LEGA = ManejoNulos.ManageNullStr(dr["NO_REPR_LEGA"]),
                                    NO_PAIS = ManejoNulos.ManageNullStr(dr["NO_PAIS"]),
                                };
                                lista.Add(empresa);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return (listaempresa: lista, error: error);
        }
        public (List<TTPUES_TRAB> listapuesto, claseError error) PuestoTrabajoObtenerPorEmpresaJson(string CO_EMPR)
        {
            List<TTPUES_TRAB> lista = new List<TTPUES_TRAB>();
            claseError error = new claseError();
            string consulta = @"Select 
                                CO_EMPR,
                                CO_PUES_TRAB,
                                DE_PUES_TRAB
                                from 
                                TTPUES_TRAB where CO_EMPR=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", CO_EMPR);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var puesto = new TTPUES_TRAB()
                                {
                                    CO_EMPR = ManejoNulos.ManageNullStr(dr["CO_EMPR"]),
                                    CO_PUES_TRAB = ManejoNulos.ManageNullStr(dr["CO_PUES_TRAB"]),
                                    DE_PUES_TRAB = ManejoNulos.ManageNullStr(dr["DE_PUES_TRAB"]),
                                };
                                lista.Add(puesto);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return (listapuesto: lista, error: error);
        }
        //Consulta para modal de Cumpleaños Intranet
        public (PersonaSqlEntidad persona, claseError error) PersonaSQLObtenerInformacionPuestoTrabajoJson(string dni, int mes, int anio) {
            PersonaSqlEntidad persona = new PersonaSqlEntidad();
            claseError error = new claseError();
            string consulta = @"Select top 1
                    emp.CO_TRAB, 
                    emp.NO_TRAB, 
                    emp.NO_APEL_PATE, 
                    emp.NO_APEL_MATE, 
                    emp.TI_SITU,
		            emp.NO_DIRE_TRAB,
					emp.NU_TLF1,
                    empresa.DE_NOMB,
                    empresa.NU_RUCS,
	                empresa.CO_EMPR,
                    unidad.DE_UNID, 
                    sede.DE_SEDE,  
                    gerencia.DE_DEPA, 
                    area.DE_AREA,  
                    grupo.DE_GRUP_OCUP,
                    puesto.DE_PUES_TRAB
                    from TMTRAB_PERS as emp inner join TMTRAB_CALC as periodo on emp.CO_TRAB=periodo.CO_TRAB
                    inner join TMEMPR as empresa on periodo.CO_EMPR=empresa.CO_EMPR 
                    inner join TMUNID_EMPR as unidad on unidad.CO_EMPR=empresa.CO_EMPR and unidad.CO_UNID=periodo.CO_UNID 
                    inner join TTSEDE as sede on sede.CO_EMPR=empresa.CO_EMPR and periodo.CO_SEDE=sede.CO_SEDE 
                    inner join TTDEPA as gerencia on gerencia.CO_EMPR=empresa.CO_EMPR and periodo.CO_DEPA=gerencia.CO_DEPA 
                    inner join TTAREA as area on area.CO_AREA=periodo.CO_AREA and area.CO_EMPR=periodo.CO_EMPR and periodo.CO_DEPA=area.CO_DEPA 
                    inner join TTGRUP_OCUP as grupo on grupo.CO_EMPR=empresa.CO_EMPR and grupo.CO_GRUP_OCUP=periodo.CO_GRUP_OCUP 
                    inner join TTPUES_TRAB as puesto on puesto.CO_EMPR=empresa.CO_EMPR and puesto.CO_PUES_TRAB=periodo.CO_PUES_TRAB 
                    where emp.CO_TRAB=@p0
                    and periodo.NU_ANNO=@p2 and periodo.NU_PERI=@p1
                    order by periodo.NU_ANNO desc;";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", dni);
                    query.Parameters.AddWithValue("@p1", mes);
                    query.Parameters.AddWithValue("@p2", anio);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())

                            {
                                persona.CO_TRAB = ManejoNulos.ManageNullStr(dr["CO_TRAB"]);
                                persona.NO_TRAB = ManejoNulos.ManageNullStr(dr["NO_TRAB"]);
                                persona.NO_APEL_PATE = ManejoNulos.ManageNullStr(dr["NO_APEL_PATE"]);
                                persona.NO_APEL_MATE = ManejoNulos.ManageNullStr(dr["NO_APEL_MATE"]);
                                persona.TI_SITU = ManejoNulos.ManageNullStr(dr["TI_SITU"]);
                                persona.DE_NOMB = ManejoNulos.ManageNullStr(dr["DE_NOMB"]);
                                persona.DE_UNID = ManejoNulos.ManageNullStr(dr["DE_UNID"]);
                                persona.DE_SEDE = ManejoNulos.ManageNullStr(dr["DE_SEDE"]);
                                persona.DE_DEPA = ManejoNulos.ManageNullStr(dr["DE_DEPA"]);
                                persona.DE_AREA = ManejoNulos.ManageNullStr(dr["DE_AREA"]);
                                persona.DE_GRUP_OCUP = ManejoNulos.ManageNullStr(dr["DE_GRUP_OCUP"]);
                                persona.DE_PUES_TRAB = ManejoNulos.ManageNullStr(dr["DE_PUES_TRAB"]);
                                persona.NU_TLF1 = ManejoNulos.ManageNullStr(dr["NU_TLF1"]);
                                persona.NO_DIRE_TRAB = ManejoNulos.ManageNullStr(dr["NO_DIRE_TRAB"]);
                                persona.NU_RUCS = ManejoNulos.ManageNullStr(dr["NU_RUCS"]);
                                persona.CO_EMPR = ManejoNulos.ManageNullStr(dr["CO_EMPR"]);
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

            return (persona:persona,error:error);
        }
        //Consulta para listado de cumpleaños desde GDT
        public (List<PersonaSqlEntidad> lista, claseError error) PersonaSQLObtenerListaCumpleaniosJson(string listaEmpresas, int mes_activo) {
            claseError error = new claseError();
            List<PersonaSqlEntidad> listaPersonas = new List<PersonaSqlEntidad>();
            string consulta = @"select emp.CO_TRAB,emp.NO_TRAB, emp.NO_APEL_PATE, emp.NO_APEL_MATE,FE_NACI_TRAB,
                                emp.NO_DIRE_MAI1,empresa.CO_EMPR,empresa.DE_NOMB from
                                TMTRAB_PERS as emp inner join TMTRAB_CALC as periodo on emp.CO_TRAB=periodo.CO_TRAB 
                                inner join TMEMPR as empresa on periodo.CO_EMPR=empresa.CO_EMPR 
                                where 
                                periodo.NU_ANNO=year(getdate()) and periodo.NU_PERI=
                                "+mes_activo+" and (select month(emp.FE_NACI_TRAB))=" +
                                "(select MONTH(getdate())) and (select day(emp.FE_NACI_TRAB))>=(select day(getdate())) " +
                                "and empresa.CO_EMPR in "+listaEmpresas+" order by day(emp.FE_NACI_TRAB) asc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

             

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var persona = new PersonaSqlEntidad
                                {
                                    CO_TRAB=ManejoNulos.ManageNullStr(dr["CO_TRAB"]),
                                    NO_TRAB = ManejoNulos.ManageNullStr(dr["NO_TRAB"]),
                                    NO_APEL_PATE = ManejoNulos.ManageNullStr(dr["NO_APEL_PATE"]),
                                    NO_APEL_MATE = ManejoNulos.ManageNullStr(dr["NO_APEL_MATE"]),
                                    FE_NACI_TRAB = ManejoNulos.ManageNullDate(dr["FE_NACI_TRAB"]),
                                    CO_EMPR = ManejoNulos.ManageNullStr(dr["CO_EMPR"]),
                                    DE_NOMB= ManejoNulos.ManageNullStr(dr["DE_NOMB"]),
                                    NO_DIRE_MAI1 = ManejoNulos.ManageNullStr(dr["NO_DIRE_MAI1"]),
                                };

                                listaPersonas.Add(persona);
                            }
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (lista:listaPersonas,error:error);
        }
        //Lista de personas de GDT para Agenda Intranet
        public (List<PersonaSqlEntidad> lista, claseError error) PersonaSQLObtenerListaAgendaJson(string listaEmpresas, int mes_activo)
        {
            claseError error = new claseError();
            List<PersonaSqlEntidad> listaPersonas = new List<PersonaSqlEntidad>();
            string consulta = @"Select distinct emp.CO_TRAB, emp.NO_TRAB, emp.NO_APEL_PATE, emp.NO_APEL_MATE, empresa.DE_NOMB, 
                                 area.DE_AREA,
                                puesto.DE_PUES_TRAB, emp.NU_TLF1, emp.NU_TLF2, emp.NO_DIRE_MAI1   
                                from TMTRAB_PERS as emp inner join TMTRAB_CALC as periodo on emp.CO_TRAB=periodo.CO_TRAB 
                                inner join TMEMPR as empresa on periodo.CO_EMPR=empresa.CO_EMPR 
                                inner join TMUNID_EMPR as unidad on unidad.CO_EMPR=empresa.CO_EMPR and unidad.CO_UNID=periodo.CO_UNID 
                                inner join TTSEDE as sede on sede.CO_EMPR=empresa.CO_EMPR and periodo.CO_SEDE=sede.CO_SEDE 
                                inner join TTDEPA as gerencia on gerencia.CO_EMPR=empresa.CO_EMPR and periodo.CO_DEPA=gerencia.CO_DEPA 
                                inner join TTAREA as area on area.CO_AREA=periodo.CO_AREA and area.CO_EMPR=periodo.CO_EMPR and periodo.CO_DEPA=area.CO_DEPA 
                                inner join TTGRUP_OCUP as grupo on grupo.CO_EMPR=empresa.CO_EMPR and grupo.CO_GRUP_OCUP=periodo.CO_GRUP_OCUP 
                                inner join TTPUES_TRAB as puesto on puesto.CO_EMPR=empresa.CO_EMPR and puesto.CO_PUES_TRAB=periodo.CO_PUES_TRAB 
                                where periodo.NU_ANNO=year(getdate()) and periodo.NU_PERI="+mes_activo+" and empresa.CO_EMPR in " + listaEmpresas+" order by emp.NO_APEL_PATE asc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);



                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var persona = new PersonaSqlEntidad
                                {
                                    CO_TRAB = ManejoNulos.ManageNullStr(dr["CO_TRAB"]),
                                    NO_TRAB = ManejoNulos.ManageNullStr(dr["NO_TRAB"]),
                                    NO_APEL_PATE = ManejoNulos.ManageNullStr(dr["NO_APEL_PATE"]),
                                    NO_APEL_MATE = ManejoNulos.ManageNullStr(dr["NO_APEL_MATE"]),
                                    DE_NOMB = ManejoNulos.ManageNullStr(dr["DE_NOMB"]),
                                    DE_AREA = ManejoNulos.ManageNullStr(dr["DE_AREA"]),
                                    DE_PUES_TRAB = ManejoNulos.ManageNullStr(dr["DE_PUES_TRAB"]),
                                    NU_TLF1 = ManejoNulos.ManageNullStr(dr["NU_TLF1"]),
                                    NU_TLF2 = ManejoNulos.ManageNullStr(dr["NU_TLF2"]),
                                    NO_DIRE_MAI1 = ManejoNulos.ManageNullStr(dr["NO_DIRE_MAI1"]),
                                    
                                };

                                listaPersonas.Add(persona);
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
            return (lista: listaPersonas, error: error);
        }
        public (List<TTSEDE> listapuesto, claseError error) TTSEDEListarporEmpresaJson(string listaEmpresas)
        {
            List<TTSEDE> lista = new List<TTSEDE>();
            claseError error = new claseError();

            string consulta = @"select sed.CO_EMPR,sed.CO_SEDE,sed.DE_SEDE ,em.DE_NOMB 
                                from TTSEDE sed
                                join TMEMPR em on em.CO_EMPR=sed.CO_EMPR where sed.CO_EMPR in " + listaEmpresas;

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var sede = new TTSEDE()
                                {
                                    CO_EMPR = ManejoNulos.ManageNullStr(dr["CO_EMPR"]),
                                    DE_NOMB = ManejoNulos.ManageNullStr(dr["DE_NOMB"]),
                                    CO_SEDE = ManejoNulos.ManageNullStr(dr["CO_SEDE"]),
                                    DE_SEDE = ManejoNulos.ManageNullStr(dr["DE_SEDE"]),
                                };
                                lista.Add(sede);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return (listapuesto: lista, error: error);
        }
        //consulta para Busqueda de empleados por apellidos
        public (List<PersonaSqlEntidad> lista, claseError error) PersonaSQLObtenerInformacionPuestoTrabajoxApellidoJson(string apellidos, int mes, int anio)
        {
            claseError error = new claseError();
            List<PersonaSqlEntidad> listaPersonas = new List<PersonaSqlEntidad>();
            string consulta = @"Select emp.CO_TRAB, 
                    emp.NO_TRAB, 
                    emp.NO_APEL_PATE, 
                    emp.NO_APEL_MATE, 
                    emp.TI_SITU,
		            emp.NO_DIRE_TRAB,
					emp.NU_TLF1,
                    empresa.DE_NOMB,
                    empresa.NU_RUCS,
                    unidad.DE_UNID, 
                    sede.DE_SEDE,  
                    gerencia.DE_DEPA, 
                    area.DE_AREA,  
                    grupo.DE_GRUP_OCUP,
                    puesto.DE_PUES_TRAB
                    from TMTRAB_PERS as emp inner join TMTRAB_CALC as periodo on emp.CO_TRAB=periodo.CO_TRAB
                    inner join TMEMPR as empresa on periodo.CO_EMPR=empresa.CO_EMPR 
                    inner join TMUNID_EMPR as unidad on unidad.CO_EMPR=empresa.CO_EMPR and unidad.CO_UNID=periodo.CO_UNID 
                    inner join TTSEDE as sede on sede.CO_EMPR=empresa.CO_EMPR and periodo.CO_SEDE=sede.CO_SEDE 
                    inner join TTDEPA as gerencia on gerencia.CO_EMPR=empresa.CO_EMPR and periodo.CO_DEPA=gerencia.CO_DEPA 
                    inner join TTAREA as area on area.CO_AREA=periodo.CO_AREA and area.CO_EMPR=periodo.CO_EMPR and periodo.CO_DEPA=area.CO_DEPA 
                    inner join TTGRUP_OCUP as grupo on grupo.CO_EMPR=empresa.CO_EMPR and grupo.CO_GRUP_OCUP=periodo.CO_GRUP_OCUP 
                    inner join TTPUES_TRAB as puesto on puesto.CO_EMPR=empresa.CO_EMPR and puesto.CO_PUES_TRAB=periodo.CO_PUES_TRAB 
                    where concat(emp.NO_APEL_PATE, ' ', emp.NO_APEL_MATE)  like '%"+apellidos+"%' and periodo.NU_PERI=@p1 and periodo.NU_ANNO=@p2;";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", mes);
                    query.Parameters.AddWithValue("@p2", anio);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var persona = new PersonaSqlEntidad
                                {
                                    CO_TRAB = ManejoNulos.ManageNullStr(dr["CO_TRAB"]),
                                    NO_TRAB = ManejoNulos.ManageNullStr(dr["NO_TRAB"]),
                                    NO_APEL_PATE = ManejoNulos.ManageNullStr(dr["NO_APEL_PATE"]),
                                    NO_APEL_MATE = ManejoNulos.ManageNullStr(dr["NO_APEL_MATE"]),
                                    TI_SITU = ManejoNulos.ManageNullStr(dr["TI_SITU"]),
                                    DE_NOMB = ManejoNulos.ManageNullStr(dr["DE_NOMB"]),
                                    DE_UNID = ManejoNulos.ManageNullStr(dr["DE_UNID"]),
                                    DE_SEDE = ManejoNulos.ManageNullStr(dr["DE_SEDE"]),
                                    DE_DEPA = ManejoNulos.ManageNullStr(dr["DE_DEPA"]),
                                    DE_AREA = ManejoNulos.ManageNullStr(dr["DE_AREA"]),
                                    DE_GRUP_OCUP = ManejoNulos.ManageNullStr(dr["DE_GRUP_OCUP"]),
                                    DE_PUES_TRAB = ManejoNulos.ManageNullStr(dr["DE_PUES_TRAB"]),
                                    NU_TLF1 = ManejoNulos.ManageNullStr(dr["NU_TLF1"]),
                                    NO_DIRE_TRAB = ManejoNulos.ManageNullStr(dr["NO_DIRE_TRAB"]),
                                    NU_RUCS = ManejoNulos.ManageNullStr(dr["NU_RUCS"]),
                            };

                                listaPersonas.Add(persona);
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
            return (lista: listaPersonas, error: error);
        }
    }
}