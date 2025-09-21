using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpoCambialWeb.Entidades
{
    [Table("TB_DEPARTAMENTO", Schema = "expocambial")]
    public class Departamento
    {
        /// <summary>
        /// ID do Departamento - Chave Primária
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_DEPARTAMENTO")]
        public int IdDepartamento { get; set; }

        /// <summary>
        /// Nome do Departamento
        /// </summary>
        [Required]
        [MaxLength(100)]
        [Column("NOME_DEPARTAMENTO")]
        public string NomeDepartamento { get; set; } = string.Empty;

        /// <summary>
        /// Junção/Descrição adicional do departamento
        /// </summary>
        [MaxLength(20)]
        [Column("JUNCAO")]
        public string? Juncao { get; set; }
    }
}