using HelpDesk.Models.Entities;
using HelpDesk.Models.Repository;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace HelpDesk.Models.Services
{
    public class ServiceCategorias
    {
        private CategoriasRepository _categoriasRepository = new CategoriasRepository();
        private GlobalRepository _globalRepository = new GlobalRepository();

        public List<CATEGORIAS> BuscarCategorias()
        {
            try
            {
                return _categoriasRepository.BuscarCategorias();
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
                return new List<CATEGORIAS>();
            }
        }

        public void CadastrarCategoria(CATEGORIAS categoria)
        {
            try
            {
                _categoriasRepository.CadastrarCategoria(categoria);
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
            }
        }

        public void AtualizarCategoria(CATEGORIAS categoria)
        {
            try
            {
                _categoriasRepository.AtualizarCategoria(categoria);
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
            }
        }

        public void ExcluirCategoria(CATEGORIAS categoria)
        {
            try
            {
                _categoriasRepository.ExcluirCategoria(categoria);
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
            }
        }
    }
}