using HelpDesk.Models.Repository;
using System;
using System.Reflection;

namespace HelpDesk.Models.Services
{
    public class ServiceGeral
    {
        private GlobalRepository _globalRepository = new GlobalRepository();

        public void AtivarInativar(int id, int status, string tabela)
        {
            try
            {
                var query = $"UPDATE {tabela} SET STATUS = '{status}' WHERE ID = '{id}'";
                _globalRepository.ExecutarComandoSQL(query);
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
            }
        }
    }
}