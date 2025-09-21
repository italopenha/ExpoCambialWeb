using ExpoCambialWeb.Dados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpoCambialWeb.Negocios
{
    public class CrudNegocios
    {
        private readonly ExpoCambialWeb.Dados.CrudDados objDad = new ExpoCambialWeb.Dados.CrudDados();

        public List<String> BuscarNomesDepartamentosPorEmailUsuario(string email)
        {
            try
            {
                return objDad.BuscarNomesDepartamentosPorEmailUsuario(email);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public ExpoCambialResultado? BuscarRegistrosExistentesExpoCambial(DateTime mesRef, string nomeDep)
        {
            try
            {
                return objDad.BuscarRegistrosExistentesExpoCambial(mesRef, nomeDep);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public bool AtualizarRegistroExpoCambial(DateTime mesRef, bool houveExposicao, string nomeDep)
        {
            try
            {
                return objDad.AtualizarRegistroExpoCambial(mesRef, houveExposicao, nomeDep);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public bool InserirRegistroExpoCambial(string emailUsuario, string nomeDep, DateTime mesRefDateTime, bool houveExposicao)
        {
            try
            {
                return objDad.InserirRegistroExpoCambial(emailUsuario, nomeDep, mesRefDateTime, houveExposicao);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
    }
}
