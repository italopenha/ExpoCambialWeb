using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpoCambialWeb.Entidades
{
    [Table("TB_USUARIO_AUT", Schema = "expocambial")]
    public class UsuarioAuth
    {
        /// <summary>
        /// ID do usuário de autenticação - Chave Primária
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

        /// <summary>
        /// Email do usuário (único)
        /// </summary>
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        [Column("EMAIL")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Hash da senha (BCrypt)
        /// </summary>
        [Required]
        [MaxLength(255)]
        [Column("SENHA_HASH")]
        public string SenhaHash { get; set; } = string.Empty;

        /// <summary>
        /// Data de criação da conta
        /// </summary>
        [Required]
        [Column("DATA_CRIACAO")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        /// <summary>
        /// Indica se a conta está ativa
        /// </summary>
        [Required]
        [Column("ATIVO")]
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// Data da última atualização
        /// </summary>
        [Column("DATA_ATUALIZACAO")]
        public DateTime? DataAtualizacao { get; set; }
    }

    /// <summary>
    /// Resultado de operações de autenticação
    /// </summary>
    public class ResultadoAuth
    {
        public bool Sucesso { get; set; }
        public string Erro { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
    }
}