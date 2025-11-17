using System.ComponentModel.DataAnnotations;

namespace nexus.Models.DTOs
{
    /// <summary>
    /// DTO para criação de badge
    /// </summary>
    public class CriarBadgeDto
    {
        /// <summary>
        /// Nome do badge (ex: "Equilíbrio Mental", "Dev Zen")
        /// </summary>
        [Required(ErrorMessage = "Nome do badge é obrigatório")]
        [StringLength(50, ErrorMessage = "Nome do badge deve ter no máximo 50 caracteres")]
        public string NomeBadge { get; set; } = string.Empty;

        /// <summary>
        /// Descrição do badge
        /// </summary>
        [StringLength(150, ErrorMessage = "Descrição deve ter no máximo 150 caracteres")]
        public string? Descricao { get; set; }

        /// <summary>
        /// Pontos necessários para conquistar o badge
        /// </summary>
        [Required(ErrorMessage = "Pontos requeridos é obrigatório")]
        [Range(0, int.MaxValue, ErrorMessage = "Pontos requeridos deve ser um número positivo")]
        public int PontosRequeridos { get; set; }
    }

    /// <summary>
    /// DTO para atualização de badge
    /// </summary>
    public class AtualizarBadgeDto
    {
        /// <summary>
        /// Nome do badge
        /// </summary>
        [Required(ErrorMessage = "Nome do badge é obrigatório")]
        [StringLength(50, ErrorMessage = "Nome do badge deve ter no máximo 50 caracteres")]
        public string NomeBadge { get; set; } = string.Empty;

        /// <summary>
        /// Descrição do badge
        /// </summary>
        [StringLength(150, ErrorMessage = "Descrição deve ter no máximo 150 caracteres")]
        public string? Descricao { get; set; }

        /// <summary>
        /// Pontos necessários para conquistar o badge
        /// </summary>
        [Required(ErrorMessage = "Pontos requeridos é obrigatório")]
        [Range(0, int.MaxValue, ErrorMessage = "Pontos requeridos deve ser um número positivo")]
        public int PontosRequeridos { get; set; }
    }

    /// <summary>
    /// DTO para resposta de badge
    /// </summary>
    public class BadgeResponseDto
    {
        /// <summary>
        /// Identificador único do badge
        /// </summary>
        public int IdBadge { get; set; }

        /// <summary>
        /// Nome do badge
        /// </summary>
        public string NomeBadge { get; set; } = string.Empty;

        /// <summary>
        /// Descrição do badge
        /// </summary>
        public string? Descricao { get; set; }

        /// <summary>
        /// Pontos necessários para conquistar
        /// </summary>
        public int PontosRequeridos { get; set; }

        /// <summary>
        /// Links HATEOAS
        /// </summary>
        public List<LinkDto> Links { get; set; } = new List<LinkDto>();
    }

    /// <summary>
    /// DTO para resposta de conquista de badge por usuário
    /// </summary>
    public class UsuarioBadgeResponseDto
    {
        /// <summary>
        /// ID do usuário
        /// </summary>
        public int IdUsuario { get; set; }

        /// <summary>
        /// ID do badge
        /// </summary>
        public int IdBadge { get; set; }

        /// <summary>
        /// Data da conquista
        /// </summary>
        public DateTime DataConquista { get; set; }

        /// <summary>
        /// Informações do badge
        /// </summary>
        public BadgeResponseDto? Badge { get; set; }

        /// <summary>
        /// Links HATEOAS
        /// </summary>
        public List<LinkDto> Links { get; set; } = new List<LinkDto>();
    }
}

