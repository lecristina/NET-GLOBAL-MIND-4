using nexus.Models;

namespace nexus.Repositories.Interfaces
{
    public interface ISprintRepository : IRepository<Sprint>
    {
        Task<(IEnumerable<Sprint> Items, long TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Sprint>> GetByUsuarioAsync(int usuarioId);
        Task<Sprint?> GetByUsuarioAndNomeAsync(int usuarioId, string nomeSprint);
    }
}

