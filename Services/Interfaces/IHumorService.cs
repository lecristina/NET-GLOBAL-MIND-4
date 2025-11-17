using nexus.Models.DTOs;

namespace nexus.Services.Interfaces
{
    public interface IHumorService
    {
        Task<PagedResultDto<HumorResponseDto>> ObterTodosAsync(int pageNumber, int pageSize);
        Task<HumorResponseDto?> ObterPorIdAsync(int id);
        Task<List<HumorResponseDto>> ObterPorUsuarioAsync(int usuarioId);
        Task<HumorResponseDto> CriarAsync(CriarHumorDto dto);
        Task<HumorResponseDto?> AtualizarAsync(int id, AtualizarHumorDto dto);
        Task<bool> ExcluirAsync(int id);
    }
}

