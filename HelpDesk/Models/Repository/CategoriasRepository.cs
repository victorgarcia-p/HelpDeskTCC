using HelpDesk.Models.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HelpDesk.Models.Repository
{
    public class CategoriasRepository
    {
        public List<CATEGORIAS> BuscarCategorias()
        {
            using (var bd = new ConnectBD())
            {
                return bd.CATEGORIAS.ToList();
            }
        }

        public void CadastrarCategoria(CATEGORIAS categoria)
        {
            using (var bd = new ConnectBD())
            {
                bd.Entry(categoria).State = EntityState.Added;
                bd.SaveChanges();
            }
        }

        public void AtualizarCategoria(CATEGORIAS categoria)
        {
            using (var bd = new ConnectBD())
            {
                bd.Entry(categoria).State = EntityState.Modified;
                bd.SaveChanges();
            }
        }

        public void ExcluirCategoria(CATEGORIAS categoria)
        {
            using (var bd = new ConnectBD())
            {
                bd.Entry(categoria).State = EntityState.Deleted;
                bd.SaveChanges();
            }
        }

        public CATEGORIAS BuscarCategoriaPorNome(string categoria)
        {
            using (var bd = new ConnectBD())
            {
                return bd.CATEGORIAS.FirstOrDefault(x => x.TITULO == categoria);
            }
        }
    }
}