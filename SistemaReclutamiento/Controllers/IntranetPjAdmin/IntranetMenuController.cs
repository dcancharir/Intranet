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

namespace SistemaReclutamiento.Controllers.IntranetPJAdmin
{
    [autorizacion(false)]
    public class IntranetMenuController : Controller
    {
        IntranetMenuModel intranetMenubl = new IntranetMenuModel();
        IntranetSeccionModel intranetSeccionbl = new IntranetSeccionModel();
        claseError error = new claseError();

        IntranetElementoModel intranetElementobl = new IntranetElementoModel();
        IntranetDetalleElementoModel intranetDetalleElementonbl = new IntranetDetalleElementoModel();
        IntranetSeccionElementoModel intranetSeccionElementobl = new IntranetSeccionElementoModel();
        IntranetElementoModalModel intanetElementoModalbl = new IntranetElementoModalModel();
        IntranetDetalleElementoModalModel intranetDetalleElementoModalbl = new IntranetDetalleElementoModalModel();
        string pathArchivosIntranet = ConfigurationManager.AppSettings["PathArchivosIntranet"].ToString();
        RutaImagenes rutaImagenes = new RutaImagenes();
        // GET: IntranetMenu
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IntranetMenuListarJson()
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetMenuEntidad> listaMenus = new List<IntranetMenuEntidad>();
            try
            {
                var menuTupla = intranetMenubl.IntranetMenuListarJson();
                error = menuTupla.error;
                listaMenus = menuTupla.intranetMenuLista;
                if (error.Respuesta)
                {
                    mensaje = "Listando Menus";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Mensaje;
                    mensaje = "No se Pudieron Listar los Menus";
                }
                
            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaMenus.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola=mensajeConsola });
        }

        [HttpPost]
        public ActionResult IntranetMenuSeccionListarJson()
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetMenuEntidad> listaMenus = new List<IntranetMenuEntidad>();
            List<IntranetSeccionEntidad> listaSecciones = new List<IntranetSeccionEntidad>();
            var datos = new List<dynamic>();
            try
            {
                var menuTupla = intranetMenubl.IntranetMenuListarJson();
                error = menuTupla.error;
                listaMenus = menuTupla.intranetMenuLista;
                if (error.Respuesta)
                {
                    mensaje = "Listando Menus";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Mensaje;
                    mensaje = "No se Pudieron Listar los Menus";
                }

                foreach (var term in listaMenus)
                {
                    var seccionTupla = intranetSeccionbl.IntranetSeccionListarTodoxMenuIDJson(term.menu_id);
                    error = seccionTupla.error;
                    listaSecciones = seccionTupla.intranetSeccionListaTodoxMenuID;
                    datos.Add(new
                    {
                        term.menu_id,
                        term.menu_orden,
                        term.menu_titulo,
                        secciones = listaSecciones
                    }); ;
                }


            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = datos.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }


        [HttpPost]
        public ActionResult IntranetMenuListarTodoJson()
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetMenuEntidad> listaMenus = new List<IntranetMenuEntidad>();
            try
            {
                var menuTupla = intranetMenubl.IntranetMenuListarTodoJson();
                error = menuTupla.error;
                listaMenus = menuTupla.intranetMenuLista;
                if (error.Respuesta)
                {
                    mensaje = "Listando Menus";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Mensaje;
                    mensaje = "No se Pudieron Listar los Menus";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaMenus.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetMenuIdObtenerJson(int menu_id)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            IntranetMenuEntidad menu = new IntranetMenuEntidad();
            try
            {
                var menuTupla = intranetMenubl.IntranetMenuIdObtenerJson(menu_id);
                error = menuTupla.error;
                menu = menuTupla.intranetMenu;
                if (error.Respuesta)
                {
                    mensaje = "Obteniendo Informacion del Menu Seleccionado";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Mensaje;
                    mensaje = "No se Pudo Obtener La Informacionb del Menu Seleccionado";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = menu, respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetMenuNuevoJson(IntranetMenuEntidad intranetMenu)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            int idIntranetMenuInsertado=0;
            int totalRegistros = 0;
            claseError error = new claseError();
            try
            {
                var listaMenusTupla = intranetMenubl.IntranetMenuListarTodoJson();
                if (listaMenusTupla.error.Respuesta) {
                    totalRegistros = listaMenusTupla.intranetMenuLista.Max(x => x.menu_orden);
                    intranetMenu.menu_orden = totalRegistros + 1;
                    var menuTupla = intranetMenubl.IntranetMenuInsertarJson(intranetMenu);
                    error = menuTupla.error;

                    if (error.Respuesta)
                    {
                        mensaje = "Se Registró Correctamente";
                        respuesta = true;
                        idIntranetMenuInsertado = menuTupla.idIntranetMenuInsertado;
                    }
                    else
                    {
                        mensaje = "No se Pudo insertar el Menu";
                        mensajeConsola = error.Mensaje;
                    }
                }
                else
                {
                    mensaje = "No se Pudo insertar el Menu";
                    mensajeConsola = error.Mensaje;
                }
                //var totalMenuTupla = intranetMenubl.IntranetMenuObtenerTotalRegistrosJson();
                //error = totalMenuTupla.error;
                //if (error.Respuesta)
                //{
                //    totalRegistros = totalMenuTupla.intranetMenuTotal;
                //    intranetMenu.menu_orden = totalRegistros + 1;
                //    var menuTupla = intranetMenubl.IntranetMenuInsertarJson(intranetMenu);
                //    error = menuTupla.error;

                //    if (error.Respuesta)
                //    {
                //        mensaje = "Se Registró Correctamente";
                //        respuesta = true;
                //        idIntranetMenuInsertado = menuTupla.idIntranetMenuInsertado;
                //    }
                //    else
                //    {
                //        mensaje = "No se Pudo insertar el Menu";
                //        mensajeConsola = error.Mensaje;
                //    }
                //}
                //else
                //{
                //    mensaje = "No se Pudo insertar el Menu";
                //    mensajeConsola = error.Mensaje;
                //}


            }
            catch (Exception exp)
            {
                mensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje,mensajeconsola=mensajeConsola});
        }
        [HttpPost]
        public ActionResult IntranetMenuEditarJson(IntranetMenuEntidad intranetMenu)
        {
            string errormensaje = "";
            bool respuestaConsulta = false;
            string mensajeConsola = "";
            claseError error = new claseError();
            if(intranetMenu.menu_id>0){
                try
                {
                    var menuTupla = intranetMenubl.IntranetMenuEditarJson(intranetMenu);
                    error = menuTupla.error;
                    if (error.Respuesta)
                    {
                        respuestaConsulta = menuTupla.intranetMenuEditado;
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
            }
            else{
                errormensaje = "Error, falta Menu ID";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje,mensajeconsola=mensajeConsola });
        }

        [HttpPost]
        public ActionResult IntranetMenuEliminarJson(int menu_id)
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
                var listaSeccionesTupla = intranetSeccionbl.IntranetSeccionListarxMenuIDJson(menu_id);
                if (listaSeccionesTupla.error.Respuesta) {
                    foreach (var seccion in listaSeccionesTupla.intranetSeccionListaxMenuID) {
                        var listaElementoTupla = intranetElementobl.IntranetElementoListarxSeccionIDJson(seccion.sec_id);
                        if (listaElementoTupla.error.Respuesta)
                        {
                            foreach (var elemento in listaElementoTupla.intranetElementoListaxSeccionID)
                            {
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
                        //Eliminar Seccion
                        var seccionTupla = intranetSeccionbl.IntranetSeccionEliminarJson(seccion.sec_id);
                    }
                }
                var menuTupla = intranetMenubl.IntranetMenuEliminarJson(menu_id);
                error = menuTupla.error;
                if (error.Respuesta)
                {
                    respuestaConsulta = menuTupla.intranetMenuEliminado;
                    errormensaje = "Menu Eliminado";
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
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje,mensajeconsola=mensajeConsola });
        }

        [HttpPost]
        public ActionResult IntranetMenuEliminarVariosJson(int[] listaMenuEliminar) {
            string errormensaje = "";
            string mensajeConsola = "";
            bool respuestaConsulta = false;
            claseError error = new claseError();
            List<IntranetDetalleElementoModalEntidad> listaDetalleElementoModal = new List<IntranetDetalleElementoModalEntidad>();
            List<IntranetElementoModalEntidad> listaElementoModal = new List<IntranetElementoModalEntidad>();
            IntranetSeccionElementoEntidad seccionElemento = new IntranetSeccionElementoEntidad();
            List<IntranetDetalleElementoEntidad> listaDetalleElemento = new List<IntranetDetalleElementoEntidad>();
            string rutaEliminar = "";
            try
            {
                for (int i = 0; i <= listaMenuEliminar.Length - 1; i++) {

                    var listaSeccionesTupla = intranetSeccionbl.IntranetSeccionListarxMenuIDJson(listaMenuEliminar[i]);
                    if (listaSeccionesTupla.error.Respuesta)
                    {
                        foreach (var seccion in listaSeccionesTupla.intranetSeccionListaxMenuID)
                        {
                            var listaElementoTupla = intranetElementobl.IntranetElementoListarxSeccionIDJson(seccion.sec_id);
                            if (listaElementoTupla.error.Respuesta)
                            {
                                foreach (var elemento in listaElementoTupla.intranetElementoListaxSeccionID)
                                {
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
                                                                                    
                                                                                    var direccion = Server.MapPath("/") + Request.ApplicationPath + "/IntranetFiles/";
                                                                                    rutaEliminar = Path.Combine(direccion + k.detelm_nombre + "." + k.detelm_extension);
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
                            //Eliminar Seccion
                            var seccionTupla = intranetSeccionbl.IntranetSeccionEliminarJson(seccion.sec_id);
                        }
                    }


                    var menuTupla = intranetMenubl.IntranetMenuEliminarJson(listaMenuEliminar[i]);

                    error = menuTupla.error;
                    if (error.Respuesta)
                    {
                        respuestaConsulta = menuTupla.intranetMenuEliminado;
                        errormensaje = "Menus Eliminados";
                    }
                    else
                    {
                        errormensaje = "Error, no se Puede Eliminar";
                        mensajeConsola = error.Mensaje;
                    }
                }
                respuestaConsulta = true;
            }
            catch (Exception ex) {
                errormensaje = "Error, no se Puede Eliminar, " + ex.Message;
                respuestaConsulta = false;
            }

            return Json(new { respuesta = respuestaConsulta , mensaje = errormensaje, mensajeconsola = mensajeConsola });
        }

        [HttpPost]
        public ActionResult IntranetMenuEditarOrdenJson(IntranetMenuEntidad[] arrayMenus) {
            IntranetMenuEntidad intranetMenu = new IntranetMenuEntidad();
            claseError error = new claseError();
            bool response = false;
            string errormensaje = "";
            int tamanio = arrayMenus.Length;
            foreach (var m in arrayMenus) {
                intranetMenu.menu_id = m.menu_id;
                intranetMenu.menu_orden = m.menu_orden;
                var reordenadoTupla = intranetMenubl.IntranetMenuEditarOrdenJson(intranetMenu);
                error = reordenadoTupla.error;
                if (error.Respuesta) {
                    response = reordenadoTupla.intranetMenuReordenado;
                    errormensaje = "Editado";
                }
                else
                {
                    response = false;
                    errormensaje = "No se Pudo Editar";
                    return Json(new { respuesta = response, mensaje =errormensaje , mensajeconsola = "" });
                }
            }
            return Json(new {tamaniomenu= tamanio,respuesta = response, mensaje = errormensaje, mensajeconsola = "" });
        }
    }
}