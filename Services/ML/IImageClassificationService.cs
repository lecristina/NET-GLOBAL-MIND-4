using nexus.Models.DTOs;

namespace nexus.Services.ML
{
    /// <summary>
    /// Interface para serviço de classificação de imagens
    /// </summary>
    public interface IImageClassificationService
    {
        /// <summary>
        /// Classifica uma imagem e analisa o ambiente de trabalho
        /// </summary>
        Task<ClassificacaoImagemResponseDto> ClassificarImagemAsync(string imagemBase64, string? descricao = null);

        /// <summary>
        /// Valida se a imagem é válida
        /// </summary>
        bool ValidarImagem(string imagemBase64);
    }
}

