using HelpDesk.Models.Entities;
using HelpDesk.Models.Repository;
using HelpDesk.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace HelpDesk.Models.Services
{
    public class ServicePlantoes
    {
        private PlantoesRepository _plantoesRepository = new PlantoesRepository();
        private GlobalRepository _globalRepository = new GlobalRepository();

        public List<ViewModelPlantoes> BuscarPlantoe()
        {
            try
            {
                return _plantoesRepository.BuscarPlantoes();
            }catch(Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
                return new List<ViewModelPlantoes>();
            }
            
        }

        public void SalvarPlantao(string dataIni, string dataFim, ViewModelUsuarios usuario, string usuarioLogado)
        {
            try
            {
                var query = $"INSERT INTO PLANTOES (TITULO, DATAINICIO, DATAFIM, CRIADOPOR, CRIADOEM, ALTERADOPOR, ALTERADOEM, USUARIO_ID) VALUES ('{usuario.NOME}','{dataIni}','{dataFim}','{usuarioLogado}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{usuarioLogado}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{usuario.ID}')";
                _globalRepository.ExecutarComandoSQL(query);
            }catch(Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
            }
        }

        public void AlterarPlantao(string dataIni, string dataFim, ViewModelUsuarios usuario, string idPlantao,string usuarioLogado)
        {
            try
            {
                var query = $"UPDATE PLANTOES SET TITULO = '{usuario.NOME}', DATAINICIO = '{dataIni}', DATAFIM = '{dataFim}', ALTERADOPOR = '{usuarioLogado}', ALTERADOEM = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', USUARIO_ID = '{usuario.ID}' WHERE ID = '{idPlantao}'";
                _globalRepository.ExecutarComandoSQL(query);
            }catch(Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
            }
        }

        public void ExcluirPlantao(string idPlantao)
        {
            try
            {
                _plantoesRepository.ExcluirPlantao(idPlantao);
            }catch(Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
            }
        }
    }
}