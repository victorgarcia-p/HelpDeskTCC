using HelpDesk.Models.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace HelpDesk.Models.Repository
{
    public class UsuariosRepository
    {
        public List<ViewModelUsuarios> BuscarUsuarios(string usuario, string perfil)
        {
            var query = "";
            if (perfil == "ADMINISTRADOR")
            {
                query = $"SELECT ID, NOME, EMAIL, LOGIN, SETOR, TOKEN, STATUS, PERFIL FROM USUARIOS WHERE LOGIN <> '{usuario}'";
            }
            else
            {
                query = $"SELECT ID, NOME, EMAIL, LOGIN, SETOR, TOKEN, STATUS, PERFIL FROM USUARIOS WHERE LOGIN <> '{usuario}' AND PERFIL <> 'ADMINISTRADOR'";
            }
            using (var bd = new ConnectBD())
            {

                return bd.Database.SqlQuery<ViewModelUsuarios>(query).ToList();
            }
        }

        public List<ViewModelUsuarios> BuscarTodosUsuarios()
        {
            using (var bd = new ConnectBD())
            {
                return bd.Database.SqlQuery<ViewModelUsuarios>($"SELECT ID, NOME, EMAIL, LOGIN, SETOR, TOKEN, STATUS, PERFIL FROM USUARIOS WHERE PERFIL IN ('TECNICO', 'ADMINISTRADOR')").ToList();
            }
        }

        public ViewModelUsuarios BuscarUsuario(string usuario)
        {
            var query = $"SELECT ID, NOME, EMAIL, LOGIN, SETOR, TOKEN, STATUS, PERFIL FROM USUARIOS WHERE LOGIN = '{usuario}'";
            using (var bd = new ConnectBD())
            {
                return bd.Database.SqlQuery<ViewModelUsuarios>(query).FirstOrDefault();
            }
        }

        public ViewModelUsuarios BuscarUsuarioEnvioEmail(string texto)
        {
            using (var bd = new ConnectBD())
            {
                return bd.Database.SqlQuery<ViewModelUsuarios>($"SELECT ID, NOME, EMAIL, LOGIN, SETOR, TOKEN, STATUS, PERFIL FROM USUARIOS WHERE LOGIN = '{texto}' OR EMAIL = '{texto}'").FirstOrDefault();
            }
        }

        public List<ViewModelUsuarios> CarregarUsuariosPorTipo(string tipo)
        {
            var query = "";
            if (tipo == "Todos")
            {
                query = "SELECT ID, LOGIN, NOME FROM USUARIOS ORDER BY LOGIN";
            }
            else if (tipo == "Tecnicos")
            {
                query = "SELECT USUARIOS.ID, USUARIOS.LOGIN, USUARIOS.NOME, CONFIGPLANTOES.COR FROM USUARIOS LEFT JOIN CONFIGPLANTOES ON CONFIGPLANTOES.USUARIO_ID = USUARIOS.ID WHERE PERFIL IN ('TECNICO', 'ADMINISTRADOR') ORDER BY LOGIN";
            }
            using (var bd = new ConnectBD())
            {
                return bd.Database.SqlQuery<ViewModelUsuarios>(query).ToList();
            }
        }
    }
}