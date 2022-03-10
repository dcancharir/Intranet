using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SistemaReclutamiento.Entidades.SeguridadIntranet
{
    public class Metodo_clase
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string ReturnType { get; set; }
        public string Attributes { get; set; }
        public IList<CustomAttributeData> AttributesControlador { get; set; }
        public string AttributesControladorString { get; set; }
        public IList<CustomAttributeData> AttributesMetodo { get; set; }
        public string AttributesMetodostring { get; set; }
    }
}