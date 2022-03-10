using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using SistemaReclutamiento.Entidades.SeguridadIntranet;
using SistemaReclutamiento.Models.SeguridadIntranet;
using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;

namespace SistemaReclutamiento.Controllers.SeguridadIntranet
{
    [autorizacion]
    public class SeguridadIntranetController : Controller
    {
        private SEG_RolDAL webRolBl = new SEG_RolDAL();
        private SEG_PermisoRolDAL webPermisoRolBl = new SEG_PermisoRolDAL();
        private SEG_PermisoDAL webPermisoBl = new SEG_PermisoDAL();
        private SEG_PermisoMenuDAL webPermisoMenuBl = new SEG_PermisoMenuDAL();
        private SEG_RolUsuarioDAL webRolUsuarioBl = new SEG_RolUsuarioDAL();
        private UsuarioModel webUsuarioBl = new UsuarioModel();

        [autorizacion(false)]
        public ActionResult SeguridadIntranetVista()
        {
            ViewBag.rolId = Session["rol"];
            //ViewBag.rolId = 1;
            return View("~/Views/SeguridadIntranet/SeguridadIntranetVista.cshtml");
        }

        [HttpPost]
        public ActionResult AgregarPermisoRol(List<SEG_PermisoRolEntidad> webPermiso)
        {
            var errormensaje = "Accion realizada Correctamente.";
            bool respuestaConsulta = false;
            try
            {
                foreach (var permiso in webPermiso)
                {
                    permiso.WEB_PRolFechaRegistro = DateTime.Now;
                    var respuestaTupla = webPermisoRolBl.GuardarPermisoRol(permiso);
                    if (respuestaTupla.error.Respuesta)
                    {
                        respuestaConsulta = respuestaTupla.respuesta;
                    }
                    else {
                        errormensaje = respuestaTupla.error.Mensaje;
                    }
                }


            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult QuitarPermisoRol(List<SEG_PermisoRolEntidad> webPermiso)
        {
            var errormensaje = "Accion realizada Correctamente.";
            bool respuestaConsulta = false;
            try
            {
                foreach (var permiso in webPermiso)
                {
                    var WEB_PermID = permiso.WEB_PermID;
                    var WEB_RolID = permiso.WEB_RolID;
                    var respuestaTupla= webPermisoRolBl.EliminarPermisoRol(WEB_PermID, WEB_RolID);
                    if (respuestaTupla.error.Respuesta) {
                        respuestaConsulta = respuestaTupla.respuesta;
                    }
                    else
                    {
                        errormensaje = respuestaTupla.error.Mensaje;
                    }
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }


        [HttpPost]
        public ActionResult AgregarPermisoMenu(SEG_PermisoMenuEntidad webPermisoMenu)
        {
            var errormensaje = "Accion realizada Correctamente.";
            bool respuestaConsulta = false;
            try
            {
                webPermisoMenu.WEB_PMeFechaRegistro = DateTime.Now;
                var respuestaTupla= webPermisoMenuBl.GuardarPermisoMenu(webPermisoMenu);
                if (respuestaTupla.error.Respuesta) {
                    respuestaConsulta = respuestaTupla.respuesta;
                }
                else{
                    errormensaje = respuestaTupla.error.Mensaje;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult QuitarPermisoMenu(SEG_PermisoMenuEntidad webPermisoMenu)
        {
            var errormensaje = "Accion realizada Correctamente.";
            bool respuestaConsulta = false;
            try
            {
                var WEB_PMeDataMenu = webPermisoMenu.WEB_PMeDataMenu;
                var WEB_RolID = webPermisoMenu.WEB_RolID;
                var respuestaTupla= webPermisoMenuBl.EliminarPermisoMenu(WEB_PMeDataMenu, WEB_RolID);
                if (respuestaTupla.error.Respuesta)
                {
                    respuestaConsulta = respuestaTupla.respuesta;
                }
                else {
                    errormensaje = respuestaTupla.error.Mensaje;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [autorizacion(false)]
        [HttpPost]
        public ActionResult ListadoFechasPrincipales()
        {

            var errormensaje = "";
            var listaxMenuPrincipal = new List<SEG_PermisoMenuEntidad>();
            try
            {
                var listaTupla= webPermisoMenuBl.GetPermisoFechaMax();
                if (listaTupla.error.Respuesta)
                {
                    listaxMenuPrincipal = listaTupla.lista;
                }
                else {
                    errormensaje = listaTupla.error.Mensaje;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Comuniquese con el Administrador";
            }

            return Json(new { data = listaxMenuPrincipal.ToList(), mensaje = errormensaje });
        }

        [autorizacion(false)]
        [HttpPost]
        public ActionResult ListadoRolesSeguridadPermiso()
        {
            var errormensaje = "";
            var listaRol = new List<SEG_RolEntidad>();
            try
            {
                var listaRolTupla = webRolBl.GetRoles();
                if (listaRolTupla.error.Respuesta) {
                    listaRol = listaRolTupla.lista.OrderBy(x => x.WEB_RolNombre).ToList();
                }
                else
                {
                    errormensaje = listaRolTupla.error.Mensaje;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }

            return Json(new { roles = listaRol, mensaje = errormensaje });
        }

        [autorizacion(false)]
        [HttpPost]
        public ActionResult ListadoMenusRolId(int rolId)
        {
            var errormensaje = "";
            var resultado = new List<dynamic>();
            var listaxMenuPrincipal = new List<SEG_PermisoMenuEntidad>();
            try
            {
                var listaxMenuTupla= webPermisoMenuBl.GetPermisoMenuRolId(rolId);
                if (listaxMenuTupla.error.Respuesta)
                {
                    listaxMenuPrincipal = listaxMenuTupla.lista;
                }
                else
                {
                    errormensaje = listaxMenuTupla.error.Mensaje;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Comuniquese con el Administrador";
            }


            return Json(new { dataResultado = listaxMenuPrincipal.ToList(), mensaje = errormensaje });
        }

        [autorizacion(false)]
        [HttpPost]
        public ActionResult ListadoControladorPermisos(int rolid)
        {
            var errormensaje = "";

            var listaPermisos = new List<SEG_PermisoEntidad>();
            var listaPermisosRol = new List<SEG_PermisoRolEntidad>();

            try
            {

                var listaPermisosTupla= webPermisoBl.GetPermisosActivos();
                var listaPermisosRolTupla= webPermisoRolBl.GetPermisoRolrolid(rolid);
                if (listaPermisosTupla.error.Respuesta && listaPermisosRolTupla.error.Respuesta)
                {
                    listaPermisos = listaPermisosTupla.lista;
                    listaPermisosRol = listaPermisosRolTupla.lista;
                }
                else
                {
                    errormensaje = listaPermisosTupla.error.Mensaje + " ---- " + listaPermisosRolTupla.error.Mensaje;
                }


            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }

            var cabeceras = listaPermisos.GroupBy(x => new { x.WEB_PermControlador }).Select(g => new { g.Key.WEB_PermControlador }).OrderBy(x => x.WEB_PermControlador);
            var permisos = listaPermisos.OrderBy(x => x.WEB_PermNombreR).ToList();
            var permisosRol = listaPermisosRol.ToList();
            return Json(new { controlador = cabeceras.ToList(), listaPermisoControlador = permisos, listaPermisosRol = permisosRol, mensaje = errormensaje });

        }

        [autorizacion(false)]
        [HttpPost]
        public ActionResult ListadoPermisos()
        {
            var errormensaje = "";
            var listaRol = new List<SEG_RolEntidad>();
            var listaPermisos = new List<SEG_PermisoEntidad>();
            var listaPermisosRol = new List<SEG_PermisoRolEntidad>();
            var listaRolUsuario = new List<SEG_RolUsuarioEntidad>();
            try
            {
                var listaRolTupla = webRolBl.GetRoles();
                var listaPermisosTupla = webPermisoBl.GetPermisosActivos();
                var listaPermisosRolTupla = webPermisoRolBl.GetPermisoRol();
                var listaRolUsuarioTupla = webRolUsuarioBl.GetRolUsuario();
                if(listaRolTupla.error.Respuesta && listaPermisosTupla.error.Respuesta && listaPermisosRolTupla.error.Respuesta&& listaRolUsuarioTupla.error.Respuesta)
                {
                    listaRol = listaRolTupla.lista.OrderBy(x => x.WEB_RolNombre).ToList();
                    listaPermisos = listaPermisosTupla.lista;
                    listaPermisosRol = listaPermisosRolTupla.lista;
                    listaRolUsuario = listaRolUsuarioTupla.lista;
                }
                else
                {
                    errormensaje = listaRolTupla.error.Mensaje + " -- " + listaPermisosTupla.error.Mensaje + " -- " + listaPermisosRolTupla.error.Mensaje + " -- " + listaRolUsuarioTupla.error.Mensaje; 
                }
               

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            var cabeceras = listaPermisos.GroupBy(x => new { x.WEB_PermControlador }).Select(g => new { g.Key.WEB_PermControlador }).OrderBy(x => x.WEB_PermControlador);
            var permisos = listaPermisos.OrderBy(x => x.WEB_PermNombreR).ToList();
            return Json(new { roles = listaRol, modulos = cabeceras.ToList(), permisos = permisos, permisosRol = listaPermisosRol.ToList(), mensaje = errormensaje });
        }

        [autorizacion(false)]
        [HttpPost]
        public ActionResult ListadoRolUsuario()
        {
            var errormensaje = "";
            var lista = new List<SEG_RolEntidad>();
            try
            {

                var listaTupla = webRolBl.GetRoles();
                if (listaTupla.error.Respuesta)
                {
                    lista = listaTupla.lista.OrderBy(x => x.WEB_RolNombre).ToList();
                }
                else
                {
                    errormensaje = listaTupla.error.Mensaje;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = lista, mensaje = errormensaje });
        }

        [autorizacion(false)]
        [HttpPost]
        public ActionResult ListadoTableUsuarioAsignarRol()
        {
            var errormensaje = "";
            var listaRol = new List<SEG_RolEntidad>();
            var listaUsu = new List<UsuarioPersonaEntidad>();
            var listaRolUsuario = new List<SEG_RolUsuarioEntidad>();
            try
            {
                var listaRolTupla = webRolBl.GetRoles();
                var listaUsuTupla = webUsuarioBl.IntranetListarUsuariosTokenJson();//crear metodo para listar empleados
                var listaRolUsuarioTupla = webRolUsuarioBl.GetRolUsuario();
                if (listaRolTupla.error.Respuesta && listaUsuTupla.error.Respuesta&&listaRolUsuarioTupla.error.Respuesta)
                {
                    listaRol = listaRolTupla.lista.OrderBy(x => x.WEB_RolNombre).ToList();
                    listaUsu = listaUsuTupla.listaUsuarios;
                    listaRolUsuario = listaRolUsuarioTupla.lista;
                }
                else
                {
                    errormensaje = listaRolTupla.error.Mensaje + " -- " + listaUsuTupla.error.Mensaje + " -- " + listaRolUsuarioTupla.error.Mensaje;
                }
              
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { roles = listaRol, usuarios = listaUsu.ToList(), rolUsuarios = listaRolUsuario.ToList(), mensaje = errormensaje });
        }
             #region ActualizarMétodosBD con métodos en controladores

        [autorizacion(false)]
        [HttpGet]
        public ActionResult ActualizarMetodos()
        {
            List<string> registrosborrados = new List<string>();
            var nuevalista = GetMetodos();   ///metodos en CONTROLADORES
            bool respuestaConsulta = false;
            var errormensaje = "";
            //var nuevalista2 = MetodosLista_List();
            var listapermisosbd = new List<SEG_PermisoEntidad>();
            var listapermisosbdTupla = webPermisoBl.GetPermisos();/////registros en  SEG_Permiso
            if (listapermisosbdTupla.error.Respuesta)
            {
                listapermisosbd = listapermisosbdTupla.lista;
                bool seguridad = true;
                foreach (var registro in listapermisosbd)
                {
                    seguridad = true;
                    string nombrepermiso = registro.WEB_PermNombre;
                    string nombrecontrolador = registro.WEB_PermControlador;
                    var permiso = new SEG_PermisoEntidad();

                    var nuevalistalinq = ((IEnumerable<dynamic>)nuevalista).Cast<dynamic>();
                    var metodolinq = nuevalista.Where(a => a.Action == nombrepermiso && a.Controller == nombrecontrolador).SingleOrDefault();///buscar registro bd en   METODOS CONTROLADORES

                    try
                    {
                        if (metodolinq != null)
                        {
                            var atributosobjeto = metodolinq.AttributesMetodo;
                            var atributosControladorobjeto = metodolinq.AttributesControlador;
                            ///atributos de controlador , si hubiera sido definido
                            var atributoscontroladorlinq = ((IEnumerable<dynamic>)atributosControladorobjeto).Cast<dynamic>();
                            if (atributoscontroladorlinq != null)
                            {
                                var seguridadcontrolador = atributoscontroladorlinq.Where(x => x.AttributeType.Name == "autorizacion").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                                if (seguridadcontrolador != null)
                                {
                                    seguridad = atributoscontroladorlinq.Where(x => x.AttributeType.Name == "autorizacion").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                                }
                            }
                            ///atributos de metodo,  reemplazan atributos de controlador
                            var objetolinq = ((IEnumerable<dynamic>)atributosobjeto).Cast<dynamic>();
                            if (objetolinq != null)
                            {
                                var seguridaddelobjeto = objetolinq.Where(x => x.AttributeType.Name == "autorizacion").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                                if (seguridaddelobjeto != null)
                                {
                                    seguridad = seguridaddelobjeto;
                                }

                            }
                        }
                        if (!seguridad)///si seguridad es false debido a controlador=>si controlador con seguridad false y metodo sin atributo seguridad
                        {
                            webPermisoBl.BorrarPermiso(nombrepermiso, nombrecontrolador);
                            registrosborrados.Add(nombrecontrolador + "/" + nombrepermiso);
                        }
                        var permisoexistectrl = nuevalista.Where(a => a.Action == nombrepermiso && a.Controller == nombrecontrolador);
                        int existepermisoencontrolador = permisoexistectrl.Count();
                        //respuestaConsulta = webPermisoBl.GetPermisoId(nombrepermiso);
                        if (existepermisoencontrolador == 0)//Permiso existe en tabla  pero no en controlador  => delete 
                        {//borrar
                            webPermisoBl.BorrarPermiso(nombrepermiso, nombrecontrolador);
                            registrosborrados.Add(nombrecontrolador + "/" + nombrepermiso);
                        }

                    }
                    catch (Exception exp)
                    {
                        errormensaje = exp.Message + " ,Llame Administrador";
                        return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
                    }

                }
            }
            else
            {
                errormensaje = listapermisosbdTupla.error.Mensaje;
            }
          
           
            //return Json(new { respuesta = "", mensaje = errormensaje });

            return Json(new { borrados = registrosborrados }, JsonRequestBehavior.AllowGet);

        }

        [autorizacion(false)]
        public List<dynamic> GetMetodos()
        {
            Assembly asm = Assembly.GetAssembly(typeof(SistemaReclutamiento.MvcApplication));
            var lista = asm.GetTypes()
                        .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                        .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                        .Where(m => (System.Attribute.GetCustomAttributes(typeof(autorizacion), true).Length > 0))
                        .Where(m => (String.Join(",", (m.GetCustomAttributesData()
                                                                .Where(f => f.Constructor.DeclaringType.Name == "autorizacion")
                                                                .Select(c => (c.ConstructorArguments[0].Value))))
                                                          != "False"))
                        .Select(x => new
                        {
                            Controller = x.DeclaringType.Name,
                            Action = x.Name,
                            ReturnType = x.ReturnType.Name,
                            Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))),



                            AttributesControlador = x.DeclaringType.GetCustomAttributesData(),
                            AttributesControladorString = String.Join(",", x.DeclaringType.GetCustomAttributesData().Select(
                                         a => String.Join(",", a.ConstructorArguments.Select(b => a.AttributeType.Name + " = " + b.Value))
                                )),
                            AttributesMetodo = x.GetCustomAttributesData(),
                            AttributesMetodostring = String.Join(",", x.GetCustomAttributesData()
                                  .Select(
                                         a => String.Join(",", a.ConstructorArguments.Select(b => a.AttributeType.Name + " = " + b.Value))
)),

                        })
                        .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();
            return lista.ToList<dynamic>(); ;

        }

        /// <summary>
        /// MetodosLista_List   RETURNS LISTA de métodos declarados con seguridad  de los CONTROLADORES
        /// </summary>
        /// <returns></returns>
        /// 



        [autorizacion(false)]
        public List<dynamic> MetodosLista_List()
        {
            Assembly asm = Assembly.GetAssembly(typeof(SistemaReclutamiento.MvcApplication));
            var lista = asm.GetTypes()
                        .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                        .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                        .Where(m => (System.Attribute.GetCustomAttributes(typeof(autorizacion), true).Length > 0))
                        .Where(m => (String.Join(",", (m.GetCustomAttributesData()
                                                                .Where(f => f.Constructor.DeclaringType.Name == "seguridad")
                                                                .Select(c => (c.ConstructorArguments[0].Value))))
                                                          != "False"))
                        .Select(x => new
                        {
                            Controller = x.DeclaringType.Name,
                            Action = x.Name,
                            ReturnType = x.ReturnType.Name,
                            Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))),
                            AttributesControlador = x.DeclaringType.GetCustomAttributesData(),
                            AttributesControladorString = String.Join(",", x.DeclaringType.GetCustomAttributesData().Select(
                                         a => String.Join(",", a.ConstructorArguments.Select(b => a.AttributeType.Name + " = " + b.Value))
                                )),
                            AttributesMetodo = x.GetCustomAttributesData(),
                            AttributesMetodostring = String.Join(",", x.GetCustomAttributesData()
                                  .Select(
                                         a => String.Join(",", a.ConstructorArguments.Select(b => a.AttributeType.Name + " = " + b.Value))
)),

                        })
                        .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();
            return lista.ToList<dynamic>();

        }

        /// <summary>
        /// MetodosLista_Objeto(nombrecontrolador,nombremetodo)
        /// </summary>
        /// <param name="control"></param>
        /// <param name="accion"></param>
        /// <returns></returns>
        [autorizacion(false)]
        public Metodo_atributos Metodo_Objeto(string control, string accion)
        {
            Assembly asm = Assembly.GetAssembly(typeof(SistemaReclutamiento.MvcApplication));
            var metodoactual = asm.GetTypes()
                        .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                        .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                        //.Where(m => (System.Attribute.GetCustomAttributes(typeof(seguridad), true).Length > 0))
                        //.Where(m => (String.Join(",", (m.GetCustomAttributesData()
                        //                                        .Where(f => f.Constructor.DeclaringType.Name == "seguridad")
                        //                                        .Select(c => (c.ConstructorArguments[0].Value))))
                        //                                  != "False"))
                        .Where(x => (x.DeclaringType.Name).ToUpper() == (control + "Controller").ToUpper())   ////correccion
                        .Where(x => (x.Name).ToUpper() == accion.ToUpper())
                        .Select(x => new Metodo_clase
                        {
                            Controller = x.DeclaringType.Name,
                            Action = x.Name,
                            ReturnType = x.ReturnType.Name,
                            Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))),
                            AttributesControlador = x.DeclaringType.GetCustomAttributesData(),
                            AttributesControladorString = String.Join(",", x.DeclaringType.GetCustomAttributesData().Select(a => String.Join(",", a.ConstructorArguments.Select(b => a.AttributeType.Name + " = " + b.Value)))),
                            AttributesMetodo = x.GetCustomAttributesData(),
                            AttributesMetodostring = String.Join(",", x.GetCustomAttributesData()
                                  .Select(
                                         a => String.Join(",", a.ConstructorArguments.Select(b => a.AttributeType.Name + " = " + b.Value))
                            )),

                        })
                        .OrderBy(x => x.Controller).ThenBy(x => x.Action).FirstOrDefault();

            var atributosobjeto = metodoactual.AttributesMetodo;
            var atributosControladorobjeto = metodoactual.AttributesControlador;
            bool seguridad = true;///por defecto todos los metodos de  nuevalista  vienen con seguridad
            string modulo = "";
            string descripcion = "";
            bool atributoseguridaddefinido = false;
            bool atributomodulodefinido = false;
            bool atributodescripcion = false;

            foreach (var atributo in atributosobjeto)
            {
                if (atributo.AttributeType.Name == "autorizacion")
                {
                    seguridad = (bool)atributo.ConstructorArguments[0].Value;
                    atributoseguridaddefinido = true;
                }
                if (atributo.AttributeType.Name == "modulo")
                {
                    modulo = (string)atributo.ConstructorArguments[0].Value;
                    atributomodulodefinido = true;
                }
                if (atributo.AttributeType.Name == "descripcion")
                {
                    descripcion = (string)atributo.ConstructorArguments[0].Value;
                    atributodescripcion = true;
                }
            }
            ///atributos de controlador , si hubiera sido definido
            var atributoscontroladorlinq = ((IEnumerable<dynamic>)atributosControladorobjeto).Cast<dynamic>();
            if (atributoscontroladorlinq != null)
            {
                var seguridadcontrolador = atributoscontroladorlinq.Where(x => x.AttributeType.Name == "autorizacion").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                if (seguridadcontrolador != null)
                {
                    seguridad = atributoscontroladorlinq.Where(x => x.AttributeType.Name == "autorizacion").Select(x => x.ConstructorArguments[0].Value
                                           ).SingleOrDefault();
                }
                var modulocontrolador = atributoscontroladorlinq.Where(x => x.AttributeType.Name == "modulo").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                if (modulocontrolador != null)
                {
                    modulo = modulocontrolador;
                }
                var descripcioncontrolador = atributoscontroladorlinq.Where(x => x.AttributeType.Name == "descripcion").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                if (descripcioncontrolador != null)
                {
                    descripcion = descripcioncontrolador;
                }
            }
            ///atributos de metodo,  reemplazan atributos de controlador
            var objetolinq = ((IEnumerable<dynamic>)atributosobjeto).Cast<dynamic>();
            if (objetolinq != null)
            {
                var seguridaddelobjeto = objetolinq.Where(x => x.AttributeType.Name == "autorizacion").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                if (seguridaddelobjeto != null)
                {
                    seguridad = seguridaddelobjeto;
                }
                var modulodelobjeto = objetolinq.Where(x => x.AttributeType.Name == "modulo").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                if (modulodelobjeto != null)
                {
                    modulo = modulodelobjeto;
                }
                var descripciondelobjeto = objetolinq.Where(x => x.AttributeType.Name == "descripcion").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                if (descripciondelobjeto != null)
                {
                    descripcion = descripciondelobjeto;
                }
            }

            Metodo_atributos MetodoAtributos = new Metodo_atributos();
            MetodoAtributos.Controlador = control;
            MetodoAtributos.Metodo = accion;
            MetodoAtributos.seguridad = seguridad;
            MetodoAtributos.modulo = modulo;
            MetodoAtributos.descripcion = descripcion;


            return MetodoAtributos;
        }
        [autorizacion(false)]
        public ActionResult updateMethod()
        {
            ActualizarMetodos();
            var nuevalista = MetodosLista_List();
            var errormensaje = "";
            bool respuestaConsulta = false;

            foreach (var item in nuevalista)
            {
                var permiso = new SEG_PermisoEntidad();
                try
                {
                    var atributosobjeto = item.AttributesMetodo;
                    var atributosControladorobjeto = item.AttributesControlador;
                    bool seguridad = true;///por defecto todos los metodos de  nuevalista  vienen con seguridad
                    string modulo = "";
                    bool atributoseguridaddefinido = false;
                    bool atributomodulodefinido = false;
                    foreach (var atributo in atributosobjeto)
                    {
                        if (atributo.AttributeType.Name == "autorizacion")
                        {
                            seguridad = (bool)atributo.ConstructorArguments[0].Value;
                            atributoseguridaddefinido = true;
                        }
                        if (atributo.AttributeType.Name == "modulo")
                        {
                            modulo = (string)atributo.ConstructorArguments[0].Value;
                            atributomodulodefinido = true;
                        }

                    }
                    ///atributos de controlador , si hubiera sido definido
                    var atributoscontroladorlinq = ((IEnumerable<dynamic>)atributosControladorobjeto).Cast<dynamic>();
                    if (atributoscontroladorlinq != null)
                    {
                        var seguridadcontrolador = atributoscontroladorlinq.Where(x => x.AttributeType.Name == "autorizacion").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                        if (seguridadcontrolador != null)
                        {
                            seguridad = atributoscontroladorlinq.Where(x => x.AttributeType.Name == "autorizacion").Select(x => x.ConstructorArguments[0].Value
                                                   ).SingleOrDefault();
                        }
                        var modulocontrolador = atributoscontroladorlinq.Where(x => x.AttributeType.Name == "modulo").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                        if (modulocontrolador != null)
                        {
                            modulo = modulocontrolador;
                        }
                    }
                    ///atributos de metodo,  reemplazan atributos de controlador
                    var objetolinq = ((IEnumerable<dynamic>)atributosobjeto).Cast<dynamic>();
                    if (objetolinq != null)
                    {
                        var seguridaddelobjeto = objetolinq.Where(x => x.AttributeType.Name == "autorizacion").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                        if (seguridaddelobjeto != null)
                        {
                            seguridad = seguridaddelobjeto;
                        }
                        var modulodelobjeto = objetolinq.Where(x => x.AttributeType.Name == "modulo").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                        if (modulodelobjeto != null)
                        {
                            modulo = modulodelobjeto;
                        }
                    }

                    permiso.WEB_ModuloNombre = modulo;
                    permiso.WEB_PermNombre = item.Action;
                    permiso.WEB_PermTipo = item.Attributes;
                    permiso.WEB_PermControlador = item.Controller;
                    permiso.WEB_PermEstado = "1";
                    if (seguridad)///guardar solo si seguridad es true 
                    {
                        var respuestaTupla= webPermisoBl.GuardarPermiso(permiso);
                        if (respuestaTupla.error.Respuesta)
                        {
                            respuestaConsulta = respuestaTupla.respuesta;
                        }
                        else
                        {
                            errormensaje = respuestaTupla.error.Mensaje;
                        }
                    }
                }
                catch (Exception exp)
                {
                    errormensaje = exp.Message + " ,Llame Administrador";
                    return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
                }
            }

            errormensaje = "";
            var listaPermisos = new List<SEG_PermisoEntidad>();
            try
            {
                var listaPermisosTupla = webPermisoBl.GetPermisos();
                if (listaPermisosTupla.error.Respuesta)
                {
                    listaPermisos = listaPermisosTupla.lista;
                }
                else
                {
                    errormensaje = listaPermisosTupla.error.Mensaje;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }

            var cabeceras = listaPermisos.GroupBy(x => new { x.WEB_PermControlador }).Select(g => new { g.Key.WEB_PermControlador }).OrderBy(x => x.WEB_PermControlador).ToList();
            return Json(new { respuesta = listaPermisos }, JsonRequestBehavior.AllowGet);

        }
        #endregion
    }
}