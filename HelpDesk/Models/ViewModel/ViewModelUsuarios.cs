namespace HelpDesk.Models.ViewModel
{
    public class ViewModelUsuarios
    {
        public int ID { get; set; }
        public string NOME { get; set; }
        public string EMAIL { get; set; }
        public string LOGIN { get; set; }
        public string SETOR { get; set; }
        public string TOKEN { get; set; }
        public bool STATUS { get; set; }
        public string PERFIL { get; set; }
        public bool REDEFINIRSENHA { get; set; }
        public string COR { get; set; }
    }
}