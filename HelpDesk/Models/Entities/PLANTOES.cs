using System;
using System.Collections.Generic;

namespace HelpDesk.Models.Entities
{
    public class PLANTOES
    {
        public int ID { get; set; }
        public string TITULO { get; set; }
        public DateTime DATAINICIO { get; set; }
        public DateTime DATAFIM { get; set; }
        public USUARIOS USUARIO { get; set; }
        public string CRIADOPOR { get; set; }
        public DateTime CRIADOEM { get; set; }
        public string ALTERADOPOR { get; set; }
        public DateTime ALTERADOEM { get; set; }
    }
}