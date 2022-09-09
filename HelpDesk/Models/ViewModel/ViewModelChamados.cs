using System;

namespace HelpDesk.Models.ViewModel
{
    public class ViewModelChamados
    {
        public int ID { get; set; }
        public string TITULO { get; set; }
        public DateTime CRIADOEM { get; set; }
        public string CRIADOPOR { get; set; }
        public string ACOMPANHAMENTO { get; set; }
        public string TECNICO { get; set; }
        public string STATUS { get; set; }
        public string CATEGORIA { get; set; }
        public DateTime ALTERADOEM { get; set; }
    }
}