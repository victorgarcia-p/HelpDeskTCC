using HelpDesk.Models.Services;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace HelpDesk.Controllers
{
    public class PlantaoController : Controller
    {
        private ServiceChamados _serviceChamados = new ServiceChamados();
        private ServiceUsuarios _serviceUsuarios = new ServiceUsuarios();
        private ServicePlantoes _servicePlantoes = new ServicePlantoes();

        public ActionResult Index()
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
        public JsonResult BuscarTecnicos(string tipo)
        {
            var retorno = _serviceChamados.CarregarUsuarios(tipo);
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult BuscarPlantoes()
        {
            var retorno = _servicePlantoes.BuscarPlantoe();
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SalvarPlantao(string dataIni, string dataFim, string usuario, string usuarioLogado)
        {
            try
            {
                var tecnico = _serviceUsuarios.BuscarUsuario(usuario);
                _servicePlantoes.SalvarPlantao(dataIni, dataFim, tecnico, usuarioLogado);
                return Json("Ok", JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json($"Erro - {Ex}", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPut]
        public JsonResult AlterarPlantao(string dataIni, string dataFim, string usuario, string idFolga, string usuarioLogado)
        {
            try
            {
                var tecnico = _serviceUsuarios.BuscarUsuario(usuario);
                _servicePlantoes.AlterarPlantao(dataIni, dataFim, tecnico, idFolga, usuarioLogado);
                return Json("Ok", JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json($"Erro - {Ex}", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpDelete]
        public JsonResult ExcluirPlantao(string idFolga)
        {
            try
            {
                _servicePlantoes.ExcluirPlantao(idFolga);
                return Json("Ok", JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json($"Erro - {Ex}", JsonRequestBehavior.AllowGet);
            }

        }
    }
}