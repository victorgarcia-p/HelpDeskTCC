using System;

namespace HelpDesk.Models.Entities
{
    public class CHAMADOS
    {
        public int ID { get; set; }
        public string TITULO { get; set; }
        public string STATUS { get; set; }
        public virtual CATEGORIAS CATEGORIA{ get; set; }
        public string CRIADOPOR { get; set; }
        public DateTime CRIADOEM { get; set; }
        public string ALTERADOPOR { get; set; }
        public DateTime ALTERADOEM { get; set; }
    }
}