using nexus.Models;

namespace nexus.Repositories.Interfaces
{
    public interface IHabitoRepository : IRepository<Habito>
    {
        Task<(IEnumerable<Habito> Items, long TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Habito>> GetByUsuarioAsync(int usuarioId);
    }
}

