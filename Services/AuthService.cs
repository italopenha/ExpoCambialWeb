using ExpoCambialWeb.Negocios;

namespace ExpoCambialWeb.Services
{
    public class AuthService
    {
        private readonly CrudNegocios _negocios;
        private string? _usuarioLogado;
        private string? _emailLogado;

        public AuthService(CrudNegocios negocios)
        {
            _negocios = negocios;
        }

        public bool EstaLogado => !string.IsNullOrEmpty(_usuarioLogado);

        public string? UsuarioLogado => _usuarioLogado;

        public string? EmailLogado => _emailLogado;

        public async Task<bool> Login(string email, string senha)
        {
            try
            {
                // Aqui você implementaria a lógica real de autenticação
                // Por exemplo, verificar no banco de dados

                // Simulação de validação (SUBSTITUA pela lógica real)
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
                    return false;

                if (senha.Length < 4) // Validação mínima simples
                    return false;

                // Buscar usuário no banco (simulação)
                var usuario = await BuscarUsuarioPorEmail(email);
                if (usuario == null)
                    return false;

                // Verificar senha (em produção, use hash)
                bool senhaValida = VerificarSenha(senha, usuario.SenhaHash);

                if (senhaValida)
                {
                    _emailLogado = email;
                    _usuarioLogado = usuario.Nome;
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public void Logout()
        {
            _usuarioLogado = null;
            _emailLogado = null;
        }

        private async Task<UsuarioAuth?> BuscarUsuarioPorEmail(string email)
        {
            // IMPLEMENTAR: Buscar usuário no banco de dados
            // Por enquanto, simulação
            await Task.Delay(100);

            // Simular alguns usuários para teste
            var usuariosSimulados = new List<UsuarioAuth>
            {
                new UsuarioAuth { Email = "admin@empresa.com", Nome = "Administrador", SenhaHash = "admin123" },
                new UsuarioAuth { Email = "italo@empresa.com", Nome = "ITALO PAIVA PENHA", SenhaHash = "1234" },
                new UsuarioAuth { Email = "maria@empresa.com", Nome = "MARIA SILVA", SenhaHash = "senha123" },
                new UsuarioAuth { Email = "italopenha77@outlook.com", Nome = "ITALO PENHA", SenhaHash = "teste" }
            };

            return usuariosSimulados.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
        }

        private bool VerificarSenha(string senha, string senhaHash)
        {
            // IMPLEMENTAR: Verificação com hash real (bcrypt, etc)
            // Por enquanto, comparação direta para simulação
            return senha == senhaHash;
        }
    }

    public class UsuarioAuth
    {
        public string Email { get; set; } = "";
        public string Nome { get; set; } = "";
        public string SenhaHash { get; set; } = "";
    }
}