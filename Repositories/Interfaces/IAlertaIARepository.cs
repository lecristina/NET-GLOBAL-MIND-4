using challenge_3_net.Models;

namespace challenge_3_net.Repositories.Interfaces
{
    public interface IAlertaIARepository : IRepository<AlertaIA>
    {
        Task<(IEnumerable<AlertaIA> Items, long TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<AlertaIA>> GetByUsuarioAsync(int usuarioId);
    }
}

