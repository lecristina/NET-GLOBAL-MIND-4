using System.ComponentModel.DataAnnotations;

namespace challenge_3_net.Models.DTOs
{
    /// <summary>
    /// DTO para criação de alerta de IA
    /// </summary>
    public class CriarAlertaIADto
    {
        /// <summary>
        /// Tipo do alerta (ex: "Burnout", "Sobrecarga", "Equilíbrio")
        /// </summary>
        [Required(ErrorMessage = "Tipo do alerta é obrigatório")]
        [StringLength(50, ErrorMessage = "Tipo do alerta deve ter no máximo 50 caracteres")]
        public string TipoAlerta { get; set; } = string.Empty;

        /// <summary>
        /// Mensagem do alerta
        /// </summary>
        [StringLength(255, ErrorMessage = "Mensagem deve ter no máximo 255 caracteres")]
        public string? Mensagem { get; set; }

        /// <summary>
        /// Nível de risco (1 a 5)
        /// </summary>
        [Required(ErrorMessage = "Nível de risco é obrigatório")]
        [Range(1, 5, ErrorMessage = "Nível de risco deve estar entre 1 e 5")]
        public int NivelRisco { get; set; }
    }

    /// <summary>
    /// DTO para resposta de alerta de IA
    /// </summary>
    public class AlertaIAResponseDto
    {
        /// <summary>
        /// Identificador único do alerta
        /// </summary>
        public int IdAlerta { get; set; }

        /// <summary>
        /// ID do usuário
        /// </summary>
        public int IdUsuario { get; set; }

        /// <summary>
        /// Data do alerta
        /// </summary>
        public DateTime DataAlerta { get; set; }

        /// <summary>
        /// Tipo do alerta
        /// </summary>
        public string TipoAlerta { get; set; } = string.Empty;

        /// <summary>
        /// Mensagem do alerta
        /// </summary>
        public string? Mensagem { get; set; }

        /// <summary>
        /// Nível de risco (1 a 5)
        /// </summary>
        public int NivelRisco { get; set; }

        /// <summary>
        /// Links HATEOAS
        /// </summary>
        public List<LinkDto> Links { get; set; } = new List<LinkDto>();
    }
}

