using HelpDesk.Models.Entities;
using HelpDesk.Models.Repository;
using HelpDesk.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HelpDesk.Models.Services
{
    public class ServiceChamados
    {
        private ChamadosRepository _chamadosRepository = new ChamadosRepository();
        private GlobalRepository _globalRepository = new GlobalRepository();
        private AccountRepository _accountRepository = new AccountRepository();
        private UsuariosRepository _usuariosRepository = new UsuariosRepository();
        private CategoriasRepository _categoriasRepository = new CategoriasRepository();
        private ServiceEmail _serviceEmail = new ServiceEmail();

        public List<ViewModelChamados> CarregarChamados(string usuario, string perfil, string filtro, string tipo, string pesquisa)
        {
            try
            {
                var retorno = new List<ViewModelChamados>();
                var chamados = _chamadosRepository.CarregarChamados(usuario, perfil, filtro, tipo, pesquisa);

                foreach (var chamado in chamados)
                {
                    string acompanhamento = "";
                    string tecnico = "";
                    var acompanhamentos = _chamadosRepository.CarregarEnvolvimentos(chamado.ID, "ACOMPANHAMENTO");
                    var tecnicos = _chamadosRepository.CarregarEnvolvimentos(chamado.ID, "TECNICO");
                    foreach (var item in acompanhamentos)
                    {
                        acompanhamento += item == acompanhamentos.Last() ? $"{item.LOGIN}" : $"{item.LOGIN};";
                    }
                    foreach (var item in tecnicos)
                    {
                        tecnico += item == tecnicos.Last() ? $"{item.LOGIN}" : $"{item.LOGIN};";
                    }

                    var novochamado = new ViewModelChamados
                    {
                        ID = chamado.ID,
                        TITULO = chamado.TITULO,
                        CRIADOEM = chamado.CRIADOEM,
                        CRIADOPOR = chamado.CRIADOPOR,
                        ACOMPANHAMENTO = acompanhamento,
                        TECNICO = tecnico,
                        STATUS = chamado.STATUS,
                        CATEGORIA = chamado.CATEGORIA,
                        ALTERADOEM = chamado.ALTERADOEM
                    };
                    retorno.Add(novochamado);
                }
                return retorno;
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
                return new List<ViewModelChamados>();
            }
        }

        public ViewModelChamados CarregarUmChamado(int id)
        {
            var chamado = _chamadosRepository.CarregarUmChamado(id);

            try
            {
                string acompanhamento = "";
                string tecnico = "";
                var acompanhamentos = _chamadosRepository.CarregarEnvolvimentos(chamado.ID, "ACOMPANHAMENTO");
                var tecnicos = _chamadosRepository.CarregarEnvolvimentos(chamado.ID, "TECNICO");
                foreach (var item in acompanhamentos)
                {
                    acompanhamento += item == acompanhamentos.Last() ? $"{item.LOGIN}" : $"{item.LOGIN};";
                }
                foreach (var item in tecnicos)
                {
                    tecnico += item == tecnicos.Last() ? $"{item.LOGIN}" : $"{item.LOGIN};";
                }

                var novochamado = new ViewModelChamados
                {
                    ID = chamado.ID,
                    TITULO = chamado.TITULO,
                    CRIADOEM = chamado.CRIADOEM,
                    CRIADOPOR = chamado.CRIADOPOR,
                    ACOMPANHAMENTO = acompanhamento,
                    TECNICO = tecnico,
                    STATUS = chamado.STATUS,
                    CATEGORIA = chamado.CATEGORIA,
                    ALTERADOEM = chamado.ALTERADOEM,
                };
                return novochamado;
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", Convert.ToString(chamado.ID));
                return new ViewModelChamados();
            }
        }

        public List<HISTORICOCHAMADOS> CarregarHistoricoChamado(int id)
        {
            try
            {
                return _chamadosRepository.CarregarHistoricoChamado(id);
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", Convert.ToString(id));
                return new List<HISTORICOCHAMADOS>();
            }
        }

        public int NovoChamado(string usuario, string titulo, string acompanhando, string mensagem, string arquivos, string stringCategoria)
        {
            try
            {
                var categoria = _categoriasRepository.BuscarCategoriaPorNome(stringCategoria);
                var registro = new CHAMADOS
                {
                    TITULO = titulo,
                    STATUS = "NOVO",
                    CRIADOPOR = usuario,
                    CRIADOEM = DateTime.Now,
                    ALTERADOPOR = usuario,
                    ALTERADOEM = DateTime.Now
                };
                var id = _chamadosRepository.NovoChamado(registro, categoria);

                if (!string.IsNullOrEmpty(acompanhando))
                {
                    var usuarios = acompanhando.Split(',');
                    AtribuirAcompanhamento(usuarios, id.ToString(), "ACOMPANHAMENTO", "NEW", usuario);
                }
                NovaMensagem(mensagem, id.ToString(), usuario, arquivos);
                return id;
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
                return 0;
            }
        }

        public void ExcluirChamado(int id)
        {
            try
            {
                var query = $"DELETE HISTORICOCHAMADOS WHERE CHAMADO_ID = {id} DELETE USUARIOSENVOLVIDOS WHERE CHAMADO_ID = {id} DELETE CHAMADOS WHERE ID = {id}";
                _globalRepository.ExecutarComandoSQL(query);
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", Convert.ToString(id));
            }
        }

        public void ExcluirArquivosChamado(string caminho)
        {
            try
            {
                if (Directory.Exists(caminho))
                {
                    Directory.Delete(caminho, true);
                }
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog($"{Ex.Message} - {caminho}", MethodBase.GetCurrentMethod().Name, "ERRO", "");
            }
        }

        public void RemoverUsuarioEnvolvido(string usuario, string chamado, string tipo, string usuarioConectado)
        {
            try
            {
                var id = _accountRepository.GetUsuarioID(usuario);

                var query = $"DELETE USUARIOSENVOLVIDOS WHERE CHAMADO_ID = {chamado} AND USUARIO_ID = {id} AND TIPO = '{tipo}' UPDATE CHAMADOS SET ALTERADOEM = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', ALTERADOPOR = '{usuarioConectado}'";
                _globalRepository.ExecutarComandoSQL(query);
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", chamado);
            }
        }

        public void NovaMensagem(string mensagem, string id, string usuario, string arquivos)
        {
            try
            {
                if (!string.IsNullOrEmpty(mensagem) || !string.IsNullOrEmpty(arquivos))
                {
                    var query = $"INSERT INTO HISTORICOCHAMADOS (CHAMADO_ID, COMENTARIOS, ARQUIVOS, CRIADOEM, CRIADOPOR) VALUES ('{id}','{mensagem}','{arquivos}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{usuario}') UPDATE CHAMADOS SET ALTERADOEM = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', ALTERADOPOR = '{usuario}'";
                    _globalRepository.ExecutarComandoSQL(query);
                }
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", id);
            }
        }

        public List<ViewModelUsuarios> CarregarUsuarios(string tipo)
        {
            try
            {
                var usuarios = _usuariosRepository.CarregarUsuariosPorTipo(tipo);
                return usuarios;
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
                return new List<ViewModelUsuarios>();
            }
        }

        public void AtribuirAcompanhamento(string[] usuarios, string chamado, string tipo, string alteracao, string usuarioConectado)
        {
            try
            {
                if (!string.IsNullOrEmpty(chamado) && !string.IsNullOrEmpty(tipo))
                {
                    if (tipo == "TECNICO")
                    {
                        AlterarStatusChamado(chamado, "EM ANDAMENTO");
                    }
                    foreach (var usuario in usuarios)
                    {
                        var query = alteracao == "ALT" ? $"INSERT INTO USUARIOSENVOLVIDOS (TIPO, CHAMADO_ID, USUARIO_ID) VALUES ('{tipo}', '{chamado}', '{usuario}') UPDATE CHAMADOS SET ALTERADOEM = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', ALTERADOPOR = '{usuarioConectado}'" : $"INSERT INTO USUARIOSENVOLVIDOS (TIPO, CHAMADO_ID, USUARIO_ID) VALUES ('{tipo}', '{chamado}', '{usuario}')";
                        _globalRepository.ExecutarComandoSQL(query);
                    }
                }
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", chamado);
            }
        }

        public void AlterarStatusChamado(string id, string status)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var query = $"UPDATE CHAMADOS SET STATUS = '{status}', ALTERADOEM = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE ID = '{id}'";
                    _globalRepository.ExecutarComandoSQL(query);
                }
            }catch(Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", id);
            }
        }

        public List<ViewModelChamadosDashboard> CarregarChamadosDashboardAlteracao(string dataIni, string dataFim)
        {
            try
            {
                return _chamadosRepository.CarregarChamadosDashboardAlteracao(dataIni, dataFim);
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
                return new List<ViewModelChamadosDashboard>();
            }
        }

        public List<ViewModelChamadosDashboard> CarregarChamadosDashboardCriacao(string dataIni, string dataFim)
        {
            try
            {
                return _chamadosRepository.CarregarChamadosDashboardCriacao(dataIni, dataFim);
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
                return new List<ViewModelChamadosDashboard>();
            }
        }

        public void AlterarCategoria(string chamado, string categoria, string usuario)
        {
            try
            {
                if (!string.IsNullOrEmpty(chamado))
                {
                    var query = $"UPDATE CHAMADOS SET CATEGORIA_ID = '{categoria}', ALTERADOEM = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', ALTERADOPOR = '{usuario}' WHERE ID = '{chamado}'";
                    _globalRepository.ExecutarComandoSQL(query);
                }
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", chamado);
            }
        }
    }
}
