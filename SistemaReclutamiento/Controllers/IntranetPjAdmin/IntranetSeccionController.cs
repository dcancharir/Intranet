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
    public class IntranetSeccionController : Controller
    {
        // GET: IntranetSeccion
        IntranetSeccionModel intranetSeccionbl = new IntranetSeccionModel();
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
        [HttpPost]
        public ActionResult IntranetSeccionListarxMenuIDJson(int menu_id)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetSeccionEntidad> listaMenus = new List<IntranetSeccionEntidad>();
            claseError error = new claseError();
            try
            {
                var seccionTupla = intranetSeccionbl.IntranetSeccionListarxMenuIDJson( menu_id);
                error = seccionTupla.error;
                listaMenus = seccionTupla.intranetSeccionListaxMenuID;
                if (error.Respuesta)
                {
                    mensaje = "Listando Secciones";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Mensaje;
                    mensaje = "No se Pudieron Listar las Secciones";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaMenus.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetSeccionListarTodoxMenuIDJson(int menu_id)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetSeccionEntidad> listaSecciones = new List<IntranetSeccionEntidad>();
            claseError error = new claseError();
            try
            {
                var seccionTupla = intranetSeccionbl.IntranetSeccionListarTodoxMenuIDJson(menu_id);
                error = seccionTupla.error;
                listaSecciones = seccionTupla.intranetSeccionListaTodoxMenuID;
                if (error.Respuesta)
                {
                    mensaje = "Listando Secciones";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Mensaje;
                    mensaje = "No se Pudieron Listar las Secciones";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaSecciones.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetSeccionEditarJson(IntranetSeccionEntidad intranetSeccion)
        {
            string errormensaje = "";
            bool respuestaConsulta = false;
            string mensajeConsola = "";
            claseError error = new claseError();
            try
            {
                var seccionTupla = intranetSeccionbl.IntranetSeccionEditarEstadoJson(intranetSeccion);
                error = seccionTupla.error;
                if (error.Respuesta)
                {
                    respuestaConsulta = seccionTupla.intranetSeccionEditado;
                    errormensaje = "Se Editó Correctamente";
                }
                else
                {
                    mensajeConsola = error.Mensaje;
                    errormensaje = "Error, no se Puede Editar";
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetSeccionInsertarJson(IntranetSeccionEntidad intranetSeccion)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            int idIntranetSeccionInsertado = 0;
            claseError error = new claseError();
            try
            {
                var totalSeccionesTupla = intranetSeccionbl.IntranetSeccionListarTodoxMenuIDJson(intranetSeccion.fk_menu);
                error = totalSeccionesTupla.error;
                if (error.Respuesta) {
                    intranetSeccion.sec_orden = totalSeccionesTupla.intranetSeccionListaTodoxMenuID.Max(x=>x.sec_orden) + 1;
                    var seccionTupla = intranetSeccionbl.IntranetSeccionInsertarJson(intranetSeccion);
                    error = seccionTupla.error;

                    if (error.Respuesta)
                    {
                        mensaje = "Se Registró Correctamente";
                        respuesta = true;
                        idIntranetSeccionInsertado = seccionTupla.idIntranetSeccionInsertado;
                    }
                    else
                    {
                        mensaje = "No se Pudo insertar el Menu";
                        mensajeConsola = error.Mensaje;
                    }
                }
                else
                {
                    mensaje = "Error al Insertar la Nueva Seccion";
                    mensajeConsola = error.Mensaje;
                }
                

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje, idIntranetMenuInsertado = idIntranetSeccionInsertado, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetSeccionEliminarJson(int sec_id)
        {
            string errormensaje = "";
            bool respuestaConsulta = false;
            claseError error = new claseError();
            string mensajeConsola = "";
            List<IntranetDetalleElementoModalEntidad> listaDetalleElementoModal = new List<IntranetDetalleElementoModalEntidad>();
            List<IntranetElementoModalEntidad> listaElementoModal = new List<IntranetElementoModalEntidad>();
            IntranetSeccionElementoEntidad seccionElemento = new IntranetSeccionElementoEntidad();
            List<IntranetDetalleElementoEntidad> listaDetalleElemento = new List<IntranetDetalleElementoEntidad>();
            string rutaEliminar = "";
            try
            {
                var listaElementoTupla = intranetElementobl.IntranetElementoListarxSeccionIDJson(sec_id);
                if (listaElementoTupla.error.Respuesta) {
                    foreach (var elemento in listaElementoTupla.intranetElementoListaxSeccionID) {
                        //Buscar los Detalles que pudiera tener
                        var detalleElementoTupla2 = intranetDetalleElementonbl.IntranetDetalleElementoListarxElementoIDJson(elemento.elem_id);

                        if (detalleElementoTupla2.error.Respuesta)
                        {
                            listaDetalleElemento = detalleElementoTupla2.intranetDetalleElementoListaxElementoID;
                            if (listaDetalleElemento.Count > 0)
                            {
                                foreach (var j in listaDetalleElemento)
                                {
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
                        //Eliminar Elementos
                        var elementoEliminado = intranetElementobl.IntranetElementoEliminarJson(elemento.elem_id);
                    }
                }
                var seccionTupla = intranetSeccionbl.IntranetSeccionEliminarJson(sec_id);
                error = seccionTupla.error;
                if (error.Respuesta)
                {
                    respuestaConsulta = seccionTupla.intranetSeccionEliminado;
                    errormensaje = "Seccion Eliminada";
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
        public ActionResult IntranetSeccionEditarOrdenJson(IntranetSeccionEntidad[] arraySecciones)
        {
            IntranetSeccionEntidad intranetSeccion = new IntranetSeccionEntidad();
            claseError error = new claseError();
            bool response = false;
            string errormensaje = "";
            int tamanio = arraySecciones.Length;
            foreach (var m in arraySecciones)
            {
                intranetSeccion.sec_id = m.sec_id;
                intranetSeccion.sec_orden = m.sec_orden;
                var reordenadoTupla = intranetSeccionbl.IntranetSeccionEditarOrdenJson(intranetSeccion);
                error = reordenadoTupla.error;
                if (error.Respuesta)
                {
                    response = reordenadoTupla.intranetSeccionReordenado;
                    errormensaje = "Editado";
                }
                else
                {
                    response = false;
                    errormensaje = "No se Pudo Editar";
                    return Json(new { respuesta = response, mensaje = errormensaje, mensajeconsola = "" });
                }
            }
            return Json(new { tamanioseccion = tamanio, respuesta = response, mensaje = errormensaje, mensajeconsola = "" });
        }

    }
}