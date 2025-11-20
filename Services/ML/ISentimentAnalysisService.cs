using nexus.Models.DTOs;

namespace nexus.Services.ML
{
    /// <summary>
    /// Interface para serviço de análise de sentimento
    /// </summary>
    public interface ISentimentAnalysisService
    {
        /// <summary>
        /// Analisa o sentimento de um texto e gera recomendações
        /// </summary>
        Task<AnaliseSentimentoResponseDto> AnalisarSentimentoAsync(string texto);

        /// <summary>
        /// Analisa múltiplos textos e retorna análise agregada
        /// </summary>
        Task<AnaliseSentimentoResponseDto> AnalisarSentimentosAsync(IEnumerable<string> textos);
    }
}

