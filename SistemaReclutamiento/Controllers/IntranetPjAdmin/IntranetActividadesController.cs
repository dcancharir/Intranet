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

namespace SistemaReclutamiento.Controllers.IntranetPJAdmin
{
    [autorizacion(false)]
    public class IntranetActividadesController : Controller
    {
        IntranetActividadesModel intranetActividadesbl = new IntranetActividadesModel();
        claseError error = new claseError();
        string PathActividadesIntranet = ConfigurationManager.AppSettings["PathArchivosIntranet"].ToString();
        RutaImagenes rutaImagenes = new RutaImagenes();
        // GET: IntranetActividades
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IntranetActividadesListarJson()
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetActividadesEntidad> listaActividades = new List<IntranetActividadesEntidad>();
            try
            {
                var ActividadesTupla = intranetActividadesbl.IntranetActividadesListarJson();
                error = ActividadesTupla.error;
                listaActividades = ActividadesTupla.intranetActividadesLista;
                if (error.Respuesta)
                {
                    mensaje = "Listando Actividadess";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Mensaje;
                    mensaje = "No se Pudieron Listar las Actividadess";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaActividades.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }

        [HttpPost]
        public ActionResult IntranetActividadesListarTodoJson()
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetActividadesEntidad> listaActividades = new List<IntranetActividadesEntidad>();
            var direccion = Server.MapPath("/") + Request.ApplicationPath + "/IntranetFiles/Actividades/";
            try
            {
                var ActividadesTupla = intranetActividadesbl.IntranetActividadesListarTodoJson();
                error = ActividadesTupla.error;
                listaActividades = ActividadesTupla.intranetActividadesLista;

                if (!Directory.Exists(direccion))
                {
                    Directory.CreateDirectory(direccion);
                }
                if (error.Respuesta)
                {
                    mensaje = "Listando Actividadess";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Mensaje;
                    mensaje = "No se Pudieron Listar las Actividadess";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }

            //var serializer = new JavaScriptSerializer();
            //serializer.MaxJsonLength = Int32.MaxValue;
            //var resultData = new
            //{
            //    data = listaActividades.ToList(),
            //    respuesta = respuesta,
            //    mensaje = mensaje,
            //    mensajeconsola = mensajeConsola
            //};


            //var result = new ContentResult
            //{
            //    Content = serializer.Serialize(resultData),
            //    ContentType = "application/json"
            //};
            //return result;
            return Json(new
            {
                data = listaActividades.ToList(),
                respuesta = respuesta,
                mensaje = mensaje,
                mensajeconsola = mensajeConsola
            });
        }

        [HttpPost]
        public ActionResult IntranetActividadesIdObtenerJson(int act_id)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            IntranetActividadesEntidad actividad = new IntranetActividadesEntidad();
            try
            {
                var actividadesTupla = intranetActividadesbl.IntranetActividadesIdObtenerJson(act_id);
                error = actividadesTupla.error;
                actividad = actividadesTupla.intranetActividades;
                if (error.Respuesta)
                {
                    actividad.img_ubicacion = actividad.act_imagen;
                    //actividad.act_imagen = rutaImagenes.ImagenIntranetActividades(PathActividadesIntranet + "/Actividades/", actividad.act_imagen);
                    mensaje = "Obteniendo Informacion de la Actividad Seleccionada";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Mensaje;
                    mensaje = "No se Pudo Obtener La Informacion de la Actividad Seleccionada";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            //var serializer = new JavaScriptSerializer();
            //serializer.MaxJsonLength = Int32.MaxValue;
            //var resultData = new
            //{
            //    data = actividad,
            //    respuesta = respuesta,
            //    mensaje = mensaje,
            //    mensajeconsola = mensajeConsola
            //};


            //var result = new ContentResult
            //{
            //    Content = serializer.Serialize(resultData),
            //    ContentType = "application/json"
            //};
            //return result;
            return Json(new { data = actividad, respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }

        [HttpPost]
        public ActionResult IntranetActividadesNuevoJson(IntranetActividadesEntidad intranetActividad)
        {
            HttpPostedFileBase file = Request.Files[0];
            int tamanioMaximo = 4194304;
            string extension = "";
            string rutaInsertar = "";
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            int idIntranetActividadInsertado = 0;
            var direccion = Server.MapPath("/") + Request.ApplicationPath + "/IntranetFiles/Actividades/";
            try
            {
                //IMAGEN
                if (file.ContentLength > 0 && file != null)
                {
                    if (file.ContentLength <= tamanioMaximo)
                    {
                        extension = Path.GetExtension(file.FileName);
                        if (extension == ".jpg" || extension == ".png")
                        {
                            var nombreArchivo = ("Actividad_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension);
                            rutaInsertar = Path.Combine(direccion, nombreArchivo);
                            if (!Directory.Exists(direccion))
                            {
                                System.IO.Directory.CreateDirectory(direccion);
                            }
                            file.SaveAs(rutaInsertar);
                            intranetActividad.act_imagen = nombreArchivo;
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
                    intranetActividad.act_imagen = "";
                }

                var actividadTupla = intranetActividadesbl.IntranetActividadesInsertarJson(intranetActividad);
                error = actividadTupla.error;

                if (error.Respuesta)
                {
                    mensaje = "Se Registró Correctamente";
                    respuesta = true;
                    idIntranetActividadInsertado = actividadTupla.idIntranetActividadesInsertado;
                }
                else
                {
                    mensaje = "No se Pudo insertar la Actividad";
                    mensajeConsola = error.Mensaje;
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje , mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetActividadesEditarJson(IntranetActividadesEntidad intranetActividad)
        {
            HttpPostedFileBase file = Request.Files[0];
            int tamanioMaximo = 4194304;
            string extension = "";
            string rutaInsertar = "";
            string rutaAnterior = "";
            string errormensaje = "";
            bool respuesta = false;
            string mensaje = "";
            string mensajeConsola = "";
            var direccion = Server.MapPath("/") + Request.ApplicationPath + "/IntranetFiles/Actividades/";
            try
            {

                if (intranetActividad.img_ubicacion == "" && intranetActividad.img_ubicacion != null)
                {
                    intranetActividad.act_imagen = intranetActividad.img_ubicacion;
                    var actividadTupla = intranetActividadesbl.IntranetActividadesEditarJson(intranetActividad);
                    error = actividadTupla.error;
                    if (error.Respuesta)
                    {
                        respuesta = actividadTupla.intranetActividadesEditado;
                        mensaje = "Se Editó la Actividad Correctamente";
                    }
                    else
                    {
                        mensajeConsola = error.Mensaje;
                        mensaje = "Error, no se Puede Editar";
                    }
                }
                else
                {
                    if (file.ContentLength > 0 && file != null)
                    {
                        if (file.ContentLength <= tamanioMaximo)
                        {
                            extension = Path.GetExtension(file.FileName);
                            if (extension == ".jpg" || extension == ".png")
                            {

                                var nombreArchivo = ("Actividad_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension);
                                rutaInsertar = Path.Combine(direccion, nombreArchivo);
                                rutaAnterior = Path.Combine(direccion + intranetActividad.img_ubicacion);
                                if (!Directory.Exists(direccion))
                                {
                                    System.IO.Directory.CreateDirectory(direccion);
                                }
                                if (System.IO.File.Exists(rutaAnterior))
                                {
                                    System.IO.File.Delete(rutaAnterior);
                                }
                                file.SaveAs(rutaInsertar);
                                intranetActividad.act_imagen = nombreArchivo;
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
                        intranetActividad.act_imagen = intranetActividad.img_ubicacion;
                    }

                    var actividadTupla = intranetActividadesbl.IntranetActividadesEditarJson(intranetActividad);
                    error = actividadTupla.error;
                    if (error.Respuesta)
                    {
                        respuesta = actividadTupla.intranetActividadesEditado;
                        mensaje = "Se Editó la Actividad Correctamente";
                    }
                    else
                    {
                        mensajeConsola = error.Mensaje;
                        mensaje = "Error, no se Puede Editar";
                    }
                }
            }
            catch (Exception exp)
            {
                mensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuesta, mensaje = errormensaje, mensajeconsola = mensajeConsola });
        }

        [HttpPost]
        public ActionResult IntranetActividadesEliminarJson(int act_id)
        {
            string errormensaje = "";
            bool respuestaConsulta = false;
            string mensajeConsola = "";
            IntranetActividadesEntidad intranetActividadesBusqueda = new IntranetActividadesEntidad();
            string rutaEliminar = "";
            var direccion = Server.MapPath("/") + Request.ApplicationPath + "/IntranetFiles/Actividades/";
            try
            {
                var actividadesBusquedatupla = intranetActividadesbl.IntranetActividadesIdObtenerJson(act_id);
                error = actividadesBusquedatupla.error;
                if (error.Respuesta) {
                    intranetActividadesBusqueda = actividadesBusquedatupla.intranetActividades;
                    if (!intranetActividadesBusqueda.act_imagen.Equals(string.Empty))
                    {
                        //Eliminar Archivo Primero
                        var nombreArchivo = intranetActividadesBusqueda.act_imagen;
                        rutaEliminar = Path.Combine(direccion, nombreArchivo);
                        //var fullPath = Server.MapPath("~/Content/intranet/images/png/"+nombreArchivo);
                        if (System.IO.File.Exists(rutaEliminar))
                        {
                            System.IO.File.Delete(rutaEliminar);
                        }
                    }

                    var ActividadesTupla = intranetActividadesbl.IntranetActividadesEliminarJson(act_id);
                    error = ActividadesTupla.error;
                    if (error.Respuesta)
                    {
                        respuestaConsulta = ActividadesTupla.intranetActividadesEliminado;
                        errormensaje = "Actividad Eliminado";
                    }
                    else
                    {
                        errormensaje = "Error, no se Puede Eliminar";
                        mensajeConsola = error.Mensaje;
                    }
                }

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje, mensajeconsola = mensajeConsola });
        }
        
        [HttpPost]
        public ActionResult IntranetActividadesEliminarVariosJson(int[] listaActividadesEliminar)
        {
            string errormensaje = "";
            string mensajeConsola = "";
            bool respuestaConsulta = false;
            claseError error = new claseError();
            IntranetActividadesEntidad intranetActividadesBusqueda = new IntranetActividadesEntidad();
            string rutaEliminar = "";
            var direccion = Server.MapPath("/") + Request.ApplicationPath + "/IntranetFiles/Actividades/";
            try
            {
                for (int i = 0; i <= listaActividadesEliminar.Length - 1; i++)
                {
                    var actividadesBusquedatupla = intranetActividadesbl.IntranetActividadesIdObtenerJson(listaActividadesEliminar[i]);
                    error = actividadesBusquedatupla.error;
                    if (error.Respuesta)
                    {
                        intranetActividadesBusqueda = actividadesBusquedatupla.intranetActividades;
                        if (!intranetActividadesBusqueda.act_imagen.Equals(string.Empty))
                        {
                            //Eliminar Archivo Primero
                            var nombreArchivo = intranetActividadesBusqueda.act_imagen;
                            //var fullPath = Server.MapPath("~/Content/intranet/images/png/" + nombreArchivo);
                            rutaEliminar = Path.Combine(direccion, nombreArchivo);
                            if (System.IO.File.Exists(rutaEliminar))
                            {
                                System.IO.File.Delete(rutaEliminar);
                            }
                        }

                        var ActividadesTupla = intranetActividadesbl.IntranetActividadesEliminarJson(intranetActividadesBusqueda.act_id);
                        error = ActividadesTupla.error;
                        if (error.Respuesta)
                        {
                            respuestaConsulta = ActividadesTupla.intranetActividadesEliminado;
                            errormensaje = "Actividades Eliminado";
                        }
                        else
                        {
                            errormensaje = "Error, no se Puede Eliminar";
                            mensajeConsola = error.Mensaje;
                        }

                    }
                }
                
            }
            catch (Exception ex)
            {
                errormensaje = "Error, no se Puede Eliminar, " + ex.Message;
                respuestaConsulta = false;
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje, mensajeconsola = mensajeConsola });
        }
    }
}