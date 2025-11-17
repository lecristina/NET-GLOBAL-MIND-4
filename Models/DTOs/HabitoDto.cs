using System.ComponentModel.DataAnnotations;

namespace challenge_3_net.Models.DTOs
{
    /// <summary>
    /// DTO para criação de hábito
    /// </summary>
    public class CriarHabitoDto
    {
        /// <summary>
        /// Tipo do hábito (ex: "Hidratação", "Pausa ativa", "Meditação")
        /// </summary>
        [Required(ErrorMessage = "Tipo do hábito é obrigatório")]
        [StringLength(50, ErrorMessage = "Tipo do hábito deve ter no máximo 50 caracteres")]
        public string TipoHabito { get; set; } = string.Empty;

        /// <summary>
        /// Data do hábito (opcional, padrão: hoje)
        /// </summary>
        public DateTime? DataHabito { get; set; }

        /// <summary>
        /// Pontuação obtida pelo hábito (padrão: 0)
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Pontuação deve ser um número positivo")]
        public int Pontuacao { get; set; } = 0;
    }

    /// <summary>
    /// DTO para resposta de hábito
    /// </summary>
    public class HabitoResponseDto
    {
        /// <summary>
        /// Identificador único do hábito
        /// </summary>
        public int IdHabito { get; set; }

        /// <summary>
        /// ID do usuário
        /// </summary>
        public int IdUsuario { get; set; }

        /// <summary>
        /// Tipo do hábito
        /// </summary>
        public string TipoHabito { get; set; } = string.Empty;

        /// <summary>
        /// Data do hábito
        /// </summary>
        public DateTime DataHabito { get; set; }

        /// <summary>
        /// Pontuação obtida
        /// </summary>
        public int Pontuacao { get; set; }

        /// <summary>
        /// Links HATEOAS
        /// </summary>
        public List<LinkDto> Links { get; set; } = new List<LinkDto>();
    }
}

