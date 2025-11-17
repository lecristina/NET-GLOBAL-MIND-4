using challenge_3_net.Models;

namespace challenge_3_net.Repositories.Interfaces
{
    public interface IHabitoRepository : IRepository<Habito>
    {
        Task<(IEnumerable<Habito> Items, long TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Habito>> GetByUsuarioAsync(int usuarioId);
    }
}

