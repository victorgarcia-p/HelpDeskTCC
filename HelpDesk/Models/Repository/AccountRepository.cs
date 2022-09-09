using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModel;
using System.Linq;

namespace HelpDesk.Models.Repository
{
    public class AccountRepository
    {
        public ViewModelUsuarios ValidarUsuario(string username, string password)
        {
            using (var bd = new ConnectBD())
            {
                return bd.Database.SqlQuery<ViewModelUsuarios>($"SELECT ID, NOME, EMAIL, LOGIN, SETOR, TOKEN, PERFIL, REDEFINIRSENHA, STATUS FROM USUARIOS WHERE LOGIN = '{username}' AND SENHA = '{password}'").FirstOrDefault();
            }
        }

        public bool ValidarLogin(string username)
        {
            using (var bd = new ConnectBD())
            {
                return bd.Database.SqlQuery<string>($"SELECT LOGIN FROM USUARIOS WHERE LOGIN = '{username}'").Any();
            }
        }

        public void AddUsuario(USUARIOS usuario)
        {
            using (var bd = new ConnectBD())
            {
                bd.USUARIOS.Add(usuario);
                bd.SaveChanges();
            }

        }

        public void AddCorUsuario(string login, string cor)
        {
            using (var bd = new ConnectBD())
            {
                var usuario = bd.USUARIOS.FirstOrDefault(x => x.LOGIN == login);

                var config = new CONFIGPLANTOES()
                {
                    USUARIO = usuario,
                    COR = cor
                };

                bd.CONFIGPLANTOES.Add(config);
                bd.SaveChanges();
            }

        }

        public int GetUsuarioID(string login)
        {
            using (var bd = new ConnectBD())
            {
                return bd.Database.SqlQuery<int>($"SELECT ID FROM USUARIOS WHERE LOGIN = '{login}'").FirstOrDefault();
            }
        }
    }
}