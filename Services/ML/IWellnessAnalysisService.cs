using nexus.Models.DTOs;

namespace nexus.Services.ML
{
    /// <summary>
    /// Interface para serviço de análise completa de bem-estar
    /// </summary>
    public interface IWellnessAnalysisService
    {
        /// <summary>
        /// Realiza análise completa de bem-estar do usuário
        /// </summary>
        Task<AnaliseBemEstarCompletaDto> AnalisarBemEstarCompletoAsync(int idUsuario);

        /// <summary>
        /// Gera alertas inteligentes baseados nos dados do usuário
        /// </summary>
        Task<List<AlertaIAGeradoDto>> GerarAlertasInteligentesAsync(int idUsuario);
    }
}

