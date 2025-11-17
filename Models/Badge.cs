using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace challenge_3_net.Models
{
    /// <summary>
    /// Entidade que representa um badge/conquista disponível no sistema
    /// </summary>
    public class Badge
    {
        /// <summary>
        /// Identificador único do badge
        /// </summary>
        [Key]
        public int IdBadge { get; set; }

        /// <summary>
        /// Nome do badge (ex: "Equilíbrio Mental", "Dev Zen")
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string NomeBadge { get; set; } = string.Empty;

        /// <summary>
        /// Descrição do badge
        /// </summary>
        [MaxLength(150)]
        public string? Descricao { get; set; }

        /// <summary>
        /// Pontos necessários para conquistar o badge
        /// </summary>
        [Required]
        public int PontosRequeridos { get; set; }

        // Propriedades de navegação
        /// <summary>
        /// Lista de usuários que conquistaram este badge
        /// </summary>
        public virtual ICollection<UsuarioBadge> UsuarioBadges { get; set; } = new List<UsuarioBadge>();
    }
}

