using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpoCambialWeb.Entidades
{
    [Table("TB_USUARIO", Schema = "expocambial")]
    public class Usuario
    {
        /// <summary>
        /// ID do Usuário - Chave Primária
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

        /// <summary>
        /// Nome completo do usuário
        /// </summary>
        [Required]
        [MaxLength(100)]
        [Column("NOME_USUARIO")]
        public string NomeUsuario { get; set; } = string.Empty;

        /// <summary>
        /// Email do usuário (único)
        /// </summary>
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        [Column("EMAIL")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// ID do Departamento - Chave Estrangeira
        /// </summary>
        [Required]
        [Column("ID_DEPARTAMENTO")]
        public int IdDepartamento { get; set; }

        /// <summary>
        /// ID do Tipo de Usuário - Chave Estrangeira
        /// </summary>
        [Required]
        [Column("ID_TIPO_USUARIO")]
        public int IdTipoUsuario { get; set; }

        /// <summary>
        /// Status do usuário (Ativo, Inativo, Suspenso)
        /// </summary>
        [MaxLength(10)]
        [Column("STATUS")]
        public string? Status { get; set; }

        // ✅ Propriedades de Navegação (relacionamentos)
        /// <summary>
        /// Departamento do usuário
        /// </summary>
        [ForeignKey(nameof(IdDepartamento))]
        public virtual Departamento? Departamento { get; set; }

        /// <summary>
        /// Tipo de usuário
        /// </summary>
        [ForeignKey(nameof(IdTipoUsuario))]
        public virtual TipoUsuario? TipoUsuario { get; set; }
    }
}