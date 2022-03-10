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
    public class IntranetDetalleElementoModalController : Controller
    {
        // GET: IntranetDetalleElementoModal
        IntranetDetalleElementoModalModel intranetDetalleElementoModalbl = new IntranetDetalleElementoModalModel();
        string pathArchivosIntranet = ConfigurationManager.AppSettings["PathArchivosIntranet"].ToString();
        claseError error = new claseError();
        RutaImagenes rutaImagenes = new RutaImagenes();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IntranetDetalleElementoModalListarxElementoModalJson(int fk_elemento_modal)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetDetalleElementoModalEntidad> listaElementos = new List<IntranetDetalleElementoModalEntidad>();
            claseError error = new claseError();
            try
            {
                var ElementoTupla = intranetDetalleElementoModalbl.IntranetDetalleElementoModalListarxElementoIDJson(fk_elemento_modal);
                error = ElementoTupla.error;
                listaElementos = ElementoTupla.intranetDetalleElementoModalListaxElementoID;
                if (error.Respuesta)
                {
                    mensaje = "Listando Detalle Elemento Modal";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Mensaje;
                    mensaje = "No se Pudieron Listar los Detalles";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaElementos.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetDetalleElementoModalInsertarJson(IntranetDetalleElementoModalEntidad intranetDetalleElementoModal)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            int idIntranetDetalleElementoModalInsertado = 0;
            int tamanioMaximo = 0;
            string extension = "";
            string rutaInsertar = "";

            IntranetSeccionElementoEntidad intranetSeccionElemento = new IntranetSeccionElementoEntidad();
            claseError error = new claseError();

            //verificar si es imagen o abre un modal si fk_seccion_elemento==0 es imagen
            if (intranetDetalleElementoModal.fk_seccion_elemento == 0)
            {
                var totalDetallesTupla = intranetDetalleElementoModalbl.IntranetDetalleElementoModalListarxElementoIDJson(intranetDetalleElementoModal.fk_elemento_modal);
                if (totalDetallesTupla.error.Respuesta)
                {
                    if (totalDetallesTupla.intranetDetalleElementoModalListaxElementoID.Count > 0)
                    {
                        intranetDetalleElementoModal.detelm_orden = totalDetallesTupla.intranetDetalleElementoModalListaxElementoID.Max(x => x.detelm_orden) + 1;

                    }
                    else {
                        intranetDetalleElementoModal.detelm_orden = 1;
                    }
                    HttpPostedFileBase file = Request.Files[0];
                    tamanioMaximo = 4194304;
                    if (file.ContentLength > 0 && file != null)
                    {
                        if (file.ContentLength <= tamanioMaximo)
                        {
                            extension = Path.GetExtension(file.FileName).ToLower(); ;
                            if (extension == ".jpg" || extension == ".png" || extension == ".jpeg")
                            {
                                string nombreArchivo = ("ElementoModal_" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                                var nombre = (nombreArchivo + extension);
                                var direccion = Server.MapPath("/") + Request.ApplicationPath + "/IntranetFiles/";
                                rutaInsertar = Path.Combine(direccion, nombre);
                                if (!Directory.Exists(direccion))
                                {
                                    System.IO.Directory.CreateDirectory(direccion);
                                }
                                file.SaveAs(rutaInsertar);
                                //intranetDetalleElementoModal.detelm_hash = rutaImagenes.ImagenIntranetActividades(pathArchivosIntranet, nombre);
                                intranetDetalleElementoModal.detelm_nombre = nombreArchivo;
                                //intranetDetalleElementoModal.detelm_extension = (extension == ".jpg" ? "jpg" : "png");
                                if (extension == ".jpg")
                                {
                                    intranetDetalleElementoModal.detelm_extension = "jpg";
                                }
                                else if (extension == ".png")
                                {
                                    intranetDetalleElementoModal.detelm_extension = "png";
                                }
                                else
                                {
                                    intranetDetalleElementoModal.detelm_extension = "jpeg";
                                }
                            }
                            else
                            {
                                mensaje = "Solo se admiten archivos jpg o png.";
                                respuesta = false;
                                return Json(new { respuesta = respuesta, mensaje = mensaje });
                            }
                        }
                        else
                        {
                            mensaje = "El tamaño maximo de arhivo permitido es de 4Mb.";
                            respuesta = false;
                            return Json(new { respuesta = respuesta, mensaje = mensaje });
                        }

                    }
                    else
                    {
                        mensaje = "Error al Subir el Archivo.";
                        respuesta = false;
                        return Json(new { respuesta = respuesta, mensaje = mensaje });
                    }
                }
                else {
                    mensaje = totalDetallesTupla.error.Mensaje;
                    respuesta = false;
                    return Json(new { respuesta, mensaje });
                }
            }
            //Lista de Texto
            else if (intranetDetalleElementoModal.fk_seccion_elemento == 1)
            {
                try
                {
                    var totalDetallesTupla = intranetDetalleElementoModalbl.IntranetDetalleElementoModalListarxElementoIDJson(intranetDetalleElementoModal.fk_elemento_modal);
                    if (totalDetallesTupla.error.Respuesta)
                    {
                        if (totalDetallesTupla.intranetDetalleElementoModalListaxElementoID.Count > 0)
                        {
                            intranetDetalleElementoModal.detelm_orden = totalDetallesTupla.intranetDetalleElementoModalListaxElementoID.Max(x => x.detelm_orden) + 1;

                        }
                        else
                        {
                            intranetDetalleElementoModal.detelm_orden = 1;
                        }
                        //Insertar Imagen si esque hubiera
                        if (intranetDetalleElementoModal.detelm_nombre != "" && intranetDetalleElementoModal.detelm_nombre != null)
                        {

                            HttpPostedFileBase file = Request.Files[0];
                            tamanioMaximo = 4194304;
                            if (file.ContentLength > 0 && file != null)
                            {
                                if (file.ContentLength <= tamanioMaximo)
                                {
                                    extension = Path.GetExtension(file.FileName).ToLower() ;
                                    if (extension == ".jpg" || extension == ".png" || extension == ".jpeg")
                                    {
                                        string nombreArchivo = ("ElementoModal_" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                                        var nombre = (nombreArchivo + extension);
                                        var direccion = Server.MapPath("/") + Request.ApplicationPath + "/IntranetFiles/";
                                
                                        rutaInsertar = Path.Combine(direccion, nombre);

                                        if (!Directory.Exists(direccion))
                                        {
                                            System.IO.Directory.CreateDirectory(direccion);
                                        }

                                        file.SaveAs(rutaInsertar);
                                        //intranetDetalleElementoModal.detelm_hash = rutaImagenes.ImagenIntranetActividades(pathArchivosIntranet, nombre);
                                        intranetDetalleElementoModal.detelm_nombre = nombreArchivo;

                                        if (extension == ".jpg")
                                        {
                                            intranetDetalleElementoModal.detelm_extension = "jpg";
                                        }
                                        else if (extension == ".png")
                                        {
                                            intranetDetalleElementoModal.detelm_extension = "png";
                                        }
                                        else
                                        {
                                            intranetDetalleElementoModal.detelm_extension = "jpeg";
                                        }
                                    }
                                    else
                                    {
                                        mensaje = "Solo se admiten archivos jpg o png.";
                                        respuesta = false;
                                        return Json(new { respuesta = respuesta, mensaje = mensaje });
                                    }
                                }
                                else
                                {
                                    mensaje = "El tamaño maximo de arhivo permitido es de 4Mb.";
                                    respuesta = false;
                                    return Json(new { respuesta = respuesta, mensaje = mensaje });
                                }

                            }
                            else
                            {
                                mensaje = "Error al Subir el Archivo.";
                                respuesta = false;
                                return Json(new { respuesta = respuesta, mensaje = mensaje });
                            }
                        }

                    }
                    else
                    {
                        mensaje = totalDetallesTupla.error.Mensaje;
                        respuesta = false;
                        return Json(new { respuesta = respuesta, mensaje = mensaje });
                    }

                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                    respuesta = false;
                    return Json(new { respuesta = respuesta, mensaje = mensaje });
                }
            }
            else {
                var totalDetallesTupla = intranetDetalleElementoModalbl.IntranetDetalleElementoModalListarxElementoIDJson(intranetDetalleElementoModal.fk_elemento_modal);
                if (totalDetallesTupla.error.Respuesta)
                {
                    if (totalDetallesTupla.intranetDetalleElementoModalListaxElementoID.Count > 0)
                    {
                        intranetDetalleElementoModal.detelm_orden = totalDetallesTupla.intranetDetalleElementoModalListaxElementoID.Max(x => x.detelm_orden) + 1;

                    }
                    else {
                        intranetDetalleElementoModal.detelm_orden = 1;
                    }
                }
                else {
                    mensaje = totalDetallesTupla.error.Mensaje;
                    respuesta = false;
                    return Json(new { respuesta = respuesta, mensaje = mensaje });
                }
            }
            try
            {

                var DetalleElementoModalTupla = intranetDetalleElementoModalbl.IntranetDetalleElementoModalInsertarJson(intranetDetalleElementoModal);
                error = DetalleElementoModalTupla.error;

                if (error.Respuesta)
                {
                    mensaje = "Se Registró Correctamente";
                    respuesta = true;
                    idIntranetDetalleElementoModalInsertado = DetalleElementoModalTupla.idIntranetDetalleElementoModalInsertado;
                }
                else
                {
                    mensaje = "No se Pudo insertar el Detalle";
                    mensajeConsola = error.Mensaje;
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje, idIntranetDetalleElementoInsertado = idIntranetDetalleElementoModalInsertado, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetDetalleElementoModalEliminarJson(int detelm_id)
        {
            string mensaje = "";
            bool respuesta = false;
            claseError error = new claseError();
              String rutaEliminar="";
            try
            {
                //buscar Imagen para eliminar
                var imagenTupla=intranetDetalleElementoModalbl.IntranetDetalleElementoModalIdObtenerJson(detelm_id);
                if(imagenTupla.error.Respuesta){
                    //var nombre = (nombreArchivo + extension);
                    var direccion = Server.MapPath("/") + Request.ApplicationPath + "/IntranetFiles/";
                    rutaEliminar = Path.Combine(direccion, imagenTupla.intranetDetalleElementoModal.detelm_nombre+"."+imagenTupla.intranetDetalleElementoModal.detelm_extension);
                        if (System.IO.File.Exists(rutaEliminar))
                    {
                        System.IO.File.Delete(rutaEliminar);
                    }
                }
                //Eliminar de BD
                var intranetDetalleElementoModalTupla = intranetDetalleElementoModalbl.IntranetDetalleElementoModalEliminarJson(detelm_id);
                error = intranetDetalleElementoModalTupla.error;
                if (error.Respuesta)
                {
                    respuesta = intranetDetalleElementoModalTupla.intranetDetalleElementoModalEliminado;
                    mensaje = "Eliminado";
                }
                else
                {
                    mensaje = error.Mensaje;
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { respuesta = respuesta, mensaje = mensaje });
        }
        [HttpPost]
        public ActionResult IntranetDetalleElementoModalIdObtenerJson(int detelm_id) {
            string mensaje = "";
            bool response = false;
            IntranetDetalleElementoModalEntidad intranetDetalleElementoModal = new IntranetDetalleElementoModalEntidad();
            claseError error = new claseError();
            try
            {
                var intranetDetalleElementoModalTupla = intranetDetalleElementoModalbl.IntranetDetalleElementoModalIdObtenerJson(detelm_id);
                error = intranetDetalleElementoModalTupla.error;
                if (error.Respuesta)
                {
                    intranetDetalleElementoModal = intranetDetalleElementoModalTupla.intranetDetalleElementoModal;
                    //intranetDetalleElementoModal.detelm_nombre_imagen = intranetDetalleElementoModal.detelm_hash;
                    //intranetDetalleElementoModal.detelm_nombre_imagen = rutaImagenes.ImagenIntranetActividades(pathArchivosIntranet, intranetDetalleElementoModal.detelm_nombre + "." + intranetDetalleElementoModal.detelm_extension);
                    intranetDetalleElementoModal.detelm_nombre_imagen = intranetDetalleElementoModal.detelm_nombre + "." + intranetDetalleElementoModal.detelm_extension;
                    response = true;
                    mensaje = "Obteniendo Data";
                }
                else {
                    mensaje = error.Mensaje;
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new{ data= intranetDetalleElementoModal ,respuesta=response,mensaje=mensaje });
        }
        [HttpPost]
        public ActionResult IntranetDetalleElementoModalEditarJson(IntranetDetalleElementoModalEntidad intranetDetalleElementoModal) {
            HttpPostedFileBase file = Request.Files[0];
            int tamanioMaximo = 4194304;
            string extension = "";
            string errormensaje = "";
            bool response = false;
            string rutaInsertar = "";
            string rutaAnterior = "";
            try
            {
                //Es imagen
                if (intranetDetalleElementoModal.detelm_nombre_imagen != "" && intranetDetalleElementoModal.detelm_nombre_imagen != null)
                {
                    //selecciono una imagen para editar
                    if (file.ContentLength > 0 && file != null)
                    {
                        if (file.ContentLength <= tamanioMaximo)
                        {
                            extension = Path.GetExtension(file.FileName).ToLower();
                            if (extension == ".jpg" || extension == ".png" || extension == ".jpeg")
                            {
                                string nombreArchivo = ("ElementoModal_" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                                var nombre = (nombreArchivo + extension);
                                var direccion = Server.MapPath("/") + Request.ApplicationPath + "/IntranetFiles/";

                                rutaInsertar = Path.Combine(direccion, nombre);
                                rutaAnterior = Path.Combine(direccion, intranetDetalleElementoModal.detelm_nombre_imagen);
                                if (!Directory.Exists(direccion))
                                {
                                    System.IO.Directory.CreateDirectory(direccion);
                                }
                                if (System.IO.File.Exists(rutaAnterior))
                                {
                                    System.IO.File.Delete(rutaAnterior);
                                }
                                file.SaveAs(rutaInsertar);
                                //intranetDetalleElementoModal.detelm_hash = rutaImagenes.ImagenIntranetActividades(pathArchivosIntranet, nombre);
                                intranetDetalleElementoModal.detelm_nombre = nombreArchivo;
                                extension = extension.Replace(".", "");
                                intranetDetalleElementoModal.detelm_extension = extension;
                            }
                        }
                        else
                        {
                            errormensaje = "El archivo supera el tamaño maximo permitido";
                            return Json(new { respuesta = false, mensaje = errormensaje });
                        }
                    }
                    else {
                        //intranetDetalleElementoModal.detelm_hash = rutaImagenes.ImagenIntranetActividades(pathArchivosIntranet, intranetDetalleElementoModal.detelm_nombre_imagen);
                        string[] word = intranetDetalleElementoModal.detelm_nombre_imagen.Split('.');
                        intranetDetalleElementoModal.detelm_nombre = word[0];
                        intranetDetalleElementoModal.detelm_extension = word[1];
                    }
                }
                if (intranetDetalleElementoModal.fk_seccion_elemento == 2)
                {
                    intranetDetalleElementoModal.fk_seccion_elemento = 0;
                }
                intranetDetalleElementoModal.detelm_ubicacion = "";
                var intranetDetalleElementotupla = intranetDetalleElementoModalbl.IntranetDetalleElementoModalEditarJson(intranetDetalleElementoModal);
                error = intranetDetalleElementotupla.error;
                if (error.Respuesta)
                {
                    response = intranetDetalleElementotupla.intranetDetalleElementoModalEditado;
                    errormensaje = "Editado";
                }
                else
                {
                    errormensaje = error.Mensaje;
                }
            }
            catch (Exception ex) {
                errormensaje = ex.Message;
            }
            return Json(new { data=intranetDetalleElementoModal,mensaje = errormensaje, respuesta = response });
        }
        [HttpPost]
        public ActionResult IntranetDetalleelementoModalEditarOrdenJson(IntranetDetalleElementoModalEntidad[] arrayDetElementoModal)
        {
            IntranetDetalleElementoModalEntidad intranetDetElementoModal= new IntranetDetalleElementoModalEntidad();
            claseError error = new claseError();
            bool response = false;
            string errormensaje = "";
            int tamanio = arrayDetElementoModal.Length;
            foreach (var m in arrayDetElementoModal)
            {
                intranetDetElementoModal.detelm_id = m.detelm_id;
                intranetDetElementoModal.detelm_orden = m.detelm_orden;
                var reordenadoTupla = intranetDetalleElementoModalbl.IntranetDetalleElementoModalEditarOrdenJson(intranetDetElementoModal);
                error = reordenadoTupla.error;
                if (error.Respuesta)
                {
                    response = reordenadoTupla.intranetDetElementoModalReordenado;
                    errormensaje = "Editado";
                }
                else
                {
                    response = false;
                    errormensaje = "No se Pudo Editar";
                    return Json(new { respuesta = response, mensaje = errormensaje, mensajeconsola = "" });
                }
            }
            return Json(new { tamaniodetalleelementomodal = tamanio, respuesta = response, mensaje = errormensaje, mensajeconsola = "" });
        }
    }
}