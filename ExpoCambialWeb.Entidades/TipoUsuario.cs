using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpoCambialWeb.Entidades
{
    [Table("TB_TIPO_USUARIO", Schema = "expocambial")]
    public class TipoUsuario
    {
        /// <summary>
        /// ID do Tipo de Usuário - Chave Primária
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_TIPO_USUARIO")]
        public int IdTipoUsuario { get; set; }

        /// <summary>
        /// Descrição do Tipo de Usuário (ex: Admin, Comum, Gestor)
        /// </summary>
        [Required]
        [MaxLength(10)]
        [Column("TIPO_USUARIO")]
        public string TipoUsuarioDescricao { get; set; } = string.Empty;
    }
}