using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.SeguridadIntranet
{
    public class Metodo_atributos
    {
        public string Controlador { get; set; }
        public string Metodo { get; set; }
        public bool seguridad { get; set; }
        public string modulo { get; set; }
        public string descripcion { get; set; }
    }
}