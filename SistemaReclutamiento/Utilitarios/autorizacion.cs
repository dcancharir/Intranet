using SistemaReclutamiento.Models.SeguridadIntranet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.Web.Routing;
using SistemaReclutamiento.Entidades.SeguridadIntranet;
using SistemaReclutamiento.Controllers.SeguridadIntranet;
using SistemaReclutamiento.Entidades;

namespace SistemaReclutamiento.Utilitarios
{
    [global::System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Method |
                AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class autorizacion: AuthorizeAttribute
    {
        SEG_PermisoRolDAL segpermisorolbl = new SEG_PermisoRolDAL();
        SEG_PermisoDAL segpermisobl = new SEG_PermisoDAL();

        public autorizacion(bool activa = true)
        {
            activar = activa;
        }
        private bool defaultactivar = true;
        public bool activar
        {
            get
            {
                return defaultactivar;
            }
            set
            {
                defaultactivar = value;
            }
        }
        public string permisonombre { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;

            var usuario = HttpContext.Current.Session["usuario"];
            if (usuario != null)
            {
                authorize = true;
            }
            return authorize;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            ////comentar para activar seguridad
            //bool seguridadActivo = false;
            //if (seguridadActivo == false)
            //{
            //    return;
            //}
            ////
            string control = filterContext.Controller.ValueProvider.GetValue("Controller").AttemptedValue;
            string accion = filterContext.Controller.ValueProvider.GetValue("action").AttemptedValue;
            
            bool authorize = false;
            Metodo_atributos metodoObjeto = new SeguridadIntranetController().Metodo_Objeto(control, accion);
            if (metodoObjeto.seguridad == false)
            {
                return;
            }

            if (filterContext.HttpContext.Session["usuario"] == null)
            {
                HandleUnauthorizedRequest(filterContext);
                return;
            }
            UsuarioEntidad registerusuario = (UsuarioEntidad)filterContext.HttpContext.Session["usuario"];
            if (registerusuario.usu_nombre == "administradorsgc")
            {
                return;
            }
            string estatus = "";
            int rolId = (int)filterContext.HttpContext.Session["rol"];
            var permisoTupla= segpermisorolbl.GetseguridadPermisoRol(rolId, control + "Controller", accion);
            var permiso = permisoTupla.webPermisoRol;

            if (permiso.WEB_PermID == 0)
            {
                var nombreControllerPermisoTupla= segpermisobl.GetPermisoId(control + "Controller", accion);
                var nombreControllerPermiso = nombreControllerPermisoTupla.webPermisoRol;

                if (nombreControllerPermiso.WEB_PermID > 0)
                {
                    if (nombreControllerPermiso.WEB_PermNombreR != "")
                    {
                        this.permisonombre = "<br><strong style='font-size:10px'>" + nombreControllerPermiso.WEB_PermNombreR + "</strong>";
                    }
                    else
                    {
                        this.permisonombre = "<br><strong style='font-size:10px'>" + nombreControllerPermiso.WEB_PermNombre + "</strong>";
                    }
                }
                else
                {
                    this.permisonombre = "<br><strong style='font-size:10px'>" + accion + "</strong>";

                }


                authorize = false;
            }
            else
            {
                if (permiso.WEB_PermNombreR != "")
                {
                    this.permisonombre = "<br><strong style='font-size:10px'>" + permiso.WEB_PermNombreR + "</strong>";
                }
                else
                {
                    this.permisonombre = "<br><strong style='font-size:10px'>" + permiso.WEB_PermNombre + "</strong>";
                }

                HttpCookie mensaje = new HttpCookie("controlador");
                mensaje.Value = "";
                authorize = true;
            }

            if (!authorize)
            {
                try
                {
                    HandleUnauthorizedRequest(filterContext);
                }
                catch (Exception ex)
                {
                    estatus = ex.Message;
                }
            }
            else
            {
                return;
            }
        }


        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            var httpContext = context.HttpContext;
            var request = httpContext.Request;
            var response = httpContext.Response;
            var accion = context.Controller.ValueProvider.GetValue("action").AttemptedValue;

            if (context.HttpContext.Request.IsAjaxRequest())
            {
                DateTime now = DateTime.Now;
                HttpCookie mensaje = new HttpCookie("controlador");
                mensaje.Value = this.permisonombre;
                mensaje.Expires = now.AddYears(50);
                context.HttpContext.Response.Cookies.Add(mensaje);

                if (HttpContext.Current.Session["usuario"] != null)
                {

                    response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    response.SuppressFormsAuthenticationRedirect = true;
                    response.End();
                    base.HandleUnauthorizedRequest(context);
                }
                else
                {


                    response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                    response.SuppressFormsAuthenticationRedirect = true;
                    response.End();
                    base.HandleUnauthorizedRequest(context);

                }
            }
            else
            {

                if (HttpContext.Current.Session["usuario"] == null)
                {
                    DateTime now = DateTime.Now;
                    HttpCookie mensaje = new HttpCookie("controlador");
                    mensaje.Value = "";
                    mensaje.Expires = now.AddYears(50);
                    context.HttpContext.Response.Cookies.Add(mensaje);

                    context.Result = new RedirectToRouteResult("acceso", null);
                }
                else
                {
                    DateTime now = DateTime.Now;
                    HttpCookie mensaje = new HttpCookie("controlador");
                    mensaje.Value = "No tiene Permiso " + this.permisonombre;
                    mensaje.Expires = now.AddYears(50);
                    context.HttpContext.Response.Cookies.Add(mensaje);

                    context.Result = new RedirectToRouteResult("reporte", null);
                }

            }

        }
    }
}