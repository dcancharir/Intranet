using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Configuration;

namespace SistemaReclutamiento.Utilitarios
{
    public class Correo
    {
        private SmtpClient cliente;  
        private MailMessage email;
        private string _HOST = ConfigurationManager.AppSettings["host"];
        private string _PORT = ConfigurationManager.AppSettings["port"];
        private string _ENABLESSL = ConfigurationManager.AppSettings["enableSSL"];
        private string _USER = ConfigurationManager.AppSettings["user"];
        private string _PASWWORD = ConfigurationManager.AppSettings["password"];

        public Correo()
        {
            
            cliente = new SmtpClient(_HOST, Int32.Parse(_PORT))
            {
                EnableSsl = Boolean.Parse(_ENABLESSL),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_USER, _PASWWORD)
            };
        }     
        public void EnviarCorreo(string destinatario, string asunto, string mensaje, bool esHtlm = false)
        {
            email = new MailMessage(_USER, destinatario, asunto, mensaje);
            email.IsBodyHtml = esHtlm;
            cliente.Send(email);
        }
        public void EnviarCorreo(MailMessage message)
        {
            cliente.Send(message);
        }
      
    }
}