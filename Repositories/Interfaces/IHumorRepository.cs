using nexus.Models;

namespace nexus.Repositories.Interfaces
{
    public interface IHumorRepository : IRepository<Humor>
    {
        Task<(IEnumerable<Humor> Items, long TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Humor>> GetByUsuarioAsync(int usuarioId);
    }
}

