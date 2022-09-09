using HelpDesk.Models.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HelpDesk.Models.Repository
{
    public class SetoresRepository
    {
        public List<SETORES> BuscarSetores()
        {
            using (var bd = new ConnectBD())
            {
                return bd.SETORES.ToList();
            }
        }

        public void CadastrarSetor(SETORES setor)
        {
            using (var bd = new ConnectBD())
            {
                bd.Entry(setor).State = EntityState.Added;
                bd.SaveChanges();
            }
        }

        public void AtualizarSetor(SETORES setor)
        {
            using (var bd = new ConnectBD())
            {
                bd.Entry(setor).State = EntityState.Modified;
                bd.SaveChanges();
            }
        }

        public void ExcluirSetor(SETORES setor)
        {
            using (var bd = new ConnectBD())
            {
                bd.Entry(setor).State = EntityState.Deleted;
                bd.SaveChanges();
            }
        }
    }
}