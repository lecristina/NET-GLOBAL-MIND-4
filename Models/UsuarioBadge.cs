using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace challenge_3_net.Models
{
    /// <summary>
    /// Entidade que representa a relação entre usuário e badge (conquista)
    /// </summary>
    public class UsuarioBadge
    {
        /// <summary>
        /// ID do usuário
        /// </summary>
        [Key]
        [Column(Order = 0)]
        public int IdUsuario { get; set; }

        /// <summary>
        /// ID do badge
        /// </summary>
        [Key]
        [Column(Order = 1)]
        public int IdBadge { get; set; }

        /// <summary>
        /// Data em que o badge foi conquistado
        /// </summary>
        [Required]
        public DateTime DataConquista { get; set; } = DateTime.UtcNow;

        // Propriedades de navegação
        /// <summary>
        /// Usuário que conquistou o badge
        /// </summary>
        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; } = null!;

        /// <summary>
        /// Badge conquistado
        /// </summary>
        [ForeignKey("IdBadge")]
        public virtual Badge Badge { get; set; } = null!;
    }
}

