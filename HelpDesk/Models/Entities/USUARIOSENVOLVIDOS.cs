namespace HelpDesk.Models.Entities
{
    public class USUARIOSENVOLVIDOS
    {
        public int ID { get; set; }
        public USUARIOS USUARIO { get; set; }
        public CHAMADOS CHAMADO { get; set; }
        public string TIPO { get; set; }
    }
}