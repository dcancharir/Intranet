using SistemaReclutamiento.Entidades.IntranetPJ;
using SistemaReclutamiento.Models;
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
    public class IntranetTipoElementoController : Controller
    {
        // GET: IntranetTipoElemento
        IntranetTipoElementoModel intranetTipoElementobl = new IntranetTipoElementoModel();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IntranetTipoElementoListarJson()
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetTipoElementoEntidad> listaTipoElementos = new List<IntranetTipoElementoEntidad>();
            claseError error = new claseError();
            try
            {
                var tipoElementoTupla = intranetTipoElementobl.IntranetTipoElementoListarJson();
                error = tipoElementoTupla.error;
                listaTipoElementos = tipoElementoTupla.intranetTipoElementoLista.Where(x=>x.tipo_estado.Equals("A")).OrderBy(x=>x.tipo_orden).ToList();
                if (error.Respuesta)
                {
                    mensaje = "Listando Tipo de Elemento";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Mensaje;
                    mensaje = "No se pudo listar";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaTipoElementos.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
    }
}