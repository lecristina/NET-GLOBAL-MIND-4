using nexus.Models;

namespace nexus.Repositories.Interfaces
{
    public interface IBadgeRepository : IRepository<Badge>
    {
        Task<(IEnumerable<Badge> Items, long TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<UsuarioBadge>> GetByUsuarioAsync(int usuarioId);
        Task<UsuarioBadge?> GetUsuarioBadgeAsync(int usuarioId, int badgeId);
        Task<UsuarioBadge> AddUsuarioBadgeAsync(UsuarioBadge usuarioBadge);
    }
}

