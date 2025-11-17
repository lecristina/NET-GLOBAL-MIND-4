using System.ComponentModel.DataAnnotations;

namespace nexus.Models.DTOs
{
    /// <summary>
    /// DTO para criação de usuário
    /// </summary>
    public class CriarUsuarioDto
    {
        /// <summary>
        /// Nome do usuário
        /// </summary>
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Email do usuário
        /// </summary>
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email deve ter um formato válido")]
        [StringLength(150, ErrorMessage = "Email deve ter no máximo 150 caracteres")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Senha do usuário
        /// </summary>
        [Required(ErrorMessage = "Senha é obrigatória")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Senha deve ter entre 6 e 100 caracteres")]
        public string Senha { get; set; } = string.Empty;

        /// <summary>
        /// Perfil do usuário (PROFISSIONAL ou GESTOR)
        /// </summary>
        [Required(ErrorMessage = "Perfil é obrigatório")]
        [RegularExpression("^(PROFISSIONAL|GESTOR)$", ErrorMessage = "Perfil deve ser PROFISSIONAL ou GESTOR")]
        public string Perfil { get; set; } = string.Empty;

        /// <summary>
        /// Empresa do usuário (opcional)
        /// </summary>
        [StringLength(100, ErrorMessage = "Empresa deve ter no máximo 100 caracteres")]
        public string? Empresa { get; set; }
    }

    /// <summary>
    /// DTO para atualização de usuário
    /// </summary>
    public class AtualizarUsuarioDto
    {
        /// <summary>
        /// Nome do usuário
        /// </summary>
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Email do usuário
        /// </summary>
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email deve ter um formato válido")]
        [StringLength(150, ErrorMessage = "Email deve ter no máximo 150 caracteres")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Perfil do usuário (PROFISSIONAL ou GESTOR)
        /// </summary>
        [Required(ErrorMessage = "Perfil é obrigatório")]
        [RegularExpression("^(PROFISSIONAL|GESTOR)$", ErrorMessage = "Perfil deve ser PROFISSIONAL ou GESTOR")]
        public string Perfil { get; set; } = string.Empty;

        /// <summary>
        /// Empresa do usuário (opcional)
        /// </summary>
        [StringLength(100, ErrorMessage = "Empresa deve ter no máximo 100 caracteres")]
        public string? Empresa { get; set; }
    }

    /// <summary>
    /// DTO para resposta de usuário
    /// </summary>
    public class UsuarioResponseDto
    {
        /// <summary>
        /// Identificador único do usuário
        /// </summary>
        public int IdUsuario { get; set; }

        /// <summary>
        /// Nome do usuário
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Email do usuário
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Perfil do usuário
        /// </summary>
        public string Perfil { get; set; } = string.Empty;

        /// <summary>
        /// Data de cadastro
        /// </summary>
        public DateTime DataCadastro { get; set; }

        /// <summary>
        /// Empresa do usuário
        /// </summary>
        public string? Empresa { get; set; }

        /// <summary>
        /// Links HATEOAS
        /// </summary>
        public List<LinkDto> Links { get; set; } = new List<LinkDto>();
    }
}
