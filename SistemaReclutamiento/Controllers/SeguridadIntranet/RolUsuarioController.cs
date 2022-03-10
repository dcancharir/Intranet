using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Entidades.SeguridadIntranet;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.SeguridadIntranet;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.SeguridadIntranet
{
    [autorizacion(false)]
    public class RolUsuarioController : Controller
    {
        private SEG_RolUsuarioDAL web_RolUsuarioBL = new SEG_RolUsuarioDAL();
        private SEG_RolDAL webRolBl = new SEG_RolDAL();
        private UsuarioModel segUsuarioBl = new UsuarioModel();
        private SEG_RolUsuarioDAL webRolUsuarioBl = new SEG_RolUsuarioDAL();

        [HttpPost]
        public ActionResult GuardarRolUsuario(SEG_RolUsuarioEntidad rolUsuario)
        {
            var errormensaje = "Accion realizada Correctamente.";
            bool respuestaConsulta = false;
            bool deleteUsuario = false;
            try
            {
                var deleteUsuarioTupla = web_RolUsuarioBL.EliminarRolUsuario(rolUsuario.UsuarioID);
                deleteUsuario = deleteUsuarioTupla.error.Respuesta;
                rolUsuario.WEB_RUsuFechaRegistro = DateTime.Now;
                var respuestaConsultaTupla = web_RolUsuarioBL.GuardarRolUsuario(rolUsuario);
                respuestaConsulta = respuestaConsultaTupla.error.Respuesta;

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        //[seguridad(false)]
        [HttpPost]
        public ActionResult ListadoRolUsuario()
        {
            var errormensaje = "";
            var lista = new List<SEG_RolEntidad>();
            try
            {
                var listaTupla = webRolBl.GetRoles();
                lista=listaTupla.lista.OrderBy(x => x.WEB_RolNombre).ToList();

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = lista, mensaje = errormensaje });
        }

        //[seguridad(false)]
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
                listaRol = listaRolTupla.lista.OrderBy(x => x.WEB_RolNombre).ToList();
                var listaUsuTupla = segUsuarioBl.IntranetListarUsuariosJson();
                listaUsu = listaUsuTupla.listaUsuarios;
                var listaRolUsuarioTupla = webRolUsuarioBl.GetRolUsuario();
                listaRolUsuario = listaRolUsuarioTupla.lista;
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { roles = listaRol, usuarios = listaUsu.ToList(), rolUsuarios = listaRolUsuario.ToList(), mensaje = errormensaje });
        }
    }
}