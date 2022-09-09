using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace HelpDesk.Models.Repository
{
    public class ChamadosRepository
    {
        public List<ViewModelChamados> CarregarChamados(string usuario, string perfil, string filtro, string tipo, string pesquisa)
        {
            using (var bd = new ConnectBD())
            {

                pesquisa = tipo == "LIKE" ? $"%{pesquisa}%" : pesquisa;

                if (perfil == "ADMINISTRADOR" || perfil == "TECNICO")
                {
                    return bd.Database.SqlQuery<ViewModelChamados>($"SELECT CHAMADOS.ID, CHAMADOS.TITULO, CHAMADOS.CRIADOEM, CHAMADOS.CRIADOPOR, CHAMADOS.STATUS, CATEGORIAS.TITULO AS 'CATEGORIA', CHAMADOS.ALTERADOEM FROM CHAMADOS INNER JOIN CATEGORIAS ON CATEGORIAS.ID = CHAMADOS.CATEGORIA_ID LEFT JOIN USUARIOSENVOLVIDOS ON USUARIOSENVOLVIDOS.CHAMADO_ID = CHAMADOS.ID LEFT JOIN USUARIOS (NOLOCK) ON USUARIOS.ID = USUARIOSENVOLVIDOS.USUARIO_ID WHERE {filtro} {tipo} '{pesquisa}' GROUP BY CHAMADOS.ID, CHAMADOS.TITULO, CHAMADOS.CRIADOEM, CHAMADOS.CRIADOPOR, CHAMADOS.STATUS, CHAMADOS.ALTERADOEM, CATEGORIAS.TITULO").ToList();
                }
                else
                {
                    return bd.Database.SqlQuery<ViewModelChamados>($"SELECT CHAMADOS.ID, CHAMADOS.TITULO, CHAMADOS.CRIADOEM, CHAMADOS.CRIADOPOR, CHAMADOS.STATUS, CATEGORIAS.TITULO AS 'CATEGORIA', CHAMADOS.ALTERADOEM FROM CHAMADOS INNER JOIN CATEGORIAS ON CATEGORIAS.ID = CHAMADOS.CATEGORIA_ID LEFT JOIN USUARIOSENVOLVIDOS ON USUARIOSENVOLVIDOS.CHAMADO_ID = CHAMADOS.ID LEFT JOIN USUARIOS (NOLOCK) ON USUARIOS.ID = USUARIOSENVOLVIDOS.USUARIO_ID WHERE (CHAMADOS.CRIADOPOR = '{usuario}' OR USUARIOS.LOGIN = '{usuario}') AND {filtro} {tipo} '{pesquisa}' GROUP BY CHAMADOS.ID, CHAMADOS.TITULO, CHAMADOS.CRIADOEM, CHAMADOS.CRIADOPOR, CHAMADOS.STATUS, CHAMADOS.ALTERADOEM, CATEGORIAS.TITULO").ToList();
                }
            }
        }

        public ViewModelChamados CarregarUmChamado(int id)
        {
            using (var bd = new ConnectBD())
            {
                return bd.Database.SqlQuery<ViewModelChamados>($"SELECT CHAMADOS.ID, CHAMADOS.TITULO, CHAMADOS.CRIADOEM, CHAMADOS.CRIADOPOR, CHAMADOS.STATUS, CATEGORIAS.TITULO AS 'CATEGORIA', CHAMADOS.ALTERADOEM FROM CHAMADOS INNER JOIN CATEGORIAS ON CATEGORIAS.ID = CHAMADOS.CATEGORIA_ID WHERE CHAMADOS.ID = {id}").FirstOrDefault();
            }
        }

        public List<HISTORICOCHAMADOS> CarregarHistoricoChamado(int ID)
        {
            using (var bd = new ConnectBD())
            {
                return bd.Database.SqlQuery<HISTORICOCHAMADOS>($"SELECT * FROM HISTORICOCHAMADOS WHERE CHAMADO_ID = '{ID}' ORDER BY CRIADOEM DESC").ToList();
            }
        }

        public int NovoChamado(CHAMADOS chamado, CATEGORIAS categoria)
        {
            using (var bd = new ConnectBD())
            {
                bd.CHAMADOS.Add(chamado);
                bd.SaveChanges();
                var id = chamado.ID;
                bd.Database.ExecuteSqlCommand($"UPDATE CHAMADOS SET CATEGORIA_ID = '{categoria.ID}' WHERE ID = '{id}'");
                return id;
            }
        }

        public List<ViewModelUsuariosEnvolvidos> CarregarEnvolvimentos(int chamado, string tipo)
        {
            using (var bd = new ConnectBD())
            {
                if (chamado != 0)
                {
                    return bd.Database.SqlQuery<ViewModelUsuariosEnvolvidos>($"SELECT USUARIOS.LOGIN, USUARIOSENVOLVIDOS.TIPO, USUARIOS.EMAIL FROM USUARIOSENVOLVIDOS(NOLOCK) LEFT JOIN USUARIOS(NOLOCK) ON USUARIOS.ID = USUARIOSENVOLVIDOS.USUARIO_ID WHERE USUARIOSENVOLVIDOS.CHAMADO_ID = {chamado} and USUARIOSENVOLVIDOS.TIPO = '{tipo}'").ToList();
                }
                return new List<ViewModelUsuariosEnvolvidos>();
            }
        }

        public List<ViewModelChamadosDashboard> CarregarChamadosDashboardAlteracao(string dataIni, string dataFim)
        {
            using (var bd = new ConnectBD())
            {
                return bd.Database.SqlQuery<ViewModelChamadosDashboard>($"SELECT CHAMADOS.*, USUARIOSENVOLVIDOS.TIPO, USUARIOS.LOGIN, CATEGORIAS.TITULO AS 'CATEGORIA' FROM CHAMADOS (NOLOCK) INNER JOIN CATEGORIAS (NOLOCK) ON CATEGORIAS.ID = CHAMADOS.CATEGORIA_ID LEFT JOIN USUARIOSENVOLVIDOS (NOLOCK) ON USUARIOSENVOLVIDOS.CHAMADO_ID = CHAMADOS.ID LEFT JOIN USUARIOS (NOLOCK) ON USUARIOS.ID = USUARIOSENVOLVIDOS.USUARIO_ID LEFT JOIN USUARIOS USUARIOS2 (NOLOCK) ON USUARIOS2.LOGIN = CHAMADOS.CRIADOPOR WHERE CHAMADOS.ALTERADOEM BETWEEN '{dataIni}' AND '{dataFim}' GROUP BY CHAMADOS.ID, CHAMADOS.TITULO, CHAMADOS.STATUS, CHAMADOS.CRIADOPOR, CHAMADOS.CRIADOEM, CHAMADOS.ALTERADOPOR, CHAMADOS.ALTERADOEM, USUARIOSENVOLVIDOS.TIPO, USUARIOS.LOGIN, CHAMADOS.CATEGORIA_ID, CATEGORIAS.TITULO").ToList();
            }
        }

        public List<ViewModelChamadosDashboard> CarregarChamadosDashboardCriacao(string dataIni, string dataFim)
        {
            using (var bd = new ConnectBD())
            {
                return bd.Database.SqlQuery<ViewModelChamadosDashboard>($"SELECT CHAMADOS.*, CATEGORIAS.TITULO AS 'CATEGORIA' FROM CHAMADOS (NOLOCK) INNER JOIN CATEGORIAS (NOLOCK) ON CATEGORIAS.ID = CHAMADOS.CATEGORIA_ID WHERE CHAMADOS.CRIADOEM BETWEEN '{dataIni}' AND '{dataFim}'").ToList();
            }
        }
    }
}