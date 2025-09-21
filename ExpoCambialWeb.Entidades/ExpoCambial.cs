using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpoCambialWeb.Entidades
{
    [Table("TB_EXPO_CAMBIAL", Schema = "expocambial")]
    public class ExpoCambialRegistro
    {
        /// <summary>
        /// ID do registro - Chave Primária
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_EXPO")]
        public int IdExpo { get; set; }

        /// <summary>
        /// Mês de referência da exposição cambial
        /// </summary>
        [Required]
        [Column("MES_REFERENCIA", TypeName = "date")]
        public DateTime MesReferencia { get; set; }

        /// <summary>
        /// Data e hora do envio da resposta
        /// </summary>
        [Required]
        [Column("DT_ENVIO")]
        public DateTime DataEnvio { get; set; } = DateTime.Now;

        /// <summary>
        /// Indica se houve exposição cambial (true = Sim, false = Não)
        /// </summary>
        [Required]
        [Column("HOUVE_EXPOSICAO")]
        public bool HouveExposicao { get; set; }

        /// <summary>
        /// ID do Departamento - Chave Estrangeira
        /// </summary>
        [Required]
        [Column("ID_DEPARTAMENTO")]
        public int IdDepartamento { get; set; }

        /// <summary>
        /// ID do Usuário - Chave Estrangeira
        /// </summary>
        [Required]
        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

        /// <summary>
        /// Valor da exposição cambial (opcional)
        /// </summary>
        [Column("VALOR", TypeName = "decimal(18,2)")]
        public decimal? Valor { get; set; }

        /// <summary>
        /// Observações adicionais sobre a exposição
        /// </summary>
        [MaxLength(2000)]
        [Column("OBSERVACOES")]
        public string? Observacoes { get; set; }

        // ✅ Propriedades de Navegação (relacionamentos)
        /// <summary>
        /// Departamento responsável pela resposta
        /// </summary>
        [ForeignKey(nameof(IdDepartamento))]
        public virtual Departamento? Departamento { get; set; }

        /// <summary>
        /// Usuário que enviou a resposta
        /// </summary>
        [ForeignKey(nameof(IdUsuario))]
        public virtual Usuario? Usuario { get; set; }
    }
}