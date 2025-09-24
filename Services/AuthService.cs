using ExpoCambialWeb.Entidades;
using ExpoCambialWeb.Dados;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.JSInterop;
using Org.BouncyCastle.Crypto.Generators;

namespace ExpoCambialWeb.Services
{
    public class AuthService
    {
        private readonly CrudDados _dados;
        private readonly IJSRuntime _jsRuntime;
        private readonly string _jwtSecret = "ExpoCambial2025SuperSecretKeyForJwtToken123456789"; // Em produção, use configuração
        private string? _tokenAtual;

        public AuthService(CrudDados dados, IJSRuntime jsRuntime)
        {
            _dados = dados;
            _jsRuntime = jsRuntime;
        }

        public bool EstaLogado => !string.IsNullOrEmpty(_tokenAtual) && TokenValido();

        public string? UsuarioLogado { get; private set; }

        public string? EmailLogado { get; private set; }

        /// <summary>
        /// Realiza login com email e senha
        /// </summary>
        public async Task<ResultadoAuth> Login(string email, string senha)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
                {
                    return new ResultadoAuth
                    {
                        Sucesso = false,
                        Erro = "Email e senha são obrigatórios"
                    };
                }

                // Buscar usuário no banco
                var usuario = await _dados.BuscarUsuarioAuthPorEmail(email);
                if (usuario == null)
                {
                    return new ResultadoAuth
                    {
                        Sucesso = false,
                        Erro = "Email ou senha incorretos"
                    };
                }

                if (!usuario.Ativo)
                {
                    return new ResultadoAuth
                    {
                        Sucesso = false,
                        Erro = "Conta desativada. Entre em contato com o administrador"
                    };
                }

                // Verificar senha
                if (!VerificarSenha(senha, usuario.SenhaHash))
                {
                    return new ResultadoAuth
                    {
                        Sucesso = false,
                        Erro = "Email ou senha incorretos"
                    };
                }

                // Gerar JWT token
                var token = GerarJwtToken(usuario);

                // Definir informações do usuário logado
                _tokenAtual = token;
                EmailLogado = usuario.Email;

                // Buscar nome do usuário na tabela principal
                UsuarioLogado = await BuscarNomeUsuario(email);

                // Salvar token no localStorage do browser
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);

                return new ResultadoAuth
                {
                    Sucesso = true,
                    Token = token,
                    Email = usuario.Email,
                    Nome = UsuarioLogado ?? email
                };
            }
            catch (Exception ex)
            {
                return new ResultadoAuth
                {
                    Sucesso = false,
                    Erro = $"Erro interno: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Cadastra novo usuário
        /// </summary>
        public async Task<ResultadoAuth> CadastrarUsuario(string email, string senha)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
                {
                    return new ResultadoAuth
                    {
                        Sucesso = false,
                        Erro = "Email e senha são obrigatórios"
                    };
                }

                if (senha.Length < 6)
                {
                    return new ResultadoAuth
                    {
                        Sucesso = false,
                        Erro = "Senha deve ter no mínimo 6 caracteres"
                    };
                }

                // Verificar se email já existe
                var usuarioExistente = await _dados.BuscarUsuarioAuthPorEmail(email);
                if (usuarioExistente != null)
                {
                    return new ResultadoAuth
                    {
                        Sucesso = false,
                        Erro = "Este email já está cadastrado"
                    };
                }

                // Criar hash da senha
                var senhaHash = CriarHashSenha(senha);

                // Criar usuário de autenticação
                var novoUsuario = new UsuarioAuth
                {
                    Email = email.ToLower().Trim(),
                    SenhaHash = senhaHash,
                    DataCriacao = DateTime.Now,
                    Ativo = true
                };

                // Salvar no banco
                var sucesso = await _dados.InserirUsuarioAuth(novoUsuario);

                if (sucesso)
                {
                    return new ResultadoAuth
                    {
                        Sucesso = true,
                        Email = email
                    };
                }
                else
                {
                    return new ResultadoAuth
                    {
                        Sucesso = false,
                        Erro = "Erro ao criar conta. Tente novamente"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResultadoAuth
                {
                    Sucesso = false,
                    Erro = $"Erro interno: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Restaura sessão do localStorage
        /// </summary>
        public async Task<bool> RestaurarSessao()
        {
            try
            {
                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

                if (string.IsNullOrEmpty(token))
                    return false;

                _tokenAtual = token;

                if (TokenValido())
                {
                    var claims = ExtrairClaimsToken(token);
                    EmailLogado = claims.FirstOrDefault(c => c.Type == "email")?.Value;
                    UsuarioLogado = await BuscarNomeUsuario(EmailLogado ?? "");
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Realiza logout
        /// </summary>
        public async Task Logout()
        {
            _tokenAtual = null;
            UsuarioLogado = null;
            EmailLogado = null;

            // Remover do localStorage
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
        }

        /// <summary>
        /// Cria hash seguro da senha usando BCrypt
        /// </summary>
        private string CriarHashSenha(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha, workFactor: 12);
        }

        /// <summary>
        /// Verifica se a senha confere com o hash
        /// </summary>
        private bool VerificarSenha(string senha, string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(senha, hash);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gera token JWT
        /// </summary>
        private string GerarJwtToken(UsuarioAuth usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", usuario.IdUsuario.ToString()),
                    new Claim("email", usuario.Email),
                    new Claim(ClaimTypes.Name, usuario.Email),
                    new Claim(ClaimTypes.Email, usuario.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7), // Token válido por 7 dias
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Verifica se o token atual é válido
        /// </summary>
        private bool TokenValido()
        {
            if (string.IsNullOrEmpty(_tokenAtual))
                return false;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSecret);

                tokenHandler.ValidateToken(_tokenAtual, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return validatedToken != null;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Extrai claims do token
        /// </summary>
        private IEnumerable<Claim> ExtrairClaimsToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jsonToken = tokenHandler.ReadJwtToken(token);
                return jsonToken.Claims;
            }
            catch
            {
                return new List<Claim>();
            }
        }

        /// <summary>
        /// Busca nome do usuário na tabela principal
        /// </summary>
        private async Task<string> BuscarNomeUsuario(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return "";

                // Buscar na tabela TB_USUARIO usando o método existente
                var usuario = await _dados.BuscarUsuarioPorEmail(email);
                return usuario?.NomeUsuario ?? email;
            }
            catch
            {
                return email;
            }
        }
    }
}