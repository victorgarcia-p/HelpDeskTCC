using System;

namespace HelpDesk.Models.ViewModel
{
    public class ViewModelChamadosDashboard
    {
        public int ID { get; set; }
        public string TITULO { get; set; }
        public string STATUS { get; set; }
        public int CATEGORIA_ID { get; set; }
        public string CRIADOPOR { get; set; }
        public DateTime CRIADOEM { get; set; }
        public string ALTERADOPOR { get; set; }
        public DateTime ALTERADOEM { get; set; }
        public string TIPO { get; set; }
        public string LOGIN { get; set; }
        public string CATEGORIA { get; set; }
    }
}