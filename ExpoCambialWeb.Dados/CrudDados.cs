using ExpoCambialWeb.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpoCambialWeb.Dados
{
    public class ExpoCambialResultado
    {
        public DateTime MesReferencia { get; set; }
        public DateTime DataEnvio { get; set; }
        public bool HouveExposicao { get; set; }
        public string NomeDepartamento { get; set; }
        public string? Juncao { get; set; }
        public string NomeUsuario { get; set; }
        public string Email { get; set; }
    }

    public class CrudDados
    {
        public List<string> BuscarNomesDepartamentosPorEmailUsuario(string email)
        {
            try
            {
                using (var context = new ExpoCambialContext())
                {
                    var resultado = (from u in context.Usuarios
                                     join d in context.Departamentos on u.IdDepartamento equals d.IdDepartamento
                                     where u.Email.ToUpper() == email.ToUpper()
                                     select d.NomeDepartamento).ToList();

                    return resultado;
                }
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
                using (var context = new ExpoCambialContext())
                {
                    var resultado = (from a in context.ExpoCambialRegistros
                                     join b in context.Departamentos on a.IdDepartamento equals b.IdDepartamento
                                     join c in context.Usuarios on a.IdUsuario equals c.IdUsuario
                                     where a.MesReferencia.Date == mesRef.Date
                                        && b.NomeDepartamento == nomeDep
                                     select new ExpoCambialResultado
                                     {
                                         MesReferencia = a.MesReferencia,
                                         DataEnvio = a.DataEnvio,
                                         HouveExposicao = a.HouveExposicao,
                                         NomeDepartamento = b.NomeDepartamento,
                                         Juncao = b.Juncao,
                                         NomeUsuario = c.NomeUsuario,
                                         Email = c.Email
                                     }).FirstOrDefault();

                    return resultado;
                }
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
                using (var context = new ExpoCambialContext())
                {
                    var idDepartamento = (from d in context.Departamentos
                                          where d.NomeDepartamento == nomeDep
                                          select d.IdDepartamento).FirstOrDefault();

                    var registroParaAtualizar = (from a in context.ExpoCambialRegistros
                                                 where a.MesReferencia.Date == mesRef.Date
                                                     && a.IdDepartamento == idDepartamento
                                                 select a).FirstOrDefault();

                    if (registroParaAtualizar != null)
                    {
                        registroParaAtualizar.HouveExposicao = houveExposicao;
                        registroParaAtualizar.DataEnvio = DateTime.Now;
                        context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false; // Registro não encontrado
                    }
                }
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
                using (var context = new ExpoCambialContext())
                {
                    var idDepartamento = (from d in context.Departamentos
                                          where d.NomeDepartamento == nomeDep
                                          select d.IdDepartamento).FirstOrDefault();

                    var idUsuario = (from u in context.Usuarios
                                     where u.Email.ToUpper() == emailUsuario.ToUpper()
                                     select u.IdUsuario).FirstOrDefault();

                    var novoRegistro = new ExpoCambialRegistro
                    {
                        MesReferencia = mesRefDateTime,
                        DataEnvio = DateTime.Now,
                        HouveExposicao = houveExposicao,
                        IdDepartamento = idDepartamento,
                        IdUsuario = idUsuario
                    };

                    context.ExpoCambialRegistros.Add(novoRegistro);

                    context.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Busca usuário de autenticação por email
        /// </summary>
        public async Task<UsuarioAuth?> BuscarUsuarioAuthPorEmail(string email)
        {
            try
            {
                using (var context = new ExpoCambialContext())
                {
                    return await context.UsuarioAuth
                        .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() && u.Ativo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar usuário de autenticação: {ex.Message}");
            }
        }

        /// <summary>
        /// Insere novo usuário de autenticação
        /// </summary>
        public async Task<bool> InserirUsuarioAuth(UsuarioAuth usuario)
        {
            try
            {
                using (var context = new ExpoCambialContext())
                {
                    context.UsuarioAuth.Add(usuario);
                    var result = await context.SaveChangesAsync();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao inserir usuário de autenticação: {ex.Message}");
            }
        }

        /// <summary>
        /// Busca usuário na tabela principal por email (para pegar o nome)
        /// </summary>
        public async Task<Usuario?> BuscarUsuarioPorEmail(string email)
        {
            try
            {
                using (var context = new ExpoCambialContext())
                {
                    return await context.Usuarios
                        .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
                }
            }
            catch (Exception ex)
            {
                // Se não encontrar, retorna null sem erro (usuário pode não estar na tabela principal ainda)
                return null;
            }
        }
    }
}
