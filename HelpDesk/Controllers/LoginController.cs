using HelpDesk.Models.Entities;
using HelpDesk.Models.Services;
using System.Web.Mvc;
using System.Web.Security;


namespace HelpDesk.Controllers
{
    public class LoginController : Controller
    {
        private ServiceAccount _serviceAccount = new ServiceAccount();

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Chamados");
            }
            return View();
        }

        public JsonResult Login(string username, string password)
        {
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                var response = _serviceAccount.Usuario(username, password);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            return Json("Entre com Usuário e Senha", JsonRequestBehavior.AllowGet);
        }


        public string Post(USUARIOS usuario)
        {
            if (usuario != null)
            {
                if (_serviceAccount.Registrar(usuario))
                {
                    _serviceAccount.Usuario(usuario.LOGIN, usuario.SENHA);
                    return "Usuário cadastrado com sucesso!";
                }
            }
            return "Usuário já cadastrado";
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }

        public JsonResult EsqueceuSenha(string texto)
        {
            var response = _serviceAccount.EsqueceuSenha(texto);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}