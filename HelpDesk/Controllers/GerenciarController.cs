using HelpDesk.Models.Entities;
using HelpDesk.Models.Services;
using HelpDesk.Models.ViewModel;
using System.Web.Mvc;

namespace HelpDesk.Controllers
{
    public class GerenciarController : Controller
    {
        private readonly ServiceUsuarios _serviceUsuarios = new ServiceUsuarios();
        private readonly ServiceAccount _serviceAccount = new ServiceAccount();
        private readonly ServiceSetores _serviceSetores = new ServiceSetores();
        private readonly ServiceCategorias _serviceCategorias = new ServiceCategorias();
        private readonly ServiceChamados _serviceChamados = new ServiceChamados();

        public ActionResult Usuarios()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Name.Split(';')[2] == "False")
                {
                    if (User.Identity.Name.Split(';')[1] != "USUARIO")
                    {
                        return View();
                    }
                    return RedirectToAction("Index", "Chamados");
                }
                return RedirectToAction("Perfil", "Gerenciar");
            }
            return RedirectToAction("Index", "Login");
        }

        public ActionResult Dashboard()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Name.Split(';')[2] == "False")
                {
                    if (User.Identity.Name.Split(';')[1] != "USUARIO")
                    {
                        return View();
                    }
                    return RedirectToAction("Index", "Chamados");
                }
                return RedirectToAction("Perfil", "Gerenciar");
            }
            return RedirectToAction("Index", "Login");
        }

        public ActionResult Perfil()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Index", "Login");
        }

        public ActionResult Setores()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Name.Split(';')[2] == "False")
                {
                    if (User.Identity.Name.Split(';')[1] != "USUARIO")
                    {
                        return View();
                    }
                    return RedirectToAction("Index", "Chamados");
                }
                return RedirectToAction("Perfil", "Gerenciar");
            }
            return RedirectToAction("Index", "Login");
        }

        public ActionResult Categorias()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Name.Split(';')[2] == "False")
                {
                    if (User.Identity.Name.Split(';')[1] != "USUARIO")
                    {
                        return View();
                    }
                    return RedirectToAction("Index", "Chamados");
                }
                return RedirectToAction("Perfil", "Gerenciar");
            }
            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public JsonResult BuscarUsuarios()
        {
            var usuario = User.Identity.Name.Split(';')[0];
            var perfil = User.Identity.Name.Split(';')[1];
            var retorno = _serviceUsuarios.BuscarUsuarios(usuario, perfil);
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public string Post(USUARIOS usuario)
        {
            if (usuario != null)
            {
                _serviceAccount.Registrar(usuario);
                return "Usuário cadastrado com sucesso!";
            }
            return "Usuário já cadastrado";
        }

        [HttpPost]
        public string CadastrarSetor(SETORES setor)
        {
            if (setor != null)
            {
                _serviceSetores.CadastrarSetor(setor);
                return $"Setor {setor.TITULO} cadastrado com sucesso!";
            }
            return "Erro ao cadastrar novo setor";
        }

        public string Update(USUARIOS usuario)
        {
            if (usuario != null)
            {
                _serviceAccount.AtualizarUsuario(usuario, User.Identity.Name.Split(';')[0]);
                return "Usuário Alterado com sucesso!";
            }
            return "Erro ao alterar usuário";
        }

        [HttpPost]
        public string AtualizarSetor(SETORES setor)
        {
            if (setor != null)
            {
                _serviceSetores.AtualizarSetor(setor);
                return "Alterado com sucesso!";
            }
            return "Erro ao alterar setor!";
        }

        [HttpGet]
        public JsonResult CarregarSetores()
        {
            var retorno = _serviceSetores.BuscarSetores();
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public string ExcluirSetor(SETORES setor)
        {
            if (setor != null)
            {
                _serviceSetores.ExcluirSetor(setor);
                return $"Setor {setor.TITULO} removido!";
            }
            return "Erro ao remover setor";
        }

        [HttpPost]
        public string AlterarSenha(string login, string senha)
        {
            if (!string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(senha))
            {
                _serviceAccount.AlterarSenha(senha, login, User.Identity.Name.Split(';')[0], true);
                return "Senha alterada com sucesso!";
            }
            return "Erro ao alterar senha!";
        }

        [HttpGet]
        public JsonResult BuscarPerfilUsuario(string login)
        {
            var retorno = new ViewModelUsuarios();
            if (!string.IsNullOrWhiteSpace(login))
            {
                retorno = _serviceUsuarios.BuscarUsuario(login);
            }
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult UsuariosDashboard()
        {
            var retorno = _serviceUsuarios.BuscarTodosUsuarios();
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ChamadosDashboardAlteracao(string dataIni, string dataFim)
        {
            var retorno = _serviceChamados.CarregarChamadosDashboardAlteracao($"{dataIni} 00:00:00", $"{dataFim} 23:59:59");
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ChamadosDashboardCriacao(string dataIni, string dataFim)
        {
            var retorno = _serviceChamados.CarregarChamadosDashboardCriacao($"{dataIni} 00:00:00", $"{dataFim} 23:59:59");
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string CadastrarCategoria(CATEGORIAS categoria)
        {
            if (categoria != null)
            {
                _serviceCategorias.CadastrarCategoria(categoria);
                return $"Categoria {categoria.TITULO} cadastrado com sucesso!";
            }
            return "Erro ao cadastrar nova categoria";
        }

        [HttpPost]
        public string AtualizarCategoria(CATEGORIAS categoria)
        {
            if (categoria != null)
            {
                _serviceCategorias.AtualizarCategoria(categoria);
                return "Alterado com sucesso!";
            }
            return "Erro ao alterar categoria!";
        }

        [HttpGet]
        public JsonResult CarregarCategorias()
        {
            var retorno = _serviceCategorias.BuscarCategorias();
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public string ExcluirCategoria(CATEGORIAS categoria)
        {
            if (categoria != null)
            {
                _serviceCategorias.ExcluirCategoria(categoria);
                return $"Categoria {categoria.TITULO} removida!";
            }
            return "Erro ao remover categoria";
        }
    }
}