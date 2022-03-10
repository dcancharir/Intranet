using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers
{
    [autorizacion(false)]
    public class SQLController : Controller
    {
        // GET: SQL
        SQLModel sqlbl = new SQLModel();
        PersonaModel personabl = new PersonaModel();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TMEMPRListarJson()
        {
            var errormensaje = "";
            var listaempresa = new List<TMEMPR>();
            var error = new claseError();
            bool response = false;
            try
            {
                var sqltupla = sqlbl.EmpresaListarJson();
                listaempresa = sqltupla.listaempresa;
                error = sqltupla.error;
                errormensaje = error.Mensaje;
                if (errormensaje.Equals(string.Empty))
                {
                    response = true;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaempresa.ToList(), respuesta = response, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult TTPUES_TRABListarJson(string CO_EMPR)
        {
            var errormensaje = "";
            var listapuesto = new List<TTPUES_TRAB>();
            var error = new claseError();
            bool response = false;
            try
            {
                var sqltupla = sqlbl.PuestoTrabajoObtenerPorEmpresaJson(CO_EMPR);
                listapuesto = sqltupla.listapuesto;
                error = sqltupla.error;
                errormensaje = error.Mensaje;
                if (errormensaje.Equals(string.Empty))
                {
                    response = true;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listapuesto.ToList(), respuesta = response, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult TTSEDEListarporEmpresaJson(string[] listaEmpresas)
        {
            string errormensaje = "";
            List<TTSEDE> lista = new List<TTSEDE>();
            var listasede = new List<dynamic>();
            claseError error = new claseError();
            bool response = false;
            string stringEmpresas = "";
            try
            {
                if (listaEmpresas.Count() > 0)
                {
                    stringEmpresas += "(";
                    foreach (var cod_emp in listaEmpresas)
                    {
                        stringEmpresas += @"'"+cod_emp+"',";
                    }
                    stringEmpresas = stringEmpresas.Substring(0, stringEmpresas.Length - 1);
                    stringEmpresas += ")";
                    var listaTupla = sqlbl.TTSEDEListarporEmpresaJson(stringEmpresas);
                    if (listaTupla.error.Respuesta)
                    {
                        lista = listaTupla.listapuesto;
                        
                        var empresas = lista.GroupBy(z=>new { z.DE_NOMB,z.CO_EMPR}).Select(group=>new { group.Key.DE_NOMB,group.Key.CO_EMPR}).ToList();
                        foreach (var item in empresas)
                        {
                            var listaChildren = new List<dynamic>();
                            foreach (var itemL in lista)
                            {
                                if (item.CO_EMPR == itemL.CO_EMPR)
                                {
                                    listaChildren.Add(new
                                    {
                                        id = itemL.CO_SEDE,
                                        text = itemL.DE_SEDE
                                    });
                                }
                                
                            }

                            listasede.Add(new
                            {
                                id="",
                                text =  item.DE_NOMB,
                                children= listaChildren
                            });
                        }


                        errormensaje = "Listando Sedes";
                        response = true;
                    }
                    else
                    {
                        errormensaje = listaTupla.error.Mensaje;
                    }
                }
                else
                {
                    errormensaje = "Datos Enviados Incorrectos";
                }
         
            }catch(Exception ex)
            {
                errormensaje = ex.Message + ",Llame Administrador";
            }
            return Json(new { data= listasede, mensaje=errormensaje, respuesta=response});
        }
      

    }
}