using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.IntranetPjAdmin
{
    [autorizacion(false)]
    public class IntranetArchivosController : Controller
    {
        // GET: IntranetArchivos
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IntranetArchivosObtenerListadoJson()
        {
            bool response = false;
            string errormensaje = "";
            var listaArchivos = new List<dynamic>();
            string nombre_archivo = "";
            try
            {
                var direccion = Server.MapPath("/") + Request.ApplicationPath + "/archivos";
                if (Directory.Exists(direccion))
                {
                    DirectoryInfo di = new DirectoryInfo(direccion);
                    foreach (var m in di.GetFiles())
                    {
                        string[] info = m.Name.Split('.');

                        //verificar si el nombre de archivo tenia varios puntos "."
                        if (info.Count() > 2)
                        {
                            foreach (var k in info)
                            {
                                if (k != info.LastOrDefault())
                                {
                                    nombre_archivo += k + ".";
                                }
                            }
                            nombre_archivo = nombre_archivo.Substring(0, nombre_archivo.Length - 1);
                        }
                        else
                        {
                            nombre_archivo = info[0];
                        }
                        //tamaño de archivo
                        float length = (m.Length / 1024f) / 1024f;
                        listaArchivos.Add(new
                        {
                            nombre = nombre_archivo,
                            extension = info.LastOrDefault(),
                            nombre_completo = m.Name,
                            tamanio=Math.Round(length,4)
                        });
                    }
                    errormensaje = "Listando Data";
                    response = true;
                }
                else
                {
                    errormensaje = "No se encuentra el Directorio";
                }
            }
            catch (Exception ex)
            {
                errormensaje = ex.Message;
            }
            return Json(new { data = listaArchivos.ToList(), respuesta = response, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult IntranetArchivosInsertar(string nombre_completo) {
            HttpPostedFileBase file = Request.Files[0];
            int tamanioMaximo = 15728640;
            string rutaInsertar = "";
            bool response = false;
            string errormensaje = "";
            try
            {
                if (file.ContentLength > 0 && file != null)
                {
                    if (file.ContentLength <= tamanioMaximo)
                    {
                        string nombreArchivo = Path.GetFileName(file.FileName);
                        var direccion = Server.MapPath("/") + Request.ApplicationPath + "/archivos/";
                        rutaInsertar = Path.Combine(direccion, nombreArchivo);
                        if (!Directory.Exists(direccion))
                        {
                            Directory.CreateDirectory(direccion);
                        }
                        //if (System.IO.File.Exists(rutaInsertar))
                        //{
                        //    string rutaAlternativa = Path.Combine(direccion, string.Format("alt_{0}", nombreArchivo));
                        //    System.IO.File.Copy(rutaInsertar,rutaAlternativa,true);
                        //}
                        file.SaveAs(rutaInsertar);
                        errormensaje = "Archivo Subido";
                        response = true;
                    }
                    else
                    {
                        errormensaje = "El tamaño maximo de arhivo permitido es de 15Mb.";
                        response = false;
                        return Json(new { respuesta = response, mensaje = errormensaje });
                    }

                }
                else
                {
                    errormensaje = "No se ha subido ningun archivo";
                }
            }
            catch (Exception ex) {
                errormensaje = ex.Message;
            }
            return Json(new { respuesta = response, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult IntranetArchivosEliminar(string nombre_completo) {
            string errormensaje = "";
            bool response = false;
            string rutaEliminar = "";
            try
            {
                var direccion = Server.MapPath("/") + Request.ApplicationPath + "/archivos/";
                rutaEliminar = Path.Combine(direccion, nombre_completo);
                if (System.IO.File.Exists(rutaEliminar))
                {
                    System.IO.File.Delete(rutaEliminar);
                }
                response = true;
                errormensaje = "Archivo Eliminado";
            }
            catch (Exception ex) {
                errormensaje = ex.Message;
            }
            return Json(new { respuesta=response,mensaje=errormensaje });
        }
        [HttpPost]
        public ActionResult IntranetArchivosEliminarVarios(string[] arrayArchivosEliminar) {
            string errormensaje = "";
            bool response = false;
            string rutaEliminar = "";
            var direccion = Server.MapPath("/") + Request.ApplicationPath + "/archivos/";
            try
            {
                if (arrayArchivosEliminar.Count() > 0)
                {
                    for (int i = 0; i <= arrayArchivosEliminar.Length - 1; i++) {
                        rutaEliminar = Path.Combine(direccion, arrayArchivosEliminar[i]);
                        if (System.IO.File.Exists(rutaEliminar))
                        {
                            System.IO.File.Delete(rutaEliminar);
                        }
                    }
                }
                response = true;
                errormensaje = "Archivos Eliminados";
            }
            catch (Exception ex)
            {
                errormensaje = ex.Message;
            }
            return Json(new { mensaje = errormensaje, respuesta = response });
        }


    }
}