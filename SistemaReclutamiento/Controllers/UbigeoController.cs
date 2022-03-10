using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Utilitarios;

namespace SistemaReclutamiento.Controllers
{
    [autorizacion(false)]
    public class UbigeoController : Controller
    {
        UbigeoModel ubigeobl = new UbigeoModel();
        // GET: Ubigeo
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UbigeoListarPaisesJson()
        {
            var errormensaje = "";
            var lista = new List<UbigeoEntidad>();
            try
            {
                lista = ubigeobl.UbigeoListarPaisesJson();
                lista = lista.OrderBy(m => m.ubi_nombre).ToList();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult UbigeoListarPaisPeruJson()
        {
            var errormensaje = "";
            var lista = new List<UbigeoEntidad>();
            try
            {
                lista = ubigeobl.UbigeoListarPaisesJson().Where(x=>x.ubi_pais_id=="PE").ToList();
                lista = lista.OrderBy(m => m.ubi_nombre).ToList();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult UbigeoListarTodoslosPaisesJson()
        {
            var errormensaje = "";
            var lista = new List<UbigeoEntidad>();
            try
            {
                lista = ubigeobl.UbigeoListarTodoslosPaisesJson();
                lista = lista.OrderBy(m => m.ubi_nombre).ToList();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult UbigeoListarDepartamentosporPaisJson(string ubi_pais_id)
        {
            var errormensaje = "";
            var lista = new List<UbigeoEntidad>();
            try
            {
                lista = ubigeobl.UbigeoListarDepartamentosporPaisJson(ubi_pais_id);
                
                foreach (var m in lista) {
                    m.ubi_nombre = m.ubi_nombre.Replace("DEPARTAMENTO ", "");
                    m.ubi_nombre.Trim();
                }
                lista = lista.OrderBy(m => m.ubi_nombre).ToList();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult UbigeoListarProvinciasporDepartamentoJson(string ubi_pais_id, string ubi_departamento_id)
        {
            var errormensaje = "";
            var lista = new List<UbigeoEntidad>();
            try
            {
                lista = ubigeobl.UbigeoListarProvinciasporDepartamentoJson(ubi_pais_id,ubi_departamento_id);
                lista = lista.OrderBy(m => m.ubi_nombre).ToList();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult UbigeoListarDistritosporProvinciaJson(string ubi_pais_id, string ubi_departamento_id,string ubi_provincia_id)
        {
            var errormensaje = "";
            var lista = new List<UbigeoEntidad>();
            try
            {
                lista = ubigeobl.UbigeoListarDistritosporProvinciaJson(ubi_pais_id, ubi_departamento_id,ubi_provincia_id);
                lista = lista.OrderBy(m => m.ubi_nombre).ToList();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult UbigeoObtenerDatosporIdJson(int ubi_id)
        {
            var errormensaje = "";
            var ubigeo = new UbigeoEntidad();
            try
            {
                ubigeo = ubigeobl.UbigeoObtenerDatosporIdJson(ubi_id);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = ubigeo, mensaje = errormensaje });
        }
    }
}