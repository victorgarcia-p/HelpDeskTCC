using HelpDesk.Models.Repository;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Reflection;
using System.Web.Configuration;

namespace HelpDesk.Models.Services
{
    public class ServiceEmail
    {
        private string emailHost = WebConfigurationManager.AppSettings["emailHost"];
        private int emailPort = Convert.ToInt32(WebConfigurationManager.AppSettings["emailPort"]);
        private string emailUser = WebConfigurationManager.AppSettings["emailUser"];
        private string emailPassword = WebConfigurationManager.AppSettings["emailPassword"];
        private GlobalRepository _globalRepository = new GlobalRepository();

        public string EnviarEmail(string assunto, string mensagem, string anexo, string destinatario, string copia)
        {
            try
            {
                var email = new MimeMessage();

                InternetAddressList TO_addressList = new InternetAddressList();
                TO_addressList.Add(MailboxAddress.Parse(destinatario));

                //se tiver algum destinatário em cópia
                if (!String.IsNullOrEmpty(copia)) { email.Bcc.Add(MailboxAddress.Parse(copia)); }
                email.To.AddRange(TO_addressList);

                email.From.Add(MailboxAddress.Parse(emailUser));
                email.Subject = assunto;
                var conteudoEmail = new BodyBuilder();
                conteudoEmail.HtmlBody = mensagem; //+ "<br><br>Email enviado automaticamente pela API <a href='https://bsite.net/helpdeskTCC'>Help Desk</a>";
                //se tiver arquivo anexo
                if (!String.IsNullOrEmpty(anexo)) { conteudoEmail.Attachments.Add(System.Web.HttpContext.Current.Server.MapPath("~/Content/Arquivos/") + anexo); }
                email.Body = conteudoEmail.ToMessageBody();
                SendEmail(email);
                if (assunto == "Redefinição de senha")
                {
                    return "Nova senha enviada para o E-Mail cadastrado";
                }
                else
                {
                    return "Usuário cadastrado com sucesso!";
                }
                
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
                return $"Erro ao enviar email: {Ex.Message} ";
            }
        }

        public void SendEmail(MimeMessage email)
        {
            try
            {
                var client = new SmtpClient();
                client.Connect(emailHost, emailPort, SecureSocketOptions.StartTls);
                client.Authenticate(emailUser, emailPassword);
                client.Send(email);
                client.Disconnect(true);
            }
            catch(Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
            }
        }
    }
}