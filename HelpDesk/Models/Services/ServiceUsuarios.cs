using HelpDesk.Models.Repository;
using HelpDesk.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace HelpDesk.Models.Services
{
    public class ServiceUsuarios
    {
        private UsuariosRepository _usuarioRepository = new UsuariosRepository();
        private GlobalRepository _globalRepository = new GlobalRepository();

        public List<ViewModelUsuarios> BuscarUsuarios(string usuario, string perfil)
        {
            try
            {
                return _usuarioRepository.BuscarUsuarios(usuario, perfil);
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
                return new List<ViewModelUsuarios>();
            }
        }

        public ViewModelUsuarios BuscarUsuario(string usuario)
        {
            try
            {
                return _usuarioRepository.BuscarUsuario(usuario);
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
                return new ViewModelUsuarios();
            }
        }
        public List<ViewModelUsuarios> BuscarTodosUsuarios()
        {
            try
            {
                return _usuarioRepository.BuscarTodosUsuarios();
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
                return new List<ViewModelUsuarios>();
            }
        }

        public void MudarStatusRedefinirSenha(bool status, string usuario)
        {
            try
            {
                var redefinir = 0;
                if (status == true)
                {
                    redefinir = 1;
                }
                var query = $"UPDATE USUARIOS SET REDEFINIRSENHA = '{redefinir}' WHERE LOGIN = '{usuario}'";
                _globalRepository.ExecutarComandoSQL(query);
            }
            catch (Exception Ex)
            {
                _globalRepository.SalvarLog(Ex.Message, MethodBase.GetCurrentMethod().Name, "ERRO", "");
            }
        }
    }
}