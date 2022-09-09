using System;

namespace HelpDesk.Models.Entities
{
    public class USUARIOS
    {
        public int ID { get; set; }
        public string NOME { get; set; }
        public string EMAIL { get; set; }
        public string LOGIN { get; set; }
        public string SENHA { get; set; }
        public string SETOR { get; set; }
        public string TOKEN { get; set; }
        public bool STATUS { get; set; }
        public string PERFIL { get; set; }
        public bool REDEFINIRSENHA { get; set; }
        public DateTime ULTIMOACESSO { get; set; }
        public string CRIADOPOR { get; set; }
        public DateTime CRIADOEM { get; set; }
        public string ALTERADOPOR { get; set; }
        public DateTime ALTERADOEM { get; set; }
    }
}