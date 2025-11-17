using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace challenge_3_net.Models
{
    /// <summary>
    /// Entidade que representa um usuário do sistema MindTrack
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Identificador único do usuário
        /// </summary>
        [Key]
        public int IdUsuario { get; set; }

        /// <summary>
        /// Nome do usuário
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Email do usuário (único)
        /// </summary>
        [Required]
        [MaxLength(150)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Hash da senha do usuário
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string SenhaHash { get; set; } = string.Empty;

        /// <summary>
        /// Perfil do usuário no sistema
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Perfil { get; set; } = string.Empty; // 'PROFISSIONAL' ou 'GESTOR'

        /// <summary>
        /// Data de cadastro do usuário
        /// </summary>
        [Required]
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Empresa do usuário (opcional)
        /// </summary>
        [MaxLength(100)]
        public string? Empresa { get; set; }

        // Propriedades de navegação
        /// <summary>
        /// Lista de registros de humor do usuário
        /// </summary>
        public virtual ICollection<Humor> Humores { get; set; } = new List<Humor>();

        /// <summary>
        /// Lista de sprints do usuário
        /// </summary>
        public virtual ICollection<Sprint> Sprints { get; set; } = new List<Sprint>();

        /// <summary>
        /// Lista de alertas de IA para o usuário
        /// </summary>
        public virtual ICollection<AlertaIA> AlertasIA { get; set; } = new List<AlertaIA>();

        /// <summary>
        /// Lista de hábitos do usuário
        /// </summary>
        public virtual ICollection<Habito> Habitos { get; set; } = new List<Habito>();

        /// <summary>
        /// Lista de badges conquistados pelo usuário
        /// </summary>
        public virtual ICollection<UsuarioBadge> UsuarioBadges { get; set; } = new List<UsuarioBadge>();
    }
}
