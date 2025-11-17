using System.ComponentModel.DataAnnotations;

namespace nexus.Models.DTOs
{
    /// <summary>
    /// DTO para criação de sprint
    /// </summary>
    public class CriarSprintDto
    {
        /// <summary>
        /// Nome da sprint
        /// </summary>
        [Required(ErrorMessage = "Nome da sprint é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome da sprint deve ter no máximo 100 caracteres")]
        public string NomeSprint { get; set; } = string.Empty;

        /// <summary>
        /// Data de início da sprint
        /// </summary>
        [Required(ErrorMessage = "Data de início é obrigatória")]
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Data de fim da sprint (opcional)
        /// </summary>
        public DateTime? DataFim { get; set; }

        /// <summary>
        /// Nível de produtividade (0.00 a 100.00)
        /// </summary>
        [Range(0, 100, ErrorMessage = "Produtividade deve estar entre 0 e 100")]
        public decimal? Produtividade { get; set; }

        /// <summary>
        /// Número de tarefas concluídas
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Tarefas concluídas deve ser um número positivo")]
        public int? TarefasConcluidas { get; set; }

        /// <summary>
        /// Número de commits realizados
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Commits deve ser um número positivo")]
        public int? Commits { get; set; }
    }

    /// <summary>
    /// DTO para atualização de sprint
    /// </summary>
    public class AtualizarSprintDto
    {
        /// <summary>
        /// Nome da sprint
        /// </summary>
        [Required(ErrorMessage = "Nome da sprint é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome da sprint deve ter no máximo 100 caracteres")]
        public string NomeSprint { get; set; } = string.Empty;

        /// <summary>
        /// Data de início da sprint
        /// </summary>
        [Required(ErrorMessage = "Data de início é obrigatória")]
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Data de fim da sprint (opcional)
        /// </summary>
        public DateTime? DataFim { get; set; }

        /// <summary>
        /// Nível de produtividade (0.00 a 100.00)
        /// </summary>
        [Range(0, 100, ErrorMessage = "Produtividade deve estar entre 0 e 100")]
        public decimal? Produtividade { get; set; }

        /// <summary>
        /// Número de tarefas concluídas
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Tarefas concluídas deve ser um número positivo")]
        public int? TarefasConcluidas { get; set; }

        /// <summary>
        /// Número de commits realizados
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Commits deve ser um número positivo")]
        public int? Commits { get; set; }
    }

    /// <summary>
    /// DTO para resposta de sprint
    /// </summary>
    public class SprintResponseDto
    {
        /// <summary>
        /// Identificador único da sprint
        /// </summary>
        public int IdSprint { get; set; }

        /// <summary>
        /// ID do usuário
        /// </summary>
        public int IdUsuario { get; set; }

        /// <summary>
        /// Nome da sprint
        /// </summary>
        public string NomeSprint { get; set; } = string.Empty;

        /// <summary>
        /// Data de início
        /// </summary>
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Data de fim
        /// </summary>
        public DateTime? DataFim { get; set; }

        /// <summary>
        /// Nível de produtividade
        /// </summary>
        public decimal? Produtividade { get; set; }

        /// <summary>
        /// Tarefas concluídas
        /// </summary>
        public int? TarefasConcluidas { get; set; }

        /// <summary>
        /// Commits realizados
        /// </summary>
        public int? Commits { get; set; }

        /// <summary>
        /// Links HATEOAS
        /// </summary>
        public List<LinkDto> Links { get; set; } = new List<LinkDto>();
    }
}

