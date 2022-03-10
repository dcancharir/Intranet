using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.IntranetPJ
{
    public class IntranetPJTerminalEntidad
    {
       
            public int TerminalId { get; set; }
            public int CodigoTerminal { get; set; }
            public string Descripcion { get; set; }
            public string TerminalCompleto { get; set; }
            public string Mac { get; set; }
            public string Latitud { get; set; }
            public string Longitud { get; set; }
            public int RangoAcceso { get; set; }
        public string Direccion { get; set; }
            public Boolean EsActivo { get; set; }
    }
}