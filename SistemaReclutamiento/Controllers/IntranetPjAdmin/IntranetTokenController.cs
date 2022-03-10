using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using RestSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SistemaReclutamiento.Entidades.IntranetPJ;
using SistemaReclutamiento.Models.IntranetPJ;
using SistemaReclutamiento.Utilitarios;

namespace SistemaReclutamiento.Controllers.IntranetPjAdmin
{
    [autorizacion(false)]
    public class IntranetTokenController : Controller
    {
        // GET: IntranetToken
        UsuarioModel usuariobl = new UsuarioModel();
        IntranetSistemasModel sistemasbl = new IntranetSistemasModel();
        IntranetEmpresaModel intranetempresabl = new IntranetEmpresaModel();
        SQLModel sqlbl = new SQLModel();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IntranetTokenListarUsuariosJson()
        {
            string errormensaje = "";
            bool response = false;
            List<UsuarioPersonaEntidad> listaPersonasPostgres = new List<UsuarioPersonaEntidad>();
            List<IntranetSistemasEntidad> listaSistemas = new List<IntranetSistemasEntidad>();
            List<IntranetEmpresaEntidad> listaEmpresas = new List<IntranetEmpresaEntidad>();
            List<PersonaSqlEntidad> listaPersonasSQL = new List<PersonaSqlEntidad>();
            //var listaIAS = new List<dynamic>();
            List<dynamic> listaTotal = new List<dynamic>();
            List<dynamic> lista = new List<dynamic>();
            try
            {
                //Listar Sistemas
                var sistemasTupla = sistemasbl.IntranetSistemasListarJson();
                if (sistemasTupla.error.Respuesta)
                {
                    listaSistemas = sistemasTupla.intranetSistemasLista;
                }
                else {
                    errormensaje += sistemasTupla.error.Mensaje;
                }

                //if (listaSistemas.Count > 0) {
                //    foreach (var sistema in listaSistemas) {
                //        var client = new RestClient(sistema.sist_ruta);
                //        var request = new RestRequest(Method.POST);
                //        IRestResponse responseAPI = client.Execute(request);
                //        dynamic jsonObj = JsonConvert.DeserializeObject(responseAPI.Content);
                //        foreach (var obj in jsonObj.data)
                //        {
                //            string NombreEmpleado = obj.NombreEmpleado;
                //            int UsuarioID = obj.UsuarioID;
                //            int EmpleadoID = obj.EmpleadoID;
                //            string UsuarioNombre = obj.UsuarioNombre;
                //            string Estado = obj.Estado;
                //            string Token = obj.UsuarioToken;
                //            lista.Add(new
                //            {
                //                NombreEmpleado,
                //                UsuarioID,
                //                EmpleadoID,
                //                UsuarioNombre,
                //                Estado,
                //                Token
                //            });
                //        }
                //        listaTotal.Add(lista);
                //    }
                //}


                //Lista de Empresas desde Postgres
                var listaEmpresasTupla = intranetempresabl.IntranetEmpresasListarJson();
                if (listaEmpresasTupla.error.Respuesta)
                {
                    listaEmpresas = listaEmpresasTupla.intranetEmpresasLista.Where(x => x.emp_estado == "A").ToList();
                    //creamos el string con la lista de empresas para el IN en sql
                    if (listaEmpresas.Count > 0)
                    {
                        //obtener mes anterior
                        var mes_anterior = DateTime.Now.Month - 1;
                        //Por lo menos hay una empresa registrada en int_empresa en Postgres
                        string stringEmpresas = "";
                        stringEmpresas += "(";
                        foreach (var m in listaEmpresas)
                        {
                            stringEmpresas += m.emp_codigo + ",";
                        }
                        stringEmpresas = stringEmpresas.Substring(0, stringEmpresas.Length - 1);
                        stringEmpresas += ")";
                        //Listado de Personas SQL
                        var listaPersonasSQLTupla = sqlbl.PersonaSQLObtenerListaAgendaJson(stringEmpresas, mes_anterior);
                        if (listaPersonasSQLTupla.error.Respuesta)
                        {
                            listaPersonasSQL = listaPersonasSQLTupla.lista;

                            //response = true;
                            //errormensaje = "Listando Agenda";
                        }
                        else
                        {
                            errormensaje += listaPersonasSQLTupla.error.Mensaje;
                        }

                        //Listado de PErsonas Postgres
                        var listaTupla = usuariobl.IntranetListarUsuariosTokenJson();
                        if (listaTupla.error.Respuesta)
                        {
                            listaPersonasPostgres = listaTupla.listaUsuarios;
                        }
                        else
                        {
                            errormensaje = listaTupla.error.Mensaje;
                        }

                        if (listaPersonasPostgres.Count > 0 && listaPersonasSQL.Count > 0)
                        {
                            foreach (var m in listaPersonasSQL) {
                                var contiene = listaPersonasPostgres.Where(x => x.per_numdoc.Equals(m.CO_TRAB)).FirstOrDefault();
                                if (contiene != null)
                                {
                                    lista.Add(new
                                    {
                                        per_numdoc = m.CO_TRAB,
                                        per_nombre = m.NO_TRAB,
                                        per_apellido_pat = m.NO_APEL_PATE,
                                        per_apellido_mat = m.NO_APEL_MATE,
                                        usu_nombre = contiene.usu_nombre,
                                        per_id = contiene.per_id,
                                        usu_token = contiene.usu_token,
                                        usu_exp_token = contiene.usu_exp_token,
                                        usu_id = contiene.usu_id
                                    });
                                }
                            }
                            errormensaje = "Listando Usuarios";
                            response = true;
                        }
                    }
                    else
                    {
                        errormensaje = "No hay Empresas";
                    }
                }
                else
                {
                    errormensaje = listaEmpresasTupla.error.Mensaje;
                }


              
            }
            catch (Exception ex)
            {
                errormensaje = ex.Message;
            }
            return Json(new { data_sistemas= listaSistemas,data = lista.ToList(), respuesta = response, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult IntranetListarUsuariosSistemasJson(List<IntranetSistemasEntidad> listaSistemas,string[] listaDNIs, string[] listaTokens)
        {
            
            List<dynamic> lista2 = new List<dynamic>();
            List<dynamic> listaTotal = new List<dynamic>();
            List<string> listaDni = listaDNIs.ToList();
            List<string> listaToken = listaTokens.ToList();
            List<dynamic> listaporSistema = new List<dynamic>();
            string NombreEmpleado = "";
            int UsuarioID = 0;
            int EmpleadoID = 0;
            string UsuarioNombre = "";
            string Estado = "";
            string Token = "";
            string DOI = "";
            string errormensaje = "";
            int indiceCoincidencia = 0;
            string TokenPostgres = "";
            bool response = false;
            try
            {
                if (listaSistemas.Count > 0)
                {
                    foreach (var sistema in listaSistemas)
                    {
                        List<dynamic> lista = new List<dynamic>();
                        var client = new RestClient(sistema.sist_ruta);
                        var request = new RestRequest(Method.POST);
                        IRestResponse responseAPI = client.Execute(request);
                        dynamic jsonObj = JsonConvert.DeserializeObject(responseAPI.Content);
                        foreach (var obj in jsonObj.data)
                        {

                            NombreEmpleado = obj.NombreEmpleado;
                            UsuarioID = obj.UsuarioID;
                            EmpleadoID = obj.EmpleadoID;
                            UsuarioNombre = obj.UsuarioNombre;
                            Estado = obj.Estado;
                            Token = obj.UsuarioToken;
                            DOI = obj.DOI;
                            indiceCoincidencia = listaDni.IndexOf(DOI);
                            if (indiceCoincidencia >= 0)
                            {
                                lista.Add(new
                                {
                                    NombreEmpleado,
                                    UsuarioID,
                                    EmpleadoID,
                                    UsuarioNombre,
                                    Estado,
                                    Token,
                                    TokenPostgres=listaToken[indiceCoincidencia].ToString(),
                                    DOI
                                });
                            }
                          
                        }
                        listaTotal.Add(new {
                            sistema.sist_id,
                            sistema.sist_nombre,
                            sistema.sist_ruta,
                            sistema.sist_descripcion,
                            sistema.sist_estado,
                            usuarios=lista
                        });
                    }
                    errormensaje = "Listando Data";
                    response = true;
                }
                else {
                    errormensaje = "Lista de Sistemas Vacia";
                }
            }
            catch (Exception ex) {
                errormensaje = ex.Message;
            }

            return Json(new { data = listaTotal.ToList(), mensaje=errormensaje,respuesta=response });
        }
        [HttpPost]
        public ActionResult IntranetModificarTokensporSistemaJson(List<IntranetSistemasEntidad> listasistemas) {
            string errormensaje = "";
            bool response = false;
            List<SEG_Usuario> listaUsuarios = new List<SEG_Usuario>();
            List<dynamic> listaTotal = new List<dynamic>();
            string NombreEmpleado = "";
            int UsuarioID = 0;
            int EmpleadoID = 0;
            string UsuarioNombre = "";
            string Estado = "";
            string TokenNuevo = "";
            string TokenAntiguo = "";
            string DOI = "";
            int indiceCoincidencia = 0;
            string TokenPostgres = "";
            try
            {
                if (listasistemas.Count > 0)
                {
                    foreach (var sistema in listasistemas)
                    {
                        List<dynamic> lista = new List<dynamic>();
                        string[] word = sistema.sist_ruta.Split('/');
                        string ruta = word[0] + "//" + word[2] + "/" + word[3];

                        var client = new RestClient(ruta);

                        var request = new RestRequest("/UsuarioEditarTokenAccesoIntranetJson",Method.POST);
                        //request.AddParameter("applitacion/json; chartset=utf-8", sistema.usuarios, ParameterType.RequestBody);
                        //request.RequestFormat = DataFormat.Json;
                        //IRestResponse responseAPI = client.Execute(request);
                        if (sistema.usuarios.Count > 0) {
                            foreach (var usuario in sistema.usuarios) {
                                usuario.Token = usuario.TokenPostgres;
                            }
                        }
                        request.AddParameter("application/json", JsonConvert.SerializeObject(sistema.usuarios), ParameterType.RequestBody);

                        IRestResponse restResponse = client.Execute(request);

                        //IRestResponse responseData = SimpleJson.DeserializeObject<IRestResponse>(restResponse.Content);
                        dynamic jsonObj = JsonConvert.DeserializeObject(restResponse.Content);
                        foreach (var obj in jsonObj.data)
                        {
                            NombreEmpleado = obj.NombreEmpleado;
                            UsuarioID = obj.UsuarioID;
                            EmpleadoID = obj.EmpleadoID;
                            UsuarioNombre = obj.UsuarioNombre;
                            Estado = obj.Estado;
                            TokenNuevo = obj.TokenNuevo;
                            TokenAntiguo = obj.TokenAntiguo;
                            DOI = obj.DOI;
                            lista.Add(new
                            {
                                NombreEmpleado,
                                UsuarioID,
                                EmpleadoID,
                                UsuarioNombre,
                                Estado,
                                TokenNuevo,
                                TokenAntiguo,
                                DOI
                            });
                        }
                        listaTotal.Add(new
                        {
                            sistema.sist_id,
                            sistema.sist_nombre,
                            sistema.sist_ruta,
                            sistema.sist_descripcion,
                            sistema.sist_estado,
                            usuarios = lista
                        });
                    }
                    response = true;
                    errormensaje = "Editado";
                }
            }
            catch (Exception ex) {
                errormensaje = ex.Message;
            }
            return Json(new { listaSistemas=listasistemas,listaTotal,mensaje=errormensaje,respuesta=response});
        }
    }
}