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
    public class IntranetElementoController : Controller
    {
        // GET: IntranetElemento
        IntranetElementoModel intranetElementobl = new IntranetElementoModel();

        IntranetDetalleElementoModel intranetDetalleElementonbl = new IntranetDetalleElementoModel();
        IntranetSeccionElementoModel intranetSeccionElementobl = new IntranetSeccionElementoModel();
        IntranetElementoModalModel intanetElementoModalbl = new IntranetElementoModalModel();
        IntranetDetalleElementoModalModel intranetDetalleElementoModalbl = new IntranetDetalleElementoModalModel();
        claseError error = new claseError();
        string pathArchivosIntranet = ConfigurationManager.AppSettings["PathArchivosIntranet"].ToString();
        RutaImagenes rutaImagenes = new RutaImagenes();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IntranetElementoListarxSeccionIDJson(int sec_id)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetElementoEntidad> listaElementos = new List<IntranetElementoEntidad>();
            claseError error = new claseError();
            try
            {
                var ElementoTupla = intranetElementobl.IntranetElementoListarxSeccionIDJson(sec_id);
                error = ElementoTupla.error;
                listaElementos = ElementoTupla.intranetElementoListaxSeccionID;
                if (error.Respuesta)
                {
                    mensaje = "Listando Elementos";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Mensaje;
                    mensaje = "No se Pudieron Listar las Elementoes";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaElementos.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetElementoInsertarJson(IntranetElementoEntidad intranetElemento)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            int idIntranetElementoInsertado = 0;
            claseError error = new claseError();
            try
            {
                var totalElementosTupla = intranetElementobl.IntranetElementoListarxSeccionIDJson(intranetElemento.fk_seccion);
                error = totalElementosTupla.error;
                if (error.Respuesta)
                {
                    intranetElemento.elem_descripcion = intranetElemento.elem_titulo;
                    intranetElemento.elem_contenido = intranetElemento.elem_titulo;
                    if (totalElementosTupla.intranetElementoListaxSeccionID.Count > 0)
                    {
                        intranetElemento.elem_orden = totalElementosTupla.intranetElementoListaxSeccionID.Max(x => x.elem_orden) + 1;
                    }
                    else {
                        intranetElemento.elem_orden = 1;
                    }
                    var seccionTupla = intranetElementobl.IntranetElementoInsertarJson(intranetElemento);
                    error = seccionTupla.error;

                    if (error.Respuesta)
                    {
                        mensaje = "Se Registró Correctamente";
                        respuesta = true;
                        idIntranetElementoInsertado = seccionTupla.idIntranetElementoInsertado;
                    }
                    else
                    {
                        mensaje = "No se Pudo insertar el Elemento";
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

            return Json(new { respuesta = respuesta, mensaje = mensaje, idIntranetElementoInsertado = idIntranetElementoInsertado, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetElementoElementoEliminarJson(int elem_id)
        {
            string errormensaje = "";
            bool respuestaConsulta = false;
            string mensajeConsola = "";
            claseError error = new claseError();
            List<IntranetDetalleElementoModalEntidad> listaDetalleElementoModal = new List<IntranetDetalleElementoModalEntidad>();
            List<IntranetElementoModalEntidad> listaElementoModal = new List<IntranetElementoModalEntidad>();
            IntranetSeccionElementoEntidad seccionElemento = new IntranetSeccionElementoEntidad();
            List<IntranetDetalleElementoEntidad> listaDetalleElemento = new List<IntranetDetalleElementoEntidad>();
            string rutaEliminar = "";
            try
            {
                //Buscar los Detalles que pudiera tener
                var detalleElementoTupla2 = intranetDetalleElementonbl.IntranetDetalleElementoListarxElementoIDJson(elem_id);

                if (detalleElementoTupla2.error.Respuesta) {
                    listaDetalleElemento = detalleElementoTupla2.intranetDetalleElementoListaxElementoID;
                    if (listaDetalleElemento.Count > 0) {
                        foreach (var j in listaDetalleElemento) {
                            var detalleElementoTupla = intranetDetalleElementonbl.IntranetDetalleElementoIdObtenerJson(j.detel_id);
                            if (detalleElementoTupla.error.Respuesta)
                            {
                                int fk_seccion_elemento = detalleElementoTupla.intranetDetalleElemento.fk_seccion_elemento;
                                if (fk_seccion_elemento > 0)
                                {
                                    //Buscar todos los elementos modales que tengan ese fk_seccion elemento
                                    var listaElementosTupla = intanetElementoModalbl.IntranetElementoModalListarxSeccionElementoIDJson(fk_seccion_elemento);
                                    if (listaElementosTupla.error.Respuesta)
                                    {
                                        listaElementoModal = listaElementosTupla.intranetElementoModalListaxseccionelementoID;
                                        if (listaElementoModal.Count > 0)
                                        {
                                            //Buscar todos los detalles de Elemento Modal por Elemento modal
                                            foreach (var m in listaElementoModal)
                                            {
                                                var detalleElementoModalTupla = intranetDetalleElementoModalbl.IntranetDetalleElementoModalListarxElementoIDJson(m.emod_id);
                                                if (detalleElementoModalTupla.error.Respuesta)
                                                {
                                                    listaDetalleElementoModal = detalleElementoModalTupla.intranetDetalleElementoModalListaxElementoID;
                                                    if (listaDetalleElementoModal.Count > 0)
                                                    {
                                                        foreach (var k in listaDetalleElementoModal)
                                                        {
                                                            //Eliminar imagenes si las hubiera
                                                            if (k.detelm_extension != "")
                                                            {
                                                               
                                                                var direcciondetElemento = Server.MapPath("/") + Request.ApplicationPath + "/IntranetFiles/";
                                                                rutaEliminar = Path.Combine(direcciondetElemento + k.detelm_nombre + "." + k.detelm_extension);
                                                                if (System.IO.File.Exists(rutaEliminar))
                                                                {
                                                                    System.IO.File.Delete(rutaEliminar);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                //Eliminar Detalles de Elemento Modal por cada Elemento Modal
                                                var detElemModalTupla = intranetDetalleElementoModalbl.IntranetDetalleElementoModalEliminarxElementoModalJson(m.emod_id);
                                            }
                                        }
                                    }
                                    //Eliminar Elementos  Modales por Seccion
                                    var elemModalTupla = intanetElementoModalbl.IntranetElementoModalEliminarxSeccionElementoJson(fk_seccion_elemento);
                                    //eliminar Seccion Elemento
                                    var secElementoTupla = intranetSeccionElementobl.IntranetSeccionElementoEliminarJson(fk_seccion_elemento);
                                }
                                //eliminar Imagenes si hubiera
                                if (detalleElementoTupla.intranetDetalleElemento.detel_extension != "")
                                {
                                 
                                    var direccion = Server.MapPath("/") + Request.ApplicationPath + "/IntranetFiles/";
                                    rutaEliminar = Path.Combine(direccion + detalleElementoTupla.intranetDetalleElemento.detel_nombre + "." + detalleElementoTupla.intranetDetalleElemento.detel_extension);
                                    if (System.IO.File.Exists(rutaEliminar))
                                    {
                                        System.IO.File.Delete(rutaEliminar);
                                    }
                                }
                                //Eliminar Detalle de Elemento
                                var detElementoEliminado = intranetDetalleElementonbl.IntranetDetalleElementoEliminarJson(j.detel_id);
                            }
                        }
                    }
                }



                var IntranetElementoTupla = intranetElementobl.IntranetElementoEliminarJson(elem_id);
                error = IntranetElementoTupla.error;
                if (error.Respuesta)
                {
                    respuestaConsulta = IntranetElementoTupla.intranetElementoEliminado;
                    errormensaje = "Elemento Eliminado";
                }
                else
                {
                    errormensaje = "Error, no se Puede Eliminar";
                    mensajeConsola = error.Mensaje;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetElementoIdObtenerJson(int elem_id) {
            string errormensaje = "";
            bool response = false;
            claseError error = new claseError();
            IntranetElementoEntidad intranetElemento = new IntranetElementoEntidad();
            try
            {
                var intranetElementoTupla = intranetElementobl.IntranetElementoIdObtenerJson(elem_id);
                error = intranetElementoTupla.error;
                if (error.Respuesta)
                {
                    intranetElemento = intranetElementoTupla.intranetElemento;
                    errormensaje = "Obteniendo Data";
                    response = true;
                }
                else {
                    errormensaje = error.Mensaje;
                }
            }
            catch (Exception ex) {
                errormensaje = ex.Message+ "Consulte con Administrador";
            }
            return Json(new {respuesta=response,mensaje=errormensaje,data=intranetElemento });
        }
        [HttpPost]
        public ActionResult IntranetElementoEditarJson(IntranetElementoEntidad intranetElemento) {
            string errormensaje = "";
            bool response = false;
            claseError error = new claseError();
            try {
                intranetElemento.elem_descripcion = intranetElemento.elem_titulo;
                intranetElemento.elem_contenido = intranetElemento.elem_titulo;
                var intranetElementoTupla = intranetElementobl.IntranetElementoEditarJson(intranetElemento);
                error = intranetElementoTupla.error;
                if (error.Respuesta)
                {
                    response = intranetElementoTupla.intranetElementoEditado;
                    errormensaje = "Elemento Editado";
                }
                else {
                    errormensaje = error.Mensaje;
                }

            }
            catch (Exception ex)
            {
                errormensaje = ex.Message;
            }
            return Json(new { respuesta = response, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult IntranetElementoEditarOrdenJson(IntranetElementoEntidad[] arrayElementos)
        {
            IntranetElementoEntidad intranetElemento = new IntranetElementoEntidad();
            claseError error = new claseError();
            bool response = false;
            string errormensaje = "";
            int tamanio = arrayElementos.Length;
            foreach (var m in arrayElementos)
            {
                intranetElemento.elem_id = m.elem_id;
                intranetElemento.elem_orden = m.elem_orden;
                var reordenadoTupla = intranetElementobl.IntranetElementoEditarOrdenJson(intranetElemento);
                error = reordenadoTupla.error;
                if (error.Respuesta)
                {
                    response = reordenadoTupla.intranetElementoReordenado;
                    errormensaje = "Editado";
                }
                else
                {
                    response = false;
                    errormensaje = "No se Pudo Editar";
                    return Json(new { respuesta = response, mensaje = errormensaje, mensajeconsola = "" });
                }
            }
            return Json(new { tamanioelemento = tamanio, respuesta = response, mensaje = errormensaje, mensajeconsola = "" });
        }
    }
}