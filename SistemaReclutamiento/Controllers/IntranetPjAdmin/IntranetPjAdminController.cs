using SistemaReclutamiento.Entidades;
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
using OfficeOpenXml;
using SistemaReclutamiento.Entidades.SeguridadIntranet;
using SistemaReclutamiento.Models.SeguridadIntranet;

namespace SistemaReclutamiento.Controllers.IntranetPJAdmin
{
    [autorizacion(false)]
    public class IntranetPjAdminController : Controller
    {
        IntranetUsuarioModel usuarioIntranetbl = new IntranetUsuarioModel();
        IntranetMenuModel intranetMenubl = new IntranetMenuModel();
        IntranetAccesoModel usuarioAccesobl = new IntranetAccesoModel();
        IntranetDetalleElementoModel detalleelementobl = new IntranetDetalleElementoModel();
        IntranetDetalleElementoModalModel detalleelementomodalbl = new IntranetDetalleElementoModalModel();
        UsuarioModel usuariobl = new UsuarioModel();
        PersonaModel personabl = new PersonaModel();
        SQLModel sqlbl = new SQLModel();
        string pathArchivosIntranet = ConfigurationManager.AppSettings["PathArchivosIntranet"].ToString();
        claseError error = new claseError();
        RutaImagenes rutaImagenes = new RutaImagenes();
        private SEG_PermisoRolDAL webPermisoRolBL = new SEG_PermisoRolDAL();
        private SEG_RolUsuarioDAL webRolUsuarioBL = new SEG_RolUsuarioDAL();
        private SEG_PermisoMenuDAL webPermisoMenuBl = new SEG_PermisoMenuDAL();
        // GET: IntranetPjAdmin
        public ActionResult Index()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJAdminIndex.cshtml");
        }

        public ActionResult PanelMenus()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJMenus.cshtml");
        }
        public ActionResult PanelActividades()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJActividades.cshtml");
        }
        public ActionResult PanelComentarios()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJComentarios.cshtml");
        }
        public ActionResult PanelFooter()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJFooter.cshtml");
        }
        public ActionResult PanelArchivos()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJArchivos.cshtml");
        }
        public ActionResult PanelFichas()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJFichas.cshtml");
        }

        public ActionResult PanelSubidaExcel()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJSubidaExcel.cshtml");
        }
        public ActionResult PanelEmpresas()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJEmpresas.cshtml");
        }

        public ActionResult PanelSecciones(int menu_id=1)
        {
            //List<IntranetMenuEntidad> intranetMenu = new List<IntranetMenuEntidad>();
            //claseError error = new claseError();
            //string mensajeerrorBD = "";
            //string mensaje = "";
            //try
            //{
            //    var menuTupla = intranetMenubl.IntranetMenuListarJson();
            //    error = menuTupla.error;
            //    if (error.Respuesta)
            //    {
            //        intranetMenu = menuTupla.intranetMenuLista;
            //        ViewBag.Menu = intranetMenu;
            //    }
            //    else
            //    {
            //        mensajeerrorBD += "Error en Menus: " + error.Mensaje + "\n";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    mensaje = ex.Message;
            //}
            
            return View("~/Views/IntranetPJAdmin/IntranetPJSecciones1.cshtml");
        }
        public ActionResult PanelConfiguracionToken()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJToken.cshtml");
        }
        public ActionResult PanelSistemas()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJSistemas.cshtml");
        }

        #region seccion1

        public ActionResult PanelSecciones1()
        {
            return View("~/Views/IntranetPjAdmin/IntranetPJSecciones1.cshtml");
        }
        #endregion


        #region Region Acceso a Mantenimiento Intranet PJ
        public ActionResult Login()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJAdminLogin.cshtml");
        }
        [HttpPost]
        public ActionResult IntranetPJAdminValidarCredencialesJson(string usu_login, string usu_password)
        {
            bool respuesta = false;
            string errormensaje = "";
            string mensajeConsola = "";
            UsuarioEntidad usuario = new UsuarioEntidad();
            PersonaEntidad persona = new PersonaEntidad();
            claseError error = new claseError();
            var permisoRol = new List<SEG_PermisoRolEntidad>();
            var rolUsuario = new SEG_RolUsuarioEntidad();
            string pendiente = "";
            try
            {
                string usuario_sgc = ConfigurationManager.AppSettings["usuario_sgc"].ToString();
                string password_sgc = ConfigurationManager.AppSettings["password_sgc"].ToString();
                if (usuario_sgc == usu_login)
                {
                    var contrasenia = Seguridad.EncriptarSHA512(usu_password.Trim());
                    if (password_sgc == contrasenia)
                    {
                        usuario.usu_nombre = "administradorsgc";
                        persona.per_nombre = "administradorsgc";

                        Session["usuSGC_full"] = usuario;
                        Session["perSGC_full"] = persona;

                        rolUsuario.WEB_RolID = 0;
                        int rol = rolUsuario.WEB_RolID;
                        Session["rol"] = rol;
                        Session["permisos"] = permisoRol;

                        Session["UsuarioID"] = usuario.usu_id;
                        Session["UsuarioNombre"] = usuario.usu_nombre;

                        usuario.usu_tipo="EMPLEADO";
                        usuario.usu_nombre = "administradorsgc";
                        Session["usuario"] = usuario;
                        respuesta = true;
                        return Json(new { respuesta,mensaje= "Bienvenido"+ usuario.usu_nombre });
                    }
                    else
                    {
                        
                        respuesta = false;
                        return Json(new { respuesta, mensaje="Contraseña no coincide" });
                    }
                }


                var usuarioTupla = usuarioAccesobl.UsuarioIntranetSGCValidarCredenciales(usu_login.ToLower());
                error = usuarioTupla.error;
                if (error.Respuesta)
                {
                    usuario = usuarioTupla.intranetUsuarioSGCEncontrado;
                    if (usuario.usu_id > 0)
                    {
                        if (usuario.usu_estado == "A")
                        {
                            if (usuario.usu_tipo == "EMPLEADO")
                            {
                                if (usuario.usu_contrasenia == Seguridad.EncriptarSHA512(usu_password.Trim()))
                                {
                                    
                                    Session["usuSGC_full"] = usuariobl.UsuarioObtenerxID(usuario.usu_id);
                                    persona = personabl.PersonaIdObtenerJson(usuario.fk_persona);
                                    Session["perSGC_full"] = persona;

                                    var rolUsuarioTupla= webRolUsuarioBL.GetRolUsuarioId(usuario.usu_id);
                                    rolUsuario = rolUsuarioTupla.webRolUsuario;
                                    //rolUsuario = webRolUsuarioBL.GetRolUsuarioId(usuario.UsuarioID);
                                    int rol = rolUsuario.WEB_RolID;
                                    var permisoRolTupla = webPermisoRolBL.GetPermisoRolrolid(rol);
                                    permisoRol = permisoRolTupla.lista;
                                    Session["rol"] = rol;
                                    Session["permisos"] = permisoRol;
                                    Session["UsuarioID"] = usuario.usu_id;
                                    Session["UsuarioNombre"] = usuario.usu_nombre;
                                    Session["usuario"] = usuario;
                                    respuesta = true;
                                    errormensaje = "Bienvenido, " + usuario.usu_nombre;
                                }
                                else
                                {
                                    errormensaje = "Contraseña no Coincide";
                                }
                            }
                            else
                            {
                                errormensaje = "Usuario no Pertenece a CPJ";
                            }
                        }
                        else
                        {
                            errormensaje = "Usuario no se Encuentra Activo";
                        }
                    }
                    else
                    {
                        errormensaje = "Usuario no Encontrado";
                    }
                }
                else
                {
                    errormensaje = "Ha ocurrido un problema";
                    mensajeConsola = error.Mensaje;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + "";
                mensajeConsola = error.Mensaje;
            }

            return Json(new { mensajeconsola = mensajeConsola, respuesta = respuesta, mensaje = errormensaje, estado = pendiente/*, usuario=usuario*/ });
        }
        [HttpPost]
        public ActionResult IntranetPJAdminCerrarSesionLoginJson()
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                Session["usuSGC_full"] = null;
                Session["perSGC_full"] = null;
                respuestaConsulta = true;
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        #endregion

        #region Edicion de Hash para imagenes de Detalle Elemento y Detalle elemento modal
        public ActionResult IntranetEditarHashDetallesJson() {
            bool response = false;
            string errormensaje = "";
            List<IntranetDetalleElementoEntidad> listaDetalleElemento = new List<IntranetDetalleElementoEntidad>();
            List<IntranetDetalleElementoModalEntidad> listaDetalleElementoModal = new List<IntranetDetalleElementoModalEntidad>();
            List<IntranetDetalleElementoEntidad> listaDetalleElementoDevuelta = new List<IntranetDetalleElementoEntidad>();
            List<IntranetDetalleElementoModalEntidad> listaDetalleElementoModalDevuelta = new List<IntranetDetalleElementoModalEntidad>();
            int totalDetalles=0, totaldetallesEditados=0, totaldetallemodal=0, totaldetallemodalEditado = 0;
            try
            {
                //Detalleelemento
                var listadetelemtupla = detalleelementobl.IntranetDetalleElementoListarJson();
                if (listadetelemtupla.error.Respuesta)
                {
                    listaDetalleElemento = listadetelemtupla.intranetDetalleElementoLista.Where(x => x.detel_extension != "").ToList();
                    totalDetalles = listaDetalleElemento.Count;
                    if (listaDetalleElemento.Count > 0) {
                        
                        foreach (var detalle in listaDetalleElemento) {
                            detalle.detel_hash= rutaImagenes.ImagenIntranetActividades(pathArchivosIntranet, detalle.detel_nombre+"."+detalle.detel_extension);
                            var detalleElementoEditado = detalleelementobl.IntranetDetalleElementoEditarHashJson(detalle);
                            if (detalleElementoEditado.error.Respuesta)
                            {
                                totaldetallesEditados++;
                            }
                            else {
                                errormensaje += detalleElementoEditado.error.Mensaje;
                            }
                        }
                    }
                    errormensaje += "Detalle Elemento,";
                }
                else
                {
                    errormensaje += listadetelemtupla.error.Mensaje;
                }
                //DetalleElementoModal
                var listadetelemodTupla = detalleelementomodalbl.IntranetDetalleElementoModalListarJson();
                if (listadetelemodTupla.error.Respuesta)
                {
                    listaDetalleElementoModal = listadetelemodTupla.intranetDetalleElementoModalLista.Where(x => x.detelm_extension != "").ToList();
                    totaldetallemodal = listaDetalleElementoModal.Count;
                    if (listaDetalleElementoModal.Count > 0)
                    {
                        foreach (var detallemodal in listaDetalleElementoModal)
                        {
                            detallemodal.detelm_hash = rutaImagenes.ImagenIntranetActividades(pathArchivosIntranet, detallemodal.detelm_nombre + "." + detallemodal.detelm_extension);
                            var detalleElementoModalEditado = detalleelementomodalbl.IntranetDetalleElementoModalEditarHashJson(detallemodal);
                            if (detalleElementoModalEditado.error.Respuesta)
                            {
                                totaldetallemodalEditado++;
                            }
                            else
                            {
                                errormensaje += detalleElementoModalEditado.error.Mensaje;
                            }
                        }
                    }
                    errormensaje += " Detalle Elemento Modal,";
                }
                else {
                    errormensaje += listadetelemodTupla.error.Mensaje;
                }
                response = true;
                errormensaje += " Editados";

            }
            catch (Exception ex) {
                errormensaje = ex.Message;
            }
            //DetalleElemento
            //DetalleElementoModal
            return Json(new {
                totaldetalleElemento =totalDetalles,
                totaldetalleElementoEditado =totaldetallesEditados,
                totaldetalleElementoModal= totaldetallemodal,
                totalDetalleElementoModalEditados= totaldetallemodalEditado,
                respuesta =response,
                mensaje =errormensaje},JsonRequestBehavior.AllowGet);
        }

        #endregion
    
        [HttpPost]
        public ActionResult IntranetBuscarEmpleadosModalJson(string busqueda, string opcion)
        {
            string errormensaje = "";
            bool response = false;
            List<PersonaSqlEntidad> lista = new List<PersonaSqlEntidad>();
            try
            {
                if (opcion.ToUpper() == "DNI")
                {
                    int mes_actual = DateTime.Now.Month;
                    int anio = DateTime.Now.Year;
                    var personaTupla = sqlbl.PersonaSQLObtenerInformacionPuestoTrabajoJson(busqueda, mes_actual, anio);
                    if (personaTupla.error.Respuesta)
                    {
                        PersonaSqlEntidad persona = new PersonaSqlEntidad();
                        if (personaTupla.persona.CO_TRAB == null)
                        {
                            if (mes_actual == 1)
                            {
                                mes_actual = 12;
                                anio = anio - 1;
                            }
                            else
                            {
                                mes_actual = mes_actual - 1;
                            }
                            var personaSQLTupla2 = sqlbl.PersonaSQLObtenerInformacionPuestoTrabajoJson(busqueda, mes_actual, anio);
                            if (personaSQLTupla2.error.Respuesta)
                            {
                                if (personaSQLTupla2.persona.CO_TRAB != null)
                                {
                                    persona = personaSQLTupla2.persona;
                                    lista.Add(persona);
                                }

                            }
                        }
                        else
                        {
                            persona = personaTupla.persona;
                            lista.Add(persona);
                        }
                       
                        response = true;
                        errormensaje = "Listando Info";
                    }
                    else
                    {
                        errormensaje = personaTupla.error.Mensaje;
                    }
                }
                else if(opcion.ToUpper()=="APELLIDOS")
                {
                    int mes_actual = DateTime.Now.Month;
                    int anio = DateTime.Now.Year;
                    var personaTupla = sqlbl.PersonaSQLObtenerInformacionPuestoTrabajoxApellidoJson(busqueda.ToUpper(), mes_actual, anio);
                    if (personaTupla.error.Respuesta)
                    {
                        if (personaTupla.lista.Count == 0)
                        {
                            if (mes_actual == 1)
                            {
                                mes_actual = 12;
                                anio = anio - 1;
                            }
                            else
                            {
                                mes_actual = mes_actual - 1;
                            }
                            var personaSQLTupla2 = sqlbl.PersonaSQLObtenerInformacionPuestoTrabajoxApellidoJson(busqueda.ToUpper(), mes_actual, anio);
                            if (personaSQLTupla2.error.Respuesta)
                            {
                                lista = personaSQLTupla2.lista;
                            }
                        }
                        else
                        {
                            lista = personaTupla.lista;
                        }
                        response = true;
                        errormensaje = "Listando Info";
                    }
                    else
                    {
                        errormensaje = personaTupla.error.Mensaje;
                    }
                }
                else
                {
                    errormensaje = "Opcion : "+opcion+", Incorrecta";
                }
            }catch(Exception ex)
            {
                errormensaje = ex.Message;
            }
            return Json(new { mensaje=errormensaje,respuesta=response,data=lista });
        }
       
        [HttpPost]
        public ActionResult ListadoMenus()
        {
            var errormensaje = "";
            var resultado = new List<dynamic>();
            var listaxMenuPrincipal = new List<SEG_PermisoMenuEntidad>();
            try
            {
                var listaxMenuPrincipalTupla= webPermisoMenuBl.GetPermisoMenuRolId(Convert.ToInt32(Session["rol"]));
                listaxMenuPrincipal = listaxMenuPrincipalTupla.lista;

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Comuniquese con el Administrador";
            }


            return Json(new { dataResultado = listaxMenuPrincipal.ToList(), mensaje = errormensaje });
        }
    }
}