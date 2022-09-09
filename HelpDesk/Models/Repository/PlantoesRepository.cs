using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HelpDesk.Models.Repository
{
    public class PlantoesRepository
    {
        public List<ViewModelPlantoes> BuscarPlantoes()
        {
            using (var bd = new ConnectBD())
            {
                var query = "SELECT PLANTOES.ID, PLANTOES.TITULO, CONVERT(VARCHAR, PLANTOES.DATAINICIO, 23) AS 'DATAINICIO', CONVERT(VARCHAR, PLANTOES.DATAFIM, 23) AS 'DATAFIM', USUARIOS.LOGIN AS 'USUARIO', CONFIGPLANTOES.COR FROM PLANTOES INNER JOIN USUARIOS ON USUARIOS.ID = PLANTOES.USUARIO_ID INNER JOIN CONFIGPLANTOES ON CONFIGPLANTOES.USUARIO_ID = PLANTOES.USUARIO_ID";
                return bd.Database.SqlQuery<ViewModelPlantoes>(query).ToList();
            }
        }

        public void ExcluirPlantao(string id)   
        {
            using (var bd = new ConnectBD())
            {
                bd.Database.ExecuteSqlCommand($"DELETE PLANTOES WHERE ID = '{id}'");
            }
        }
    }
}