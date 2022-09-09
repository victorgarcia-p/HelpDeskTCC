using HelpDesk.Models.Repository;
using HelpDesk.Models.Services;
using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace HelpDesk.Controllers
{
    public class ChamadosController : Controller
    {
        private ServiceChamados _serviceChamados = new ServiceChamados();
        private GlobalRepository _globalRepository = new GlobalRepository();
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Name.Split(';')[2] == "False")
                {
                    return View();
                }
                return RedirectToAction("Perfil", "Gerenciar");
            }
            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        [Route("/chamado/{id}")]
        public ActionResult Chamado(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Name.Split(';')[2] == "False")
                {
                    var chamados = _serviceChamados.CarregarChamados(User.Identity.Name.Split(';')[0], User.Identity.Name.Split(';')[1], "CHAMADOS.ID", "=", id.ToString());
                    foreach (var chamado in chamados)
                    {
                        if (chamado.ID == id)
                        {
                            ViewBag.Message = id;
                            return View();
                        }
                    }
                    return RedirectToAction("Index", "Chamados");
                }
                return RedirectToAction("Perfil", "Gerenciar");
            }
            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public JsonResult CarregarChamados(string filtro, string tipo, string pesquisa)
        {
            var usuario = User.Identity.Name.Split(';')[0];
            var perfil = User.Identity.Name.Split(';')[1];
            var retorno = _serviceChamados.CarregarChamados(usuario, perfil, filtro, tipo, pesquisa);
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CarregarUmChamado(int id)
        {
            var retorno = _serviceChamados.CarregarUmChamado(id);
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CarregarHistoricoChamado(int id)
        {
            var retorno = _serviceChamados.CarregarHistoricoChamado(id);
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void NovoChamado(string titulo, string acompanhando, string mensagem, string categoria)
        {
            var id = 0;
            var arquivo = "";
            try
            {
                if (!string.IsNullOrEmpty(titulo) && !string.IsNullOrEmpty(mensagem))
                {
                    var files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        try
                        {
                            HttpPostedFileBase file = files[i];

                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                arquivo += $"{testfiles[testfiles.Length - 1]};";
                            }
                            else
                            {
                                arquivo += $"{file.FileName};";
                            }
                        }
                        catch (Exception Ex)
                        {
                            _globalRepository.SalvarLog(Ex.Message, "UploadArquivo", "ERRO", "");
                        }
                    }

                    var usuario = User.Identity.Name.Split(';')[0];
                    id = _serviceChamados.NovoChamado(usuario, titulo, acompanhando, mensagem, arquivo, categoria);

                    if (Request.Files.Count > 0)
                    {
                        InserirArquivo(Request.Files, id.ToString());
                    }
                }
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", id.ToString());
            }
        }

        [HttpDelete]
        public void ExcluirChamado(int id)
        {
            if (id > 0)
            {
                _serviceChamados.ExcluirChamado(id);
                _serviceChamados.ExcluirArquivosChamado(Server.MapPath($"~/content/Arquivos/{id}/"));
            }
        }

        [HttpDelete]
        public void RemoverUsuarioEnvolvido(string usuario, string chamado, string tipo)
        {
            _serviceChamados.RemoverUsuarioEnvolvido(usuario, chamado, tipo, User.Identity.Name.Split(';')[0]);
        }

        [HttpPost]
        public void NovaMensagem(string mensagem, string id, string encerrar)
        {
            var arquivo = "";
            var files = Request.Files;
            try
            {
                if (!string.IsNullOrEmpty(mensagem) || files.Count > 0)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        try
                        {
                            HttpPostedFileBase file = files[i];

                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                if (i == (files.Count - 1))
                                {
                                    arquivo += testfiles[testfiles.Length - 1];
                                }
                                else
                                {
                                    arquivo += $"{testfiles[testfiles.Length - 1]};";
                                }


                            }
                            else
                            {
                                if (i == (files.Count - 1))
                                {
                                    arquivo += file.FileName;
                                }
                                else
                                {
                                    arquivo += $"{file.FileName};";
                                }
                            }
                        }
                        catch (Exception Ex)
                        {
                            _globalRepository.SalvarLog(Ex.Message, "UploadArquivo", "ERRO", id);
                        }
                    }

                    var usuario = User.Identity.Name.Split(';')[0];
                    _serviceChamados.NovaMensagem(mensagem, id, usuario, arquivo);

                    if (Request.Files.Count > 0)
                    {
                        InserirArquivo(Request.Files, id);
                    }
                }

                if (encerrar == "sim")
                {
                    _serviceChamados.AlterarStatusChamado(id, "ENCERRADO");
                }
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", id);
            }
        }

        public void InserirArquivo(HttpFileCollectionBase files, string id)
        {

            for (int i = 0; i < files.Count; i++)
            {
                string fname = "";
                HttpPostedFileBase file = files[i];

                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fname = testfiles[testfiles.Length - 1];
                }
                else
                {
                    fname = file.FileName;
                }

                if (!Directory.Exists(Server.MapPath($"~/content/Arquivos/{id}/")))
                {
                    Directory.CreateDirectory(Server.MapPath($"~/content/Arquivos/{id}/"));
                }

                var caminho = Path.Combine(Server.MapPath($"~/content/Arquivos/{id}/"), fname);
                file.SaveAs(caminho);
            }
        }

        [HttpGet]
        public JsonResult CarregarUsuarios(string tipo)
        {
            var retorno = _serviceChamados.CarregarUsuarios(tipo);
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void AtribuirAcompanhamento(string[] usuarios, string chamado, string tipo)
        {
            _serviceChamados.AtribuirAcompanhamento(usuarios, chamado, tipo, "ALT", User.Identity.Name.Split(';')[0]);
        }

        [HttpPost]
        public void AlterarCategoria(string chamado, string categoria, string usuario)
        {
            _serviceChamados.AlterarCategoria(chamado, categoria, usuario);
        }
    }
}