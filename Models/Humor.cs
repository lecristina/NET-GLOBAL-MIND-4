using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nexus.Models
{
    /// <summary>
    /// Entidade que representa um registro de humor do usuário
    /// </summary>
    public class Humor
    {
        /// <summary>
        /// Identificador único do registro de humor
        /// </summary>
        [Key]
        public int IdHumor { get; set; }

        /// <summary>
        /// ID do usuário que registrou o humor
        /// </summary>
        [Required]
        public int IdUsuario { get; set; }

        /// <summary>
        /// Data do registro
        /// </summary>
        [Required]
        public DateTime DataRegistro { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Nível de humor (1 a 5)
        /// </summary>
        [Required]
        [Range(1, 5)]
        public int NivelHumor { get; set; }

        /// <summary>
        /// Nível de energia (1 a 5)
        /// </summary>
        [Required]
        [Range(1, 5)]
        public int NivelEnergia { get; set; }

        /// <summary>
        /// Comentário opcional sobre o humor
        /// </summary>
        [MaxLength(255)]
        public string? Comentario { get; set; }

        // Propriedades de navegação
        /// <summary>
        /// Usuário que registrou o humor
        /// </summary>
        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; } = null!;
    }
}

