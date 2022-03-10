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
using System.Web.Script.Serialization;

namespace SistemaReclutamiento.Controllers.IntranetPjAdmin
{
    [autorizacion(false)]
    public class IntranetFooterController : Controller
    {
        // GET: IntranetFooter
        IntranetFooterModel intranetFooterbl = new IntranetFooterModel();
        string PathActividadesIntranet = ConfigurationManager.AppSettings["PathArchivosIntranet"].ToString();
        RutaImagenes rutaImagenes = new RutaImagenes();

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IntranetFooterInsertarJson(IntranetFooterEntidad intranetFooter) {
            claseError error = new claseError();
            HttpPostedFileBase file = Request.Files[0];
            IntranetFooterEntidad footer = new IntranetFooterEntidad();
            int tamanioMaximo = 4194304;
            string extension = "";
            string rutaInsertar = "";
            string errormensaje = "";
            bool response = false;
            var direccion = Server.MapPath("/") + Request.ApplicationPath + "/IntranetFiles/Footer/";
            try
            {
                intranetFooter.foot_estado = "A";
                if (file.ContentLength > 0 && file != null)
                {
                    if (file.ContentLength <= tamanioMaximo)
                    {
                        extension = Path.GetExtension(file.FileName);
                        if (extension == ".jpg" || extension == ".png" ||extension==".jpeg")
                        {
                            var nombreArchivo = "Footer_" + DateTime.Now.ToString("yyyyMMddHHmmss")+extension;
                            rutaInsertar = Path.Combine(direccion, nombreArchivo);
                            if (!Directory.Exists(direccion)) {
                                System.IO.Directory.CreateDirectory(direccion);
                            }
                            file.SaveAs(rutaInsertar);
                            intranetFooter.foot_imagen = nombreArchivo;
                            if (intranetFooter.ruta_anterior != "" && intranetFooter.ruta_anterior != null) {
                                string rutaAnterior = Path.Combine(direccion , intranetFooter.ruta_anterior);
                                if (System.IO.File.Exists(rutaAnterior))
                                {
                                    System.IO.File.Delete(rutaAnterior);
                                }
                            }
                        }
                        else
                        {
                            errormensaje = "Solo se aceptan formaton PNG ó JPG";
                            return Json(new { mensaje = errormensaje, respuesta = response });
                        }
                    }
                    else
                    {
                        errormensaje = "La imagen sobrepasa el tamaño maximo permitido (4mb)";
                        return Json(new { mensaje = errormensaje, respuesta = response });
                    }
                     //Eliminar registro de BD
                    var footerEliminadoTupla = intranetFooterbl.IntranetFooterEliminarJson(intranetFooter.foot_posicion);
                    if (footerEliminadoTupla.error.Respuesta)
                    {
                        var intranetFooterTupla = intranetFooterbl.IntranetFooterInsertarJson(intranetFooter);
                        error = intranetFooterTupla.error;
                        if (error.Respuesta)
                        {
                            if (intranetFooterTupla.idIntranetFooterInsertado > 0)
                            {
                                var footerTupla = intranetFooterbl.IntranetFooterIdObtenerJson(intranetFooterTupla.idIntranetFooterInsertado);
                                if (footerTupla.error.Respuesta) {
                                    response = true;
                                    errormensaje = "Insertado";
                                    footer = footerTupla.footer;
                                    footer.ruta_anterior = footer.foot_imagen;
                                }
                            
                            }
                            else
                            {
                                errormensaje = "No se Pudo Insertar";
                            }
                        }
                        else
                        {
                            errormensaje = error.Mensaje;
                        }
                    }
                    else {
                        errormensaje = "No se Pudo Insertar Imagen";
                    }
                }
                else {
                    //Editar Sin Imagen
                    var footerTupla=intranetFooterbl.IntrantetFooterEditarporPosicionJson(intranetFooter.foot_posicion,intranetFooter.foot_descripcion);
                    if(footerTupla.error.Respuesta){

                        if(footerTupla.IntranetFooterEditado>0){
                            var footerIdTupla = intranetFooterbl.IntranetFooterIdObtenerJson(footerTupla.IntranetFooterEditado);
                                if (footerIdTupla.error.Respuesta) {
                                    response = true;
                                    errormensaje = "Insertado";
                                    footer = footerIdTupla.footer;
                                    footer.ruta_anterior = footer.foot_imagen;
                                }
                        }
                    }
                    else{
                        errormensaje=footerTupla.error.Mensaje;
                    }
                }
               
                
            }
            catch (Exception ex) {
                errormensaje = ex.Message;
            }
            return Json(new { mensaje = errormensaje, respuesta = response,data=footer });
        }
        [HttpPost]
        public ActionResult IntranetFooterObtenerImagenes() {
            string errormensaje = "";
            bool response = false;
            claseError error = new claseError();
            List<IntranetFooterEntidad> lista = new List<IntranetFooterEntidad>();
            var direccion = Server.MapPath("/") + Request.ApplicationPath + "/IntranetFiles/Footer/";
            try
            {
                var intranetFooterTupla = intranetFooterbl.IntranetFooterObtenerFootersJson();
                error = intranetFooterTupla.error;
                if (error.Respuesta)
                {
                    lista = intranetFooterTupla.listaFooters;
                    foreach (var m in lista)
                    {
                        m.ruta_anterior = m.foot_imagen;
                        m.foot_imagen=m.foot_imagen;
                    }
                    
                    errormensaje = "Listando Data";
                    response = true;
                }
                else {
                    errormensaje = error.Mensaje;
                }
            }
            catch (Exception ex) {
                errormensaje = ex.Message;
            }
            //var serializer = new JavaScriptSerializer();
            //serializer.MaxJsonLength = Int32.MaxValue;
            //var resultData = new
            //{
            //    data = lista.ToList(),
            //    respuesta = response,
            //    mensaje = errormensaje
            //};


            //var result = new ContentResult
            //{
            //    Content = serializer.Serialize(resultData),
            //    ContentType = "application/json"
            //};
            return Json(new
            {
                data = lista.ToList(),
                respuesta = response,
                mensaje = errormensaje
            });
            //return Json(new { mensaje = errormensaje, respuesta = response, data = lista.ToList() });
        }
    }
}