using System;
using System.Collections.Generic;

namespace HelpDesk.Models.Entities
{
    public class LOGS
    {
        public int ID { get; set; }
        public string MENSAGEM { get; set; }
        public string METODO { get; set; }
        public string CHAMADO { get; set; }
        public string STATUS { get; set; }
        public string CRIADOPOR { get; set; }
        public DateTime CRIADOEM { get; set; }
        public string ALTERADOPOR { get; set; }
        public DateTime ALTERADOEM { get; set; }
    }
}