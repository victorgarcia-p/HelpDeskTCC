using HelpDesk.Models.Entities;
using HelpDesk.Models.Repository;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace HelpDesk.Models.Services
{
    public class ServiceSetores
    {
        private SetoresRepository _setoresRepository = new SetoresRepository();
        private GlobalRepository _globalRepository = new GlobalRepository();

        public List<SETORES> BuscarSetores()
        {
            try
            {
                return _setoresRepository.BuscarSetores();
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
                return new List<SETORES>();
            }
        }

        public void CadastrarSetor(SETORES setor)
        {
            try
            {
                _setoresRepository.CadastrarSetor(setor);
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
            }
        }

        public void AtualizarSetor(SETORES setor)
        {
            try
            {
                _setoresRepository.AtualizarSetor(setor);
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
            }
        }

        public void ExcluirSetor(SETORES setor)
        {
            try
            {
                _setoresRepository.ExcluirSetor(setor);
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
            }
        }
    }
}