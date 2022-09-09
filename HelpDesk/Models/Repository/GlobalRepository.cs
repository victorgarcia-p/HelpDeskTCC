using HelpDesk.Models.Entities;
using System;

namespace HelpDesk.Models.Repository
{
    public class GlobalRepository
    {
        public void ExecutarComandoSQL(string query)
        {
            using (var bd = new ConnectBD())
            {
                bd.Database.ExecuteSqlCommand(query);
            }
        }

        public void SalvarLog(string MENSAGEM, string METODO, string STATUS, string CHAMADO)
        {
            using (var bd = new ConnectBD())
            {
                var log = new LOGS
                {
                    MENSAGEM = MENSAGEM,
                    METODO = METODO,
                    STATUS = STATUS,
                    CHAMADO = CHAMADO,
                    CRIADOPOR = "API",
                    CRIADOEM = DateTime.Now,
                    ALTERADOPOR = "API",
                    ALTERADOEM = DateTime.Now
                };
                bd.LOGS.Add(log);
                bd.SaveChanges();
            }
        }
    }
}