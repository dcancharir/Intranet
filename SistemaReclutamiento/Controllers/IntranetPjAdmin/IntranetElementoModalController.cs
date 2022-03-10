using SistemaReclutamiento.Entidades.IntranetPJ;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.IntranetPJ;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.IntranetPjAdmin
{
    [autorizacion(false)]
    public class IntranetElementoModalController : Controller
    {
        // GET: IntranetElementoModal
        IntranetElementoModalModel intranetElementoModalbl = new IntranetElementoModalModel();
        IntranetDetalleElementoModalModel intranetDetalleElementoModalbl = new IntranetDetalleElementoModalModel();
        string pathArchivosIntranet = ConfigurationManager.AppSettings["PathArchivosIntranet"].ToString();
        claseError error = new claseError();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IntranetElementoModalListarxSeccionElementoJson(int fk_seccion_elemento)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetElementoModalEntidad> listaElementoModal = new List<IntranetElementoModalEntidad>();
            try
            {
                var DetalleElementoTupla = intranetElementoModalbl.IntranetElementoModalListarxSeccionElementoIDJson(fk_seccion_elemento);
                error = DetalleElementoTupla.error;
                listaElementoModal = DetalleElementoTupla.intranetElementoModalListaxseccionelementoID;
                if (error.Respuesta)
                {
                    mensaje = "Listando Elementos Modal";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Mensaje;
                    mensaje = "No se Pudieron Listar los Elementos Modales";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaElementoModal.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetElementoModalInsertarJson(IntranetElementoModalEntidad intranetElementoModal)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            int idIntranetElementoModalInsertado = 0;
            claseError error = new claseError();
            try
            {
                var totalElementosTupla = intranetElementoModalbl.IntranetElementoModalListarxSeccionElementoIDJson(intranetElementoModal.fk_seccion_elemento);
                error = totalElementosTupla.error;
                if (error.Respuesta)
                {
                    if (totalElementosTupla.intranetElementoModalListaxseccionelementoID.Count > 0)
                    {
                        intranetElementoModal.emod_orden = totalElementosTupla.intranetElementoModalListaxseccionelementoID.Max(x => x.emod_orden) + 1;
                    }
                    else {
                        intranetElementoModal.emod_orden = 1;
                    }
                    var seccionTupla = intranetElementoModalbl.IntranetElementoModalInsertarJson(intranetElementoModal);
                    error = seccionTupla.error;

                    if (error.Respuesta)
                    {
                        mensaje = "Se Registró Correctamente";
                        respuesta = true;
                        idIntranetElementoModalInsertado = seccionTupla.idIntranetElementoModalInsertado;
                    }
                    else
                    {
                        mensaje = "No se Pudo insertar el Elemento Modal";
                        mensajeConsola = error.Mensaje;
                    }
                }
                else
                {
                    mensaje = "Error al Insertar el Nuevo Elemento";
                    mensajeConsola = error.Mensaje;
                }


            }
            catch (Exception exp)
            {
                mensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje, idIntranetElementoModalInsertado = idIntranetElementoModalInsertado, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetElementoModalEliminarJson(int emod_id) {
            string mensaje = "";
            bool response = false;
            claseError error = new claseError();
            string rutaAnterior = "";
            try {
                //Eliminar Imagenes de Detalle de Elemento Modal
                List<IntranetDetalleElementoModalEntidad> detalleElementoModal = new List<IntranetDetalleElementoModalEntidad>();
                var listaDetallesTupla = intranetDetalleElementoModalbl.IntranetDetalleElementoModalListarxElementoIDJson(emod_id);
                detalleElementoModal = listaDetallesTupla.intranetDetalleElementoModalListaxElementoID;
                foreach (var m in detalleElementoModal) {
              
                    var direccion = Server.MapPath("/") + Request.ApplicationPath + "/IntranetFiles/";
                    rutaAnterior = Path.Combine(direccion + m.detelm_nombre+"."+m.detelm_extension);
                    if (System.IO.File.Exists(rutaAnterior))
                    {
                        System.IO.File.Delete(rutaAnterior);
                    }
                }
                var intranetDetalleElementoModalTupla = intranetDetalleElementoModalbl.IntranetDetalleElementoModalEliminarxElementoModalJson(emod_id);
                error = intranetDetalleElementoModalTupla.error;
                if (error.Respuesta)
                {
                    //Se eliminaron los detalles, ahora eliminamos el elemento modal
                    var intranetElementoModalTupla = intranetElementoModalbl.IntranetElementoModalEliminarJson(emod_id);
                    error = intranetElementoModalTupla.error;
                    if (error.Respuesta)
                    {
                        response = true;
                        mensaje = "Se Elimino Correctamente";
                    }
                    else
                    {
                        mensaje = error.Mensaje;
                    }
                }
                else {
                    mensaje = error.Mensaje;
                }
            }
            catch (Exception ex){
                mensaje = ex.Message;
            }
            return Json(new { respuesta=response,mensaje=mensaje });
        }
        [HttpPost]
        public ActionResult IntranetElementoModalIdObtenerJson(int emod_id) {
            string errormensaje = "";
            bool response = false;
            claseError error = new claseError();
            IntranetElementoModalEntidad elementoModal = new IntranetElementoModalEntidad();
            try
            {
                var elementoModaltupla = intranetElementoModalbl.IntranetElementoModalIdObtenerJson(emod_id);
                error = elementoModaltupla.error;
                if (error.Respuesta)
                {
                    elementoModal = elementoModaltupla.intranetElementoModal;
                    response = true;
                    errormensaje = "Obteniendo Data";
                }
                else {
                    errormensaje = error.Mensaje;
                }
            }
            catch (Exception ex)
            {
                errormensaje = ex.Message + ",Llame al Administrador";
            }
            return Json(new { data = elementoModal, respuesta = response, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult IntranetElementoModalEditarJson(IntranetElementoModalEntidad intranetElementoModal) {
            string errormensaje = "";
            bool response = false;
            claseError error = new claseError();
            try
            {
                intranetElementoModal.emod_contenido = intranetElementoModal.emod_titulo;
                intranetElementoModal.emod_descripcion = intranetElementoModal.emod_titulo;
                var intranetElementModalTupla = intranetElementoModalbl.IntranetElementoModalEditarJson(intranetElementoModal);
                error = intranetElementModalTupla.error;
                if (error.Respuesta)
                {
                    response = intranetElementModalTupla.intranetElementoModalEditado;
                }
                else {
                    errormensaje = error.Mensaje;
                }
            }
            catch (Exception ex) {
                errormensaje = ex.Message+",Llame al Administrador";
            }
            return Json(new { respuesta = response, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult IntranetElementoModalEditarOrdenJson(IntranetElementoModalEntidad[] arrayElementoModal)
        {
            IntranetElementoModalEntidad intranetElementoModal = new IntranetElementoModalEntidad();
            claseError error = new claseError();
            bool response = false;
            string errormensaje = "";
            int tamanio = arrayElementoModal.Length;
            foreach (var m in arrayElementoModal)
            {
                intranetElementoModal.emod_id = m.emod_id;
                intranetElementoModal.emod_orden = m.emod_orden;
                var reordenadoTupla = intranetElementoModalbl.IntranetElementoModalEditarOrdenJson(intranetElementoModal);
                error = reordenadoTupla.error;
                if (error.Respuesta)
                {
                    response = reordenadoTupla.intranetElemModalReordenado;
                    errormensaje = "Editado";
                }
                else
                {
                    response = false;
                    errormensaje = "No se Pudo Editar";
                    return Json(new { respuesta = response, mensaje = errormensaje, mensajeconsola = "" });
                }
            }
            return Json(new { tamanioelementomodal = tamanio, respuesta = response, mensaje = errormensaje, mensajeconsola = "" });
        }
    }
    
}