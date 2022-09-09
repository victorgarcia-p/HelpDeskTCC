using HelpDesk.Models.Entities;
using HelpDesk.Models.Repository;
using HelpDesk.Models.ViewModel;
using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace HelpDesk.Models.Services
{
    public class ServiceAccount
    {
        private AccountRepository _accountRepository = new AccountRepository();
        private GlobalRepository _globalRepository = new GlobalRepository();
        private ServiceEmail _serviceEmail = new ServiceEmail();
        private ServiceUsuarios _serviceUsuarios = new ServiceUsuarios();
        private UsuariosRepository _usuarioRepository = new UsuariosRepository();

        public string Usuario(string username, string password)
        {
            try
            {
                ViewModelUsuarios usuario = _accountRepository.ValidarUsuario(username, CryptSenha(password));
                if (usuario != null)
                {
                    if (usuario.STATUS == true)
                    {
                        UltimoAcesso(usuario.LOGIN);
                        FormsAuthentication.SetAuthCookie(usuario.LOGIN + ";" + usuario.PERFIL + ";" + usuario.REDEFINIRSENHA, false);
                        return "Ok";
                    }
                    return "Usuário inativo";
                }
                return "Login ou senha incorretos";
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
                return "Erro ao realizar Login";
            }
        }

        public bool Registrar(USUARIOS usuario)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(usuario.SENHA))
                {
                    usuario.SENHA = GerarSenha();
                    usuario.REDEFINIRSENHA = true;
                }

                if (!_accountRepository.ValidarLogin(usuario.LOGIN))
                {
                    var registro = new USUARIOS
                    {
                        NOME = usuario.NOME,
                        EMAIL = usuario.EMAIL,
                        LOGIN = usuario.LOGIN,
                        SENHA = CryptSenha(usuario.SENHA),
                        SETOR = usuario.SETOR,
                        STATUS = usuario.STATUS,
                        PERFIL = usuario.PERFIL,
                        REDEFINIRSENHA = usuario.REDEFINIRSENHA,
                        ULTIMOACESSO = DateTime.Now,
                        CRIADOPOR = "internet",
                        CRIADOEM = DateTime.Now,
                        ALTERADOPOR = "internet",
                        ALTERADOEM = DateTime.Now
                    };
                    _accountRepository.AddUsuario(registro);

                    var random = new Random();
                    _accountRepository.AddCorUsuario(usuario.LOGIN, String.Format("#{0:X6}", random.Next(0x1000000)));

                    var mensagem = "Olá <br /><br /> Um novo usuário foi criado utilizando esse e-mail segue abaixo suas informações para Login no site." +
                    "<br /><br /> Uma nova senha será solicitada no seu próximo acesso." +
                    $"<b>Login: </b><h4>{usuario.LOGIN}<h4/>" +
                    $"<b>Senha: </b><h4>{usuario.SENHA}<h4/>";
                    var retorno = _serviceEmail.EnviarEmail("Informações de novo regitro", mensagem, "", usuario.EMAIL, "");

                    return true;
                }
                return false;
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
                return false;
            }
        }

        public void AtualizarUsuario(USUARIOS usuario, string usuarioConectado)
        {
            try
            {
                var status = usuario.STATUS == true ? "1" : "0";
                var query = $"UPDATE USUARIOS SET NOME = '{usuario.NOME}', EMAIL = '{usuario.EMAIL}', LOGIN = '{usuario.LOGIN}', SETOR = '{usuario.SETOR}', PERFIL = '{usuario.PERFIL}', STATUS = '{status}', ALTERADOPOR = '{usuarioConectado}', ALTERADOEM = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE ID = '{usuario.ID}'";
                _globalRepository.ExecutarComandoSQL(query);
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
            }
        }

        public void UltimoAcesso(string login)
        {
            try
            {
                var query = $"UPDATE USUARIOS SET ULTIMOACESSO = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE LOGIN = '{login}'";
                _globalRepository.ExecutarComandoSQL(query);
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
            }
        }

        static string CryptSenha(string password)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(password);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        static string GerarSenha()
        {
            string res = "";
            Random rnd = new Random();
            while (res.Length < 8) res += (new Func<Random, string>((r) =>
            {
                char c = (char)((r.Next(123) * DateTime.Now.Millisecond % 123));
                return (Char.IsLetterOrDigit(c)) ? c.ToString() : "";
            }))(rnd);
            return res;
        }

        public string EsqueceuSenha(string texto)
        {
            try
            {
                var usuario = _usuarioRepository.BuscarUsuarioEnvioEmail(texto);
                if (usuario != null)
                {
                    var novasenha = GerarSenha();
                    AlterarSenha(novasenha, usuario.LOGIN, "internet", false);
                    var mensagem = "Olá <br /><br /> Foi Solicitada a <b>recuperação de senha</b> para este usuário. Uma nova senha foi gerada automaticamente! " +
                        "<br /><br /> Uma nova senha será solicitada no seu próximo acesso." +
                        $"<b>Senha: </b><h4>{novasenha}<h4/>";
                    var retorno = _serviceEmail.EnviarEmail("Redefinição de senha", mensagem, "", usuario.EMAIL, "");
                    _serviceUsuarios.MudarStatusRedefinirSenha(true, usuario.LOGIN);
                    return retorno;
                }
                return "Usuário não encontrado";
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
                return $"Erro - {Ex.Message}";
            }

        }

        public void AlterarSenha(string senha, string login, string usuarioConectado, bool alteracao)
        {
            try
            {
                senha = CryptSenha(senha);
                var query = $"UPDATE USUARIOS SET SENHA = '{senha}', REDEFINIRSENHA = 0, ALTERADOEM = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', ALTERADOPOR = '{usuarioConectado}' WHERE LOGIN = '{login}'";
                _globalRepository.ExecutarComandoSQL(query);
                if (alteracao == true)
                {
                    ViewModelUsuarios usuario = _accountRepository.ValidarUsuario(login, senha);
                    FormsAuthentication.SetAuthCookie(usuario.LOGIN + ";" + usuario.PERFIL + ";" + usuario.REDEFINIRSENHA, false);
                }
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
            }
        }
    }
}