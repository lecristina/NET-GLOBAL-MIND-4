using challenge_3_net.Models.DTOs;

namespace challenge_3_net.Services.Interfaces
{
    public interface IHabitoService
    {
        Task<PagedResultDto<HabitoResponseDto>> ObterTodosAsync(int pageNumber, int pageSize);
        Task<HabitoResponseDto?> ObterPorIdAsync(int id);
        Task<List<HabitoResponseDto>> ObterPorUsuarioAsync(int usuarioId);
        Task<HabitoResponseDto> CriarAsync(CriarHabitoDto dto);
        Task<bool> ExcluirAsync(int id);
    }
}

