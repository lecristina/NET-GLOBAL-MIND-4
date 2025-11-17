using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nexus.Models
{
    /// <summary>
    /// Entidade que representa um alerta gerado por IA para o usuário
    /// </summary>
    public class AlertaIA
    {
        /// <summary>
        /// Identificador único do alerta
        /// </summary>
        [Key]
        public int IdAlerta { get; set; }

        /// <summary>
        /// ID do usuário que recebeu o alerta
        /// </summary>
        [Required]
        public int IdUsuario { get; set; }

        /// <summary>
        /// Data do alerta
        /// </summary>
        [Required]
        public DateTime DataAlerta { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Tipo do alerta (ex: "Burnout", "Sobrecarga", "Equilíbrio")
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string TipoAlerta { get; set; } = string.Empty;

        /// <summary>
        /// Mensagem do alerta
        /// </summary>
        [MaxLength(255)]
        public string? Mensagem { get; set; }

        /// <summary>
        /// Nível de risco (1 a 5)
        /// </summary>
        [Required]
        [Range(1, 5)]
        public int NivelRisco { get; set; }

        // Propriedades de navegação
        /// <summary>
        /// Usuário que recebeu o alerta
        /// </summary>
        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; } = null!;
    }
}

