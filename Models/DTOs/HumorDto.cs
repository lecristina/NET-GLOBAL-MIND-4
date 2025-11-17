using System.ComponentModel.DataAnnotations;

namespace challenge_3_net.Models.DTOs
{
    /// <summary>
    /// DTO para criação de registro de humor
    /// </summary>
    public class CriarHumorDto
    {
        /// <summary>
        /// Nível de humor (1 a 5)
        /// </summary>
        [Required(ErrorMessage = "Nível de humor é obrigatório")]
        [Range(1, 5, ErrorMessage = "Nível de humor deve estar entre 1 e 5")]
        public int NivelHumor { get; set; }

        /// <summary>
        /// Nível de energia (1 a 5)
        /// </summary>
        [Required(ErrorMessage = "Nível de energia é obrigatório")]
        [Range(1, 5, ErrorMessage = "Nível de energia deve estar entre 1 e 5")]
        public int NivelEnergia { get; set; }

        /// <summary>
        /// Comentário opcional sobre o humor
        /// </summary>
        [StringLength(255, ErrorMessage = "Comentário deve ter no máximo 255 caracteres")]
        public string? Comentario { get; set; }
    }

    /// <summary>
    /// DTO para atualização de registro de humor
    /// </summary>
    public class AtualizarHumorDto
    {
        /// <summary>
        /// Nível de humor (1 a 5)
        /// </summary>
        [Required(ErrorMessage = "Nível de humor é obrigatório")]
        [Range(1, 5, ErrorMessage = "Nível de humor deve estar entre 1 e 5")]
        public int NivelHumor { get; set; }

        /// <summary>
        /// Nível de energia (1 a 5)
        /// </summary>
        [Required(ErrorMessage = "Nível de energia é obrigatório")]
        [Range(1, 5, ErrorMessage = "Nível de energia deve estar entre 1 e 5")]
        public int NivelEnergia { get; set; }

        /// <summary>
        /// Comentário opcional sobre o humor
        /// </summary>
        [StringLength(255, ErrorMessage = "Comentário deve ter no máximo 255 caracteres")]
        public string? Comentario { get; set; }
    }

    /// <summary>
    /// DTO para resposta de registro de humor
    /// </summary>
    public class HumorResponseDto
    {
        /// <summary>
        /// Identificador único do registro
        /// </summary>
        public int IdHumor { get; set; }

        /// <summary>
        /// ID do usuário
        /// </summary>
        public int IdUsuario { get; set; }

        /// <summary>
        /// Data do registro
        /// </summary>
        public DateTime DataRegistro { get; set; }

        /// <summary>
        /// Nível de humor (1 a 5)
        /// </summary>
        public int NivelHumor { get; set; }

        /// <summary>
        /// Nível de energia (1 a 5)
        /// </summary>
        public int NivelEnergia { get; set; }

        /// <summary>
        /// Comentário sobre o humor
        /// </summary>
        public string? Comentario { get; set; }

        /// <summary>
        /// Links HATEOAS
        /// </summary>
        public List<LinkDto> Links { get; set; } = new List<LinkDto>();
    }
}

