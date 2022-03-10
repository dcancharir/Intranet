using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Entidades.IntranetPJ;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.IntranetPJ;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using RestSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;

namespace SistemaReclutamiento.Controllers.IntranetPJ
{
    [autorizacion(false)]
    public class IntranetPJController : Controller
    {
        IntranetSeccionModel intraSeccionBL = new IntranetSeccionModel();
        IntranetElementoModel intraElementobl = new IntranetElementoModel();
        IntranetDetalleElementoModel intraDetalleElementobl = new IntranetDetalleElementoModel();
        IntranetMenuModel intranetMenubl = new IntranetMenuModel();
        IntranetElementoModalModel intranetElementoModalbl = new IntranetElementoModalModel();
        IntranetDetalleElementoModalModel intranetDetalleElementoModalbl = new IntranetDetalleElementoModalModel();
        IntranetSeccionElementoModel intranetSeccionImagenbl = new IntranetSeccionElementoModel();
        IntranetActividadesModel intranetActividadesbl = new IntranetActividadesModel();
        PersonaModel personabl = new PersonaModel();
        IntranetSaludoCumpleaniosModel intranetSaludoCumpleaniosbl = new IntranetSaludoCumpleaniosModel();
        IntranetCPJLocalModel intranetCPJLocalbl = new IntranetCPJLocalModel();
        IntranetFooterModel intranetFooterbl = new IntranetFooterModel();
        SQLModel sqlbl = new SQLModel();
        IntranetEmpresaModel intranetempresabl = new IntranetEmpresaModel();
        //Acceso
        IntranetAccesoModel usuarioAccesobl = new IntranetAccesoModel();
        UsuarioModel usuariobl = new UsuarioModel();
        // GET: IntranetPJ
        RutaImagenes rutaImagenes = new RutaImagenes();
        //string[] TiposDocumentoOFIPLAN = { "BRE", "DNI", "PAS", "CEX", "AFP" };
        string PathActividadesIntranet = ConfigurationManager.AppSettings["PathArchivosIntranet"].ToString();

        //API key
        public readonly string ApiKey = ConfigurationManager.AppSettings["tokenApiApuesta"].ToString();
        public ActionResult Index(int? menu)
        {
            claseError error = new claseError();
            var menuTupla = intranetMenubl.IntranetMenuListarJson();
            int? id = 0;
            if (menu != 0)
            {
                id = menu;
            }
            else
            {
                error = menuTupla.error;
                if (error.Respuesta)
                {
                    if (menuTupla.intranetMenuLista.Count > 0)
                    {
                        var elementomenu = menuTupla.intranetMenuLista.First();
                        id = elementomenu.menu_id;
                    }
                    else
                    {
                        id = 0;
                    }

                }
                else
                {
                    id = -1;
                }
            }
            ViewBag.menu_id = id;
            return View("~/Views/IntranetPJ/IntranetPJIndex.cshtml");
        }

        [HttpPost]
        public ActionResult ListarDataJson(int menu_id)
        {

            //listar menus, cumpleaños, actividades, informacion de la Empresa, comentarios y Seccion PJ NEWS para el layout
            List<IntranetSeccionEntidad> intranetSeccion = new List<IntranetSeccionEntidad>();
            List<IntranetMenuEntidad> intranetMenu = new List<IntranetMenuEntidad>();
            List<IntranetActividadesEntidad> intranetActividades_ = new List<IntranetActividadesEntidad>();
            List<IntranetActividadesEntidad> intranetActividades = new List<IntranetActividadesEntidad>();
            List<IntranetSaludoCumpleanioEntidad> intraSaludos = new List<IntranetSaludoCumpleanioEntidad>();
            List<IntranetFooterEntidad> intranetFooter = new List<IntranetFooterEntidad>();

            //List<PersonaEntidad> listaPersona = new List<PersonaEntidad>();
            claseError error = new claseError();

            //Para rotulado de noticias
            List<Tuple<DateTime ,string ,string >> listaNoticias = new List<Tuple<DateTime, string ,string>>();
            List<Tuple<DateTime, string, string>> listaNoticiasDesordenado = new List<Tuple<DateTime, string, string>>();
            Random randNum = new Random();
            var ListaSeccion = new List<dynamic>();

            //Para listado de cumpleaños
            List<PersonaSqlEntidad> listaPersonasSQL = new List<PersonaSqlEntidad>();
            List<IntranetEmpresaEntidad> listaEmpresas = new List<IntranetEmpresaEntidad>();
            List<PersonaEntidad> listaPersonasPostgres = new List<PersonaEntidad>();
            var listaPersona = new List<dynamic>();

            string mensajeerrorBD = "";
            string mensaje = "";
            bool respuesta = false;
            //Cantidad de Salas y Apuestas Deportivas
            IntranetCPJLocalEntidad intranetCPJlocal = new IntranetCPJLocalEntidad();
            int cantidadSalas = 0;
            int cantidadApuestasDeportivas = 0;
            try {
                //Listando Footers
                var footerTupla = intranetFooterbl.IntranetFooterObtenerFootersJson();
                if (footerTupla.error.Respuesta)
                {
                    intranetFooter = footerTupla.listaFooters.ToList();
                }
                else {
                    mensajeerrorBD += "Error en Footers: " + error.Mensaje+"\n";
                }
                //listando menus
                var menuTupla = intranetMenubl.IntranetMenuListarJson();
                error = menuTupla.error;
                if (error.Respuesta)
                {
                    intranetMenu = menuTupla.intranetMenuLista;
                    
                }
                else {
                    mensajeerrorBD += "Error en Menus: " + error.Mensaje+"\n";
                }

                var mensajesCumpleanios = intranetSaludoCumpleaniosbl.IntranetSaludoCumpleanioActivosListarJson();
                error = mensajesCumpleanios.error;
                if (error.Respuesta)
                {
                    intraSaludos = mensajesCumpleanios.intranetSaludoCumpleanioLista;

                }
                else
                {
                    mensajeerrorBD += "Error en Menus: " + error.Mensaje + "\n";
                }

                //listando actividades
                var actividadesTupla = intranetActividadesbl.IntranetActividadesListarJson();
                error = actividadesTupla.error;
                if (error.Respuesta)
                {
                    intranetActividades_ = actividadesTupla.intranetActividadesLista;
                    if (intranetActividades_.Count > 0)
                    {
                        foreach (var m in intranetActividades_)
                        {
                            intranetActividades.Add(m);
                            listaNoticias.Add(Tuple.Create( m.act_fecha,  m.act_descripcion.ToUpper(), "ACTIVIDAD: "));
                        }
                    }
                }
                else
                {
                    mensajeerrorBD += "Error en Actividades: " + error.Mensaje + "\n";
                }

                //listando Cumpleaños

                //Lista de Empresas desde Postgres
                var listaEmpresasTupla = intranetempresabl.IntranetEmpresasListarJson();
                if (listaEmpresasTupla.error.Respuesta)
                {
                    listaEmpresas = listaEmpresasTupla.intranetEmpresasLista.Where(x => x.emp_estado == "A").ToList();
                    //creamos el string con la lista de empresas para el IN en sql
                    if (listaEmpresas.Count > 0)
                    {
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
                        //obteniendo mes anterior
                        var mes_anterior = DateTime.Now.Month - 1;
                        var listaPersonasSQLTupla = sqlbl.PersonaSQLObtenerListaCumpleaniosJson(stringEmpresas,mes_anterior);
                        if (listaPersonasSQLTupla.error.Respuesta)
                        {
                            listaPersonasSQL = listaPersonasSQLTupla.lista;

                            //response = true;
                            //errormensaje = "Listando Agenda";
                        }
                        else
                        {
                            mensajeerrorBD += listaPersonasSQLTupla.error.Mensaje;
                        }
                        //Listado de PErsonas SQL
                        var listaPersonasPostgresTupla = personabl.PersonaListarEmpleadosJson();
                        if (listaPersonasPostgresTupla.error.Respuesta)
                        {
                            listaPersonasPostgres = listaPersonasPostgresTupla.listaPersonas;
                        }
                        else
                        {
                            mensajeerrorBD += listaPersonasPostgresTupla.error.Mensaje;
                        }
                        if (listaPersonasPostgres.Count > 0 && listaPersonasSQL.Count > 0)
                        {
                            foreach (var m in listaPersonasSQL)
                            {
                                var contiene = listaPersonasPostgres.Where(x => x.per_numdoc.Equals(m.CO_TRAB)).SingleOrDefault();
                                if (contiene != null)
                                {
                                    string descripcionNoticia = contiene.per_nombre + " " + contiene.per_apellido_pat + " " + contiene.per_apellido_mat;
                                    listaNoticias.Add(Tuple.Create(contiene.per_fechanacimiento, descripcionNoticia.ToUpper(), "CUMPLEAÑOS: "));
                                    listaPersona.Add(new
                                    {
                                        per_id=contiene.per_id,
                                        per_numdoc = m.CO_TRAB,
                                        per_nombre = m.NO_TRAB,
                                        per_apellido_pat = m.NO_APEL_PATE,
                                        per_apellido_mat = m.NO_APEL_MATE,
                                        per_fechanacimiento = m.FE_NACI_TRAB,
                                        DE_NOMB = m.DE_NOMB,
                                        DE_AREA = m.DE_AREA,
                                        DE_PUES_TRAB = m.DE_PUES_TRAB,
                                        per_telefono = contiene.per_telefono,
                                        per_celular = contiene.per_celular,
                                        per_correoelectronico = contiene.per_correoelectronico,
                                        NU_TLF1 = m.NU_TLF1,
                                        NU_TLF2 = m.NU_TLF2,
                                        NO_DIRE_MAI1 = m.NO_DIRE_MAI1,

                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        mensajeerrorBD = "No hay Empresas";
                    }
                }
                else
                {
                    mensajeerrorBD += listaEmpresasTupla.error.Mensaje;
                }


                //var cumpleaniosTupla = personabl.PersonaObtenerCumpleaniosporDia();
                //error = cumpleaniosTupla.error;
                //if (error.Respuesta)
                //{
                //    listaPersona = cumpleaniosTupla.personaLista;
                //    if (listaPersona.Count > 0)
                //    {
                //        foreach (var m in listaPersona)
                //        {
                //            string descripcionNoticia = m.per_nombre + " " + m.per_apellido_pat + " " + m.per_apellido_mat;
                //            listaNoticias.Add(Tuple.Create(m.per_fechanacimiento, descripcionNoticia.ToUpper(), "CUMPLEAÑOS: "));
                //        }
                //    }
                //}
                //else
                //{
                //    mensajeerrorBD += "Error en Cumpleaños: " + error.Mensaje + "\n";
                //}
                //desordenando lista de noticias
                while (listaNoticias.Count > 0)
                {
                    int val = randNum.Next(0, listaNoticias.Count - 1);
                    listaNoticiasDesordenado.Add(listaNoticias[val]);
                    listaNoticias.RemoveAt(val);
                    if (listaNoticiasDesordenado.Count >= 10) {
                        break;
                    }
                }
                
                var seccionesMenu = intraSeccionBL.IntranetSeccionListarxMenuIDJson(menu_id);
                error = seccionesMenu.error;
                if (error.Respuesta)
                {
                    intranetSeccion = seccionesMenu.intranetSeccionListaxMenuID.Where(x=>x.sec_estado=="A").ToList();
                    if (intranetSeccion.Count > 0)
                    {
                        //secciones
                        foreach (var itemSt in intranetSeccion)
                        {
                            var ListaElementos = new List<dynamic>();
                            var elementos = intraElementobl.IntranetElementoListarxSeccionIDJson(itemSt.sec_id);
                            List<IntranetElementoEntidad> intranetElementos = new List<IntranetElementoEntidad>();
                            intranetElementos = elementos.intranetElementoListaxSeccionID.Where(x=>x.elem_estado=="A").ToList();
                            error = elementos.error;
                            if (intranetElementos.Count > 0)
                            {
                                //elementos
                                foreach (var itemElementos in intranetElementos)
                                {
                                    var ListaDetalleElemento = new List<dynamic>();
                                    var detalleelementos = intraDetalleElementobl.IntranetDetalleElementoListarxElementoIDJson(itemElementos.elem_id);
                                    List<IntranetDetalleElementoEntidad> intranetDetElementos = new List<IntranetDetalleElementoEntidad>();
                                    intranetDetElementos = detalleelementos.intranetDetalleElementoListaxElementoID.Where(x => x.detel_estado == "A").ToList();
                                    error = elementos.error;
                                    if (intranetDetElementos.Count > 0) {
                                        //detalle elementos
                                        foreach (var itemDetalleElemento in intranetDetElementos)
                                        {
                                            if (itemDetalleElemento.detel_nombre != "")
                                            {
                                                //itemDetalleElemento.detel_nombre  = rutaImagenes.ImagenIntranetActividades(PathActividadesIntranet + "\\", itemDetalleElemento.detel_nombre+"."+ itemDetalleElemento.detel_extension);
                                                itemDetalleElemento.detel_nombre = "IntranetFiles/"+itemDetalleElemento.detel_nombre+"."+itemDetalleElemento.detel_extension;
                                            }

                                            var seccion_elemento = new List<dynamic>();
                                            if (itemDetalleElemento.fk_seccion_elemento>0)
                                            {
                                                var seccion_ele = intranetSeccionImagenbl.IntranetSeccionElementoIdObtenerJson(itemDetalleElemento.fk_seccion_elemento);
                                                if (seccion_ele.intranetSeccionElemento.sele_id > 0)
                                                {
                                                    var Lista_elemento_modal = new List<dynamic>();
                                                    var elementosModal = intranetElementoModalbl.IntranetElementoModalListarxSeccionElementoIDJson(seccion_ele.intranetSeccionElemento.sele_id);
                                                    List<IntranetElementoModalEntidad> intranetElementoModal = new List<IntranetElementoModalEntidad>();
                                                    intranetElementoModal = elementosModal.intranetElementoModalListaxseccionelementoID.Where(x => x.emod_estado == "A").ToList();
                                                    error = elementos.error;
                                                    if (intranetElementoModal.Count > 0)
                                                    {
                                                        //elemento modal
                                                        foreach (var itemElementosModal in intranetElementoModal)
                                                        {
                                                            var ListaDetalleElementoModal = new List<dynamic>();
                                                            var detalleelementosModal = intranetDetalleElementoModalbl.IntranetDetalleElementoModalListarxElementoIDJson(itemElementosModal.emod_id);
                                                            List<IntranetDetalleElementoModalEntidad> intranetDetElementoModal = new List<IntranetDetalleElementoModalEntidad>();
                                                            intranetDetElementoModal = detalleelementosModal.intranetDetalleElementoModalListaxElementoID.Where(x => x.detelm_estado == "A").ToList();
                                                            error = elementos.error;
                                                            if (intranetDetElementoModal.Count > 0)
                                                            {
                                                                foreach (var itemDetalleElementosModal in intranetDetElementoModal)
                                                                {
                                                                    if (itemDetalleElementosModal.detelm_nombre != "")
                                                                    {
                                                                        //itemDetalleElementosModal.detelm_nombre = rutaImagenes.ImagenIntranetActividades(PathActividadesIntranet + "\\", itemDetalleElementosModal.detelm_nombre + "." + itemDetalleElementosModal.detelm_extension);
                                                                        itemDetalleElementosModal.detelm_nombre = "IntranetFiles/"+itemDetalleElementosModal.detelm_nombre+"."+itemDetalleElementosModal.detelm_extension;
                                                                    }
                                                                    ListaDetalleElementoModal.Add(new
                                                                    {
                                                                        itemDetalleElementosModal.detelm_id,
                                                                        itemDetalleElementosModal.detelm_descripcion,
                                                                        itemDetalleElementosModal.detelm_nombre,
                                                                        itemDetalleElementosModal.fk_elemento_modal,
                                                                        itemDetalleElementosModal.detelm_orden,
                                                                        itemDetalleElementosModal.detelm_posicion,
                                                                    });
                                                                }
                                                            }
                                                            Lista_elemento_modal.Add(new
                                                            {
                                                                itemElementosModal.emod_id,
                                                                itemElementosModal.emod_titulo,
                                                                itemElementosModal.emod_descripcion,
                                                                itemElementosModal.emod_contenido,
                                                                itemElementosModal.emod_orden,
                                                                itemElementosModal.fk_seccion_elemento,
                                                                itemElementosModal.fk_tipo_elemento,
                                                                detalle_elemento_modal = ListaDetalleElementoModal
                                                            });
                                                        }
                                                    }

                                                        seccion_elemento.Add(new{
                                                            seccion_ele.intranetSeccionElemento.sele_id,
                                                            seccion_ele.intranetSeccionElemento.sele_orden,
                                                            elemento_modal = Lista_elemento_modal
                                                        });
                                                }
                                            }

                                            ListaDetalleElemento.Add(new {
                                                itemDetalleElemento.detel_id,
                                                itemDetalleElemento.detel_descripcion,
                                                itemDetalleElemento.detel_nombre,
                                                itemDetalleElemento.fk_elemento,
                                                itemDetalleElemento.fk_seccion_elemento,
                                                itemDetalleElemento.detel_orden,
                                                itemDetalleElemento.detel_posicion,
                                                itemDetalleElemento.detel_blank,
                                                itemDetalleElemento.detel_url,
                                                seccion_elemento = seccion_elemento 
                                            });
                                        }

                                    }
                                    ListaElementos.Add(new {
                                        itemElementos.elem_id,
                                        itemElementos.elem_titulo,
                                        itemElementos.elem_descripcion,
                                        itemElementos.elem_contenido,
                                        itemElementos.elem_orden,
                                        itemElementos.fk_seccion,
                                        itemElementos.fk_tipo_elemento,
                                        detalleElemento = ListaDetalleElemento
                                    });
                                }
                            }

                            ListaSeccion.Add(new { itemSt.sec_id,itemSt.sec_orden, elementos = ListaElementos });
                        }
                    }

                }
                else
                {
                    mensajeerrorBD += "Error en Menus: " + error.Mensaje + "\n";
                }




                respuesta = true;
                mensaje = "Listando Data";
                //listando Cantidad de Salas
                var cantidadTupla = intranetCPJLocalbl.IntranetCPJLocalListarCantidadLocalesJson();
                error = cantidadTupla.error;
                if (error.Respuesta)
                {
                    cantidadSalas = cantidadTupla.intranetLocalCantidadSalas;
                    //cantidadApuestasDeportivas = cantidadTupla.intranetLocalCantidadApuestasDeportivas;
                }
                else {
                    mensajeerrorBD += "Error al Listar Cantidad de Salas y Apuestas Deportivas" + error.Mensaje+"\n";
                }
                //Listando Cantidad de Apuestas Deportivas
                try
                {
                    var client = new RestClient("https://api.apuestatotal.com/v2/locales");

                    var request = new RestRequest(Method.GET);
                    request.AddHeader("Authorization", "Bearer " + ApiKey);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");

                    IRestResponse response = client.Execute(request);
                    JObject json = JObject.Parse(response.Content);
                    //var result = json["result"];
                    dynamic jsonObj = JsonConvert.DeserializeObject(response.Content);
                    var terminalesApi = new List<IntranetPJTerminalEntidad>();
                    cantidadApuestasDeportivas = Convert.ToInt32(jsonObj.result.Count);
                    //foreach (var obj in jsonObj.result)
                    //{
                    //    var terminal = new IntranetPJTerminalEntidad
                    //    {
                    //        Descripcion = obj.nombre,
                    //        Latitud = obj.latitud,
                    //        Longitud = obj.longitud,
                    //        CodigoTerminal = obj.cc_id,
                    //        EsActivo = true
                    //    };
                    //    terminalesApi.Add(terminal);
                    //}

                }
                catch (Exception ex) {
                    mensajeerrorBD += ex.Message;
                }
            }
            catch (Exception ex) {
                mensaje = ex.Message;
                respuesta = false;
            }

            return Json(new
            {
                dataMenus = intranetMenu.ToList(),
                dataActividades = intranetActividades.ToList(),
                dataCumpleanios = listaPersona,
                dataSaludos = intraSaludos.ToList(),
                respuesta = respuesta,
                mensaje = mensaje,
                listaNoticias = listaNoticiasDesordenado,
                dataSecciones = ListaSeccion.ToList(),
                mensajeerrorBD = mensajeerrorBD,
                cantidadSalas = cantidadSalas,
                cantidadApuestasDeportivas = cantidadApuestasDeportivas,
                dataFooter = intranetFooter
            });
            //var serializer = new JavaScriptSerializer();
            //serializer.MaxJsonLength = Int32.MaxValue;

            //var resultData = new
            //{
            //    dataMenus = intranetMenu.ToList(),
            //    dataActividades = intranetActividades.ToList(),
            //    dataCumpleanios = listaPersona,
            //    dataSaludos = intraSaludos.ToList(),
            //    respuesta = respuesta,
            //    mensaje = mensaje,
            //    listaNoticias = listaNoticiasDesordenado,
            //    dataSecciones = ListaSeccion.ToList(),
            //    mensajeerrorBD = mensajeerrorBD,
            //    cantidadSalas = cantidadSalas,
            //    cantidadApuestasDeportivas = cantidadApuestasDeportivas,
            //    dataFooter= intranetFooter
            //};
            //var result = new ContentResult
            //{
            //    Content = serializer.Serialize(resultData),
            //    ContentType = "application/json"
            //};
            //return result;

        }

        public ActionResult Mapa(string tipo)
        {
            
            ViewBag.tipo = tipo;
            return View("~/Views/IntranetPJ/IntranetPJMapa.cshtml");
        }


        public ActionResult somopj()
        {
            return View();
        }

        public ActionResult politicaspj()
        {
            return View();
        }
        public ActionResult Descargas()
        {
            return View("~/Views/IntranetPJ/IntranetPJDescargas.cshtml");
        }
        public ActionResult Agenda()
        {
            return View("~/Views/IntranetPJ/IntranetPJAgenda.cshtml");
        }
        [HttpPost]
        public ActionResult ListarLocalesporTipoJson(string tipo, string nombre="") {
            string _tipo = tipo;
            string _nombre = nombre.ToLower();
            List<IntranetCPJLocalEntidad> intranetLocalLista = new List<IntranetCPJLocalEntidad>();
            claseError error = new claseError();
            bool response = false;
            string errormensaje = "";
            try {
                if (tipo == "ApuestasDeportivas")
                {
                    var client = new RestClient("https://api.apuestatotal.com/v2/locales");

                    var request = new RestRequest(Method.GET);
                    request.AddHeader("Authorization", "Bearer " + ApiKey);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("Connection", "keep-alive");

                    IRestResponse responseAPI = client.Execute(request);
                    JObject json = JObject.Parse(responseAPI.Content);
                    //var result = json["result"];
                    dynamic jsonObj = JsonConvert.DeserializeObject(responseAPI.Content);
                    string latitud = "";
                    string longitud = "";
                    CultureInfo culture = new CultureInfo("en-US");
                    //var terminalesApi = new List<IntranetPJTerminalEntidad>();
                    string patternLatitud = @"^(\+|-)?(?:90(?:(?:\.0{1,6})?)|(?:[0-9]|[1-8][0-9])(?:(?:\.[0-9]{1,6})?))$";
                    string patternLongitud = @"^(\+|-)?(?:180(?:(?:\.0{1,6})?)|(?:[0-9]|[1-9][0-9]|1[0-7][0-9])(?:(?:\.[0-9]{1,6})?))$";
                    decimal latitud_double = 0;
                    decimal longitud_double = 0;
                    foreach (var obj in jsonObj.result)
                    {
                        latitud = Convert.ToString(obj.latitud);
                        //latitud = latitud.Replace(",", "");
                        longitud = Convert.ToString(obj.longitud);
                        //longitud = longitud.Replace(",", "");

                        if (System.Text.RegularExpressions.Regex.IsMatch(latitud, patternLatitud))
                        {
                            latitud_double = Convert.ToDecimal((latitud == "" ? "0" : latitud), culture);
                        }
                        else {
                            latitud_double = 0;
                        }
                        if (System.Text.RegularExpressions.Regex.IsMatch(longitud, patternLongitud))
                        {
                            longitud_double = Convert.ToDecimal((longitud == "" ? "0" : longitud), culture);
                        }
                        else {
                            longitud_double = 0;
                        }
                        
                        var terminal = new IntranetCPJLocalEntidad
                        {
                            loc_nombre = ManejoNulos.ManageNullStr(obj.nombre),
                            loc_latitud =ManejoNulos.ManageNullDouble(latitud_double),
                            loc_longitud = ManejoNulos.ManageNullDouble(longitud_double),
                            loc_direccion = ManejoNulos.ManageNullStr(obj.direccion),
                        };
                        intranetLocalLista.Add(terminal);
                    }
                    response = true;
                    errormensaje = "Listando Apuestas Deportivas";
                }
                else {
                    var listaTupla = intranetCPJLocalbl.IntranetCPJLocalListarporNombreJson(_tipo, _nombre);
                    error = listaTupla.error;
                    if (error.Respuesta)
                    {
                        intranetLocalLista = listaTupla.intranetCPJLocalesLista;
                        response = true;
                        errormensaje = "Listando Locales";
                    }
                    else
                    {
                        response = false;
                        errormensaje = error.Mensaje;
                    }
                }
                
            }
            catch (Exception ex) {
                errormensaje = "No se Pudieron Listar los Locales"+ ex.Message;
                response = false;
            }
            return Json(new { data=intranetLocalLista.ToList(),respuesta=response,mensaje=errormensaje});
        }
        public ActionResult Login() {
            return View("~/Views/IntranetPJ/IntranetPJLogin.cshtml");
        }
        [HttpPost]
        public ActionResult IntranetLoginValdidarCredencialesJson(string usu_login,string usu_password)
        {
            bool respuesta = false;
            string errormensaje = "";
            string mensajeConsola = "";
            UsuarioEntidad usuario = new UsuarioEntidad();
            PersonaEntidad persona = new PersonaEntidad();
            claseError error = new claseError();
            string pendiente = "";
            try
            {
                var usuarioTupla = usuarioAccesobl.UsuarioIntranetValidarCredenciales(usu_login.ToLower());
                error = usuarioTupla.error;
                if (error.Respuesta)
                {
                    usuario = usuarioTupla.intranetUsuarioEncontrado;
                    if (usuario.usu_id > 0)
                    {
                        if (usuario.usu_estado == "A")
                        {
                            if (usuario.usu_tipo == "EMPLEADO")
                            {
                                var contrasenia = Seguridad.EncriptarSHA512(usu_password.Trim());
                                if (usuario.usu_contrasenia == Seguridad.EncriptarSHA512(usu_password.Trim()))
                                {
                                    //creacion de Token usuario y dni ira en el token separado de un punto
                                    Session["usuIntranet_full"] = usuariobl.UsuarioObtenerxID(usuario.usu_id);
                                    persona = personabl.PersonaIdObtenerJson(usuario.fk_persona);
                                    Session["perIntranet_full"] = persona;
                                    respuesta = true;
                                    errormensaje = "Bienvenido, " + usuario.usu_nombre;
                                }
                                else{
                                    errormensaje = "Contraseña no Coincide";
                                }
                            }
                            else
                            {
                                errormensaje = "Usuario no Pertenece";
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
                else {
                    errormensaje = "Ha ocurrido un problema";
                    mensajeConsola = error.Mensaje;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + "";
                mensajeConsola = error.Mensaje;
            }

            return Json(new { mensajeconsola=mensajeConsola,respuesta = respuesta, mensaje = errormensaje, estado = pendiente/*, usuario=usuario*/ });
        }
        [HttpPost]
        public ActionResult IntranetPJCerrarSesionLoginJson()
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {

                Session["usuIntranet_full"] = null;
                Session["perIntranet_full"] = null;
                respuestaConsulta = true;
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult IntranetObtenerAreadeTrabajoxUsuarioJson(string dni) {
            PersonaSqlEntidad personasql = new PersonaSqlEntidad();
            claseError error = new claseError();
            var errormensaje = "";
            bool respuestaConsulta = false;
            try {
                int mes_actual = DateTime.Now.Month;
                int anio = DateTime.Now.Year;
                var personaSQLTupla = sqlbl.PersonaSQLObtenerInformacionPuestoTrabajoJson(dni,mes_actual,anio);
                error = personaSQLTupla.error;
                if (error.Respuesta)
                {
                    if (personaSQLTupla.persona.CO_TRAB==null)
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
                        var personaSQLTupla2= sqlbl.PersonaSQLObtenerInformacionPuestoTrabajoJson(dni, mes_actual,anio);
                        if (personaSQLTupla2.error.Respuesta)
                        {
                            personasql = personaSQLTupla2.persona;
                        }
                    }
                    else
                    {
                        personasql = personaSQLTupla.persona;

                    }
                    errormensaje = "Obteniendo Datos";
                    respuestaConsulta = true;
                }
                else {
                    errormensaje = error.Mensaje;
                }
            }
            catch (Exception ex) {
                errormensaje = ex.Message;
            }
            return Json(new { data=personasql, respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult IntranetObtenerListadoArchivos() {
            bool response = false;
            string errormensaje = "";
            var listaArchivos = new List<dynamic>();
            string nombre_archivo = "";
            try {
                var direccion = Server.MapPath("/") + Request.ApplicationPath+"/archivos";
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
                        else {
                            nombre_archivo = info[0];
                        }
                        //tamaño de archivo
                        float length = (m.Length / 1024f) / 1024f;
                   
                        listaArchivos.Add(new
                        {
                            nombre = nombre_archivo,
                            extension = info.LastOrDefault(),
                            nombre_completo=m.Name,
                            tamanio= Math.Round(length, 4)
                        });
                    }
                    errormensaje = "Listando Data";
                    response = true;
                }
                else {
                    errormensaje = "No se encuentra el Directorio";
                }
            }
            catch (Exception ex) {
                errormensaje = ex.Message;
            }
            return Json(new { data=listaArchivos.ToList(),respuesta=response,mensaje=errormensaje});
        }
        
        public FileResult IntranetDescargarArchivo(string fileName="")
        {
            var direccion = Server.MapPath("/") + Request.ApplicationPath + "/archivos/";
            string fullName = Path.Combine(direccion, fileName);
            byte[] fileBytes = ConvertirArchivo(fullName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        byte[] ConvertirArchivo(string s) {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }
        [HttpPost]
        public ActionResult IntranetListarAgenda() {
            List<PersonaSqlEntidad> listaPersonasSQL = new List<PersonaSqlEntidad>();
            List<IntranetEmpresaEntidad> listaEmpresas = new List<IntranetEmpresaEntidad>();
            List<PersonaEntidad> listaPersonasPostgres = new List<PersonaEntidad>();
            var lista = new List<dynamic>();
            string errormensaje = "";
            bool response = false;
            try
            {
                //Lista de Empresas desde Postgres
                var listaEmpresasTupla = intranetempresabl.IntranetEmpresasListarJson();
                if (listaEmpresasTupla.error.Respuesta)
                {
                    listaEmpresas = listaEmpresasTupla.intranetEmpresasLista.Where(x=>x.emp_estado=="A").ToList();
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
                        var listaPersonasSQLTupla = sqlbl.PersonaSQLObtenerListaAgendaJson(stringEmpresas,mes_anterior);
                        if (listaPersonasSQLTupla.error.Respuesta)
                        {
                            listaPersonasSQL = listaPersonasSQLTupla.lista;

                            //response = true;
                            //errormensaje = "Listando Agenda";
                        }
                        else {
                            errormensaje += listaPersonasSQLTupla.error.Mensaje;
                        }
                        //Listado de PErsonas SQL
                        var listaPersonasPostgresTupla = personabl.PersonaListarEmpleadosJson();
                        if (listaPersonasPostgresTupla.error.Respuesta){
                            listaPersonasPostgres = listaPersonasPostgresTupla.listaPersonas;
                        }
                        else {
                            errormensaje += listaPersonasPostgresTupla.error.Mensaje;
                        }
                        if (listaPersonasPostgres.Count > 0 && listaPersonasSQL.Count > 0) {
                            foreach (var m in listaPersonasSQL) {
                                var contiene = listaPersonasPostgres.Where(x => x.per_numdoc.Equals(m.CO_TRAB)).FirstOrDefault();
                                if (contiene!=null) {
                                    lista.Add(new
                                    {
                                        CO_TRAB = m.CO_TRAB,
                                        NO_TRAB = m.NO_TRAB,
                                        NO_APEL_PATE = m.NO_APEL_PATE,
                                        NO_APEL_MATE = m.NO_APEL_MATE,
                                        DE_NOMB = m.DE_NOMB,
                                        DE_AREA = m.DE_AREA,
                                        DE_PUES_TRAB = m.DE_PUES_TRAB,
                                        NU_TLF1 = contiene.per_telefono,
                                        NU_TLF2 = contiene.per_celular,
                                        NO_DIRE_MAI1 = contiene.per_correoelectronico,
                                    });
                                }
                            }
                        }
                    }
                    else {
                        errormensaje = "No hay Empresas";
                    }
                    response = true;
                    errormensaje = "Listando Data";
                }
                else {
                    errormensaje = listaEmpresasTupla.error.Mensaje;
                }
            }
            catch (Exception ex) {
                errormensaje = ex.Message;
            }
            return Json(new{ data=lista.ToList(),mensaje=errormensaje,respuesta=response});
        }
        
    }
}