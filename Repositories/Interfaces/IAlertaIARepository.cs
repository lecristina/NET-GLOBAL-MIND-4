using nexus.Models;

namespace nexus.Repositories.Interfaces
{
    public interface IAlertaIARepository : IRepository<AlertaIA>
    {
        Task<(IEnumerable<AlertaIA> Items, long TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<AlertaIA>> GetByUsuarioAsync(int usuarioId);
    }
}

