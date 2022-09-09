using System;

namespace HelpDesk.Models.Entities
{
    public class HISTORICOCHAMADOS
    {
        public int ID { get; set; }
        public CHAMADOS CHAMADO { get; set; }
        public string COMENTARIOS { get; set; }
        public string ARQUIVOS { get; set; }
        public DateTime CRIADOEM { get; set; }
        public string CRIADOPOR { get; set; }
    }
}