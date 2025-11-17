using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace challenge_3_net.Models
{
    /// <summary>
    /// Entidade que representa um hábito registrado pelo usuário
    /// </summary>
    public class Habito
    {
        /// <summary>
        /// Identificador único do hábito
        /// </summary>
        [Key]
        public int IdHabito { get; set; }

        /// <summary>
        /// ID do usuário que registrou o hábito
        /// </summary>
        [Required]
        public int IdUsuario { get; set; }

        /// <summary>
        /// Tipo do hábito (ex: "Hidratação", "Pausa ativa", "Meditação")
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string TipoHabito { get; set; } = string.Empty;

        /// <summary>
        /// Data do hábito
        /// </summary>
        [Required]
        public DateTime DataHabito { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Pontuação obtida pelo hábito
        /// </summary>
        [Required]
        public int Pontuacao { get; set; } = 0;

        // Propriedades de navegação
        /// <summary>
        /// Usuário que registrou o hábito
        /// </summary>
        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; } = null!;
    }
}

