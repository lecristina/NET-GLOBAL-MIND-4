using challenge_3_net.Models.DTOs;

namespace challenge_3_net.Services.Interfaces
{
    public interface IAlertaIAService
    {
        Task<PagedResultDto<AlertaIAResponseDto>> ObterTodosAsync(int pageNumber, int pageSize);
        Task<AlertaIAResponseDto?> ObterPorIdAsync(int id);
        Task<List<AlertaIAResponseDto>> ObterPorUsuarioAsync(int usuarioId);
        Task<AlertaIAResponseDto> CriarAsync(CriarAlertaIADto dto);
        Task<bool> ExcluirAsync(int id);
    }
}

