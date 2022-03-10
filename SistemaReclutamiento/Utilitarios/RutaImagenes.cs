using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;


namespace SistemaReclutamiento.Utilitarios
{
    public class RutaImagenes
    {
        public void imagenPostulante_CV(string ruta,string name)
        {
            string postulante_image = "";
            if (name != null && name != ""&& ruta!=null && ruta!="")
            {
                postulante_image = @"" + ruta + "/" + name;
                if (System.IO.File.Exists(postulante_image))
                {
                    byte[] imagebytes = System.IO.File.ReadAllBytes(postulante_image);
                    string base64String = Convert.ToBase64String(imagebytes);
                    HttpContext.Current.Session["rutaPerfil"] = base64String;
                }
                else {
                    postulante_image = HttpContext.Current.Server.MapPath("~/images/user.png");
                    byte[] imagebytes = System.IO.File.ReadAllBytes(postulante_image);
                    string base64String = Convert.ToBase64String(imagebytes);
                    HttpContext.Current.Session["rutaPerfil"] = base64String;
                }
            }
            else
            {
                postulante_image = @"" + ruta ;
                byte[] imagebytes = System.IO.File.ReadAllBytes(postulante_image);
                string base64String = Convert.ToBase64String(imagebytes);
                HttpContext.Current.Session["rutaPerfil"] = base64String;
            }
        }

        public void Postulante_CV(string ruta,string name)
        {
            string postulante_cv = @"" + ruta + "/" + name;
            if (postulante_cv != null)
            {
                if (System.IO.File.Exists(postulante_cv))
                {
                    byte[] imagebytes = System.IO.File.ReadAllBytes(postulante_cv);
                    string base64String = Convert.ToBase64String(imagebytes);
                    HttpContext.Current.Session["rutaCv"] = base64String;
                }
            }
        }
        public string ImagenIntranetActividades(string ruta, string name) {
            string pathImagenActividades = @"" + ruta + "/" + name;
            string pathImagenporDefecto =@""+ ConfigurationManager.AppSettings["PathArchivosIntranet"].ToString()+"/Actividades/actividad.png";
            string base64String = "";
            if (name != null && name != "" && ruta != null && ruta != "")
            {
                if (System.IO.File.Exists(pathImagenActividades))
                {
                    byte[] imagebytes = System.IO.File.ReadAllBytes(pathImagenActividades);
                    base64String = Convert.ToBase64String(imagebytes);
                }
                else
                {
                    //byte[] imagebytes = System.IO.File.ReadAllBytes(pathImagenporDefecto);
                    //string base64String = Convert.ToBase64String(imagebytes);
                    //return base64String;
                    base64String=string.Empty;
                }
            }
            else {
                //byte[] imagebytes = System.IO.File.ReadAllBytes(pathImagenporDefecto);
                //string base64String = Convert.ToBase64String(imagebytes);
                //return base64String;
                base64String= string.Empty;
            }
            return base64String;
        }
        public string ImagenIntranetFooter(string ruta, string name)
        {
            string pathImagenFooter = @"" + ruta + "/" + name;
            string base64String = "";
            if (name != null && name != "" && ruta != null && ruta != "")
            {
                if (System.IO.File.Exists(pathImagenFooter))
                {
                    byte[] imagebytes = System.IO.File.ReadAllBytes(pathImagenFooter);
                    base64String = Convert.ToBase64String(imagebytes);
                }
                else
                {
                    base64String = string.Empty;
                }
            }
            else
            {
                base64String = string.Empty;
            }
            return base64String;
        }
        public string ImagenCumpleaños(string ruta, string name)
        {
            string pathImagenAvatar = @"" + ruta + "/" + name;
            string base64String = "";
            if (name != null && name != "" && ruta != null && ruta != "")
            {
                if (System.IO.File.Exists(pathImagenAvatar))
                {
                    byte[] imagebytes = System.IO.File.ReadAllBytes(pathImagenAvatar);
                    base64String = Convert.ToBase64String(imagebytes);
                }
                else
                {
                    base64String = string.Empty;
                }
            }
            else
            {
                base64String = string.Empty;
            }
            return base64String;
        }
    }
}