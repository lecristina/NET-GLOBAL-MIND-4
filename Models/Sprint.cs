using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace challenge_3_net.Models
{
    /// <summary>
    /// Entidade que representa uma sprint de trabalho do usuário
    /// </summary>
    public class Sprint
    {
        /// <summary>
        /// Identificador único da sprint
        /// </summary>
        [Key]
        public int IdSprint { get; set; }

        /// <summary>
        /// ID do usuário dono da sprint
        /// </summary>
        [Required]
        public int IdUsuario { get; set; }

        /// <summary>
        /// Nome da sprint
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string NomeSprint { get; set; } = string.Empty;

        /// <summary>
        /// Data de início da sprint
        /// </summary>
        [Required]
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Data de fim da sprint (opcional)
        /// </summary>
        public DateTime? DataFim { get; set; }

        /// <summary>
        /// Nível de produtividade (0.00 a 100.00)
        /// </summary>
        [Range(0, 100)]
        public decimal? Produtividade { get; set; }

        /// <summary>
        /// Número de tarefas concluídas
        /// </summary>
        public int? TarefasConcluidas { get; set; }

        /// <summary>
        /// Número de commits realizados
        /// </summary>
        public int? Commits { get; set; }

        // Propriedades de navegação
        /// <summary>
        /// Usuário dono da sprint
        /// </summary>
        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; } = null!;
    }
}

