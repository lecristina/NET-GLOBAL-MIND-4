using nexus.Models.DTOs;

namespace nexus.Services.Interfaces
{
    public interface IBadgeService
    {
        Task<PagedResultDto<BadgeResponseDto>> ObterTodosAsync(int pageNumber, int pageSize);
        Task<BadgeResponseDto?> ObterPorIdAsync(int id);
        Task<List<UsuarioBadgeResponseDto>> ObterPorUsuarioAsync(int usuarioId);
        Task<BadgeResponseDto> CriarAsync(CriarBadgeDto dto);
        Task<BadgeResponseDto?> AtualizarAsync(int id, AtualizarBadgeDto dto);
        Task<UsuarioBadgeResponseDto?> ConcederBadgeAsync(int usuarioId, int badgeId);
        Task<bool> ExcluirAsync(int id);
    }
}

