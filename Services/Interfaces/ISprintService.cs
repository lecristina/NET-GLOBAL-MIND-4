using nexus.Models.DTOs;

namespace nexus.Services.Interfaces
{
    public interface ISprintService
    {
        Task<PagedResultDto<SprintResponseDto>> ObterTodosAsync(int pageNumber, int pageSize);
        Task<SprintResponseDto?> ObterPorIdAsync(int id);
        Task<List<SprintResponseDto>> ObterPorUsuarioAsync(int usuarioId);
        Task<SprintResponseDto> CriarAsync(CriarSprintDto dto);
        Task<SprintResponseDto?> AtualizarAsync(int id, AtualizarSprintDto dto);
        Task<bool> ExcluirAsync(int id);
    }
}

