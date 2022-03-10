using SistemaReclutamiento.Entidades.IntranetPJ;
using SistemaReclutamiento.Models.IntranetPJ;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.IntranetPjAdmin
{
    [autorizacion(false)]
    public class IntranetSistemasController : Controller
    {
        // GET: IntranetSistemas
        IntranetSistemasModel sistemasbl = new IntranetSistemasModel();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IntranetSistemaListarJson()
        {
            string errormensaje = "";
            bool response = false;
            List<IntranetSistemasEntidad> lista = new List<IntranetSistemasEntidad>();
            
            try
            {
                var listatupla = sistemasbl.IntranetSistemasListarJson();
                if (listatupla.error.Respuesta){
                    lista = listatupla.intranetSistemasLista;
                    errormensaje = "Listando Data";
                    response = true;
                }
                else {
                    errormensaje = listatupla.error.Mensaje;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta = response, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult IntranetSistemaInsertarJson(IntranetSistemasEntidad sistema) {
            string errormensaje = "";
            bool response = false;
            int idIntranetSistema =0;
            try
            {
                var sistematupla = sistemasbl.IntranetSistemaInsertarJson(sistema);
                if (sistematupla.error.Respuesta)
                {
                    idIntranetSistema = sistematupla.idIntranetSistemaInsertado;
                    if (idIntranetSistema > 0)
                    {
                        errormensaje = "Editado con Éxito";
                        response = true;
                    }
                    else {
                        errormensaje = "No se Pudo Insertar";
                    }
                }
                else
                {
                    errormensaje = sistematupla.error.Mensaje;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = sistema, respuesta = response, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult IntranetSistemaIdObtenerJson(int sist_id) {
            string errormensaje = "";
            bool response = false;
            IntranetSistemasEntidad sistema = new IntranetSistemasEntidad();

            try
            {
                var sistematupla = sistemasbl.IntranetSistemaIdObtenerJson(sist_id);
                if (sistematupla.error.Respuesta)
                {
                    sistema = sistematupla.intranetSistema;
                    errormensaje = "Obteniendo Data";
                    response = true;
                }
                else
                {
                    errormensaje = sistematupla.error.Mensaje;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = sistema, respuesta = response, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult IntranetSistemaEditarJson(IntranetSistemasEntidad sistema)
        {
            string errormensaje = "";
            bool response = false;
            try
            {
                var sistematupla = sistemasbl.IntranetSistemaEditarJson(sistema);
                if (sistematupla.error.Respuesta)
                {
                    response = sistematupla.intranetSistemaEditado;
                    errormensaje = "Editado con Éxito";
                }
                else
                {
                    errormensaje = sistematupla.error.Mensaje;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = sistema, respuesta = response, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult IntranetSistemaEliminarJson(int sist_id) {
            string errormensaje = "";
            bool response = false;
            try
            {
                var sistematupla = sistemasbl.IntranetMenuEliminarJson(sist_id);
                if (sistematupla.error.Respuesta)
                {
                    response = sistematupla.intranetSistemaEliminado;
                    errormensaje = "Eliminado con Éxito";
                }
                else
                {
                    errormensaje = sistematupla.error.Mensaje;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta = response, mensaje = errormensaje });
        }
    }
}