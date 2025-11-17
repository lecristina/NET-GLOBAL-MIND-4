using Microsoft.EntityFrameworkCore;
using nexus.Data;
using nexus.Models;
using nexus.Repositories.Interfaces;

namespace nexus.Repositories
{
    public class AlertaIARepository : Repository<AlertaIA>, IAlertaIARepository
    {
        public AlertaIARepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<(IEnumerable<AlertaIA> Items, long TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _dbSet.AsQueryable();
            var totalCount = await query.LongCountAsync();
            
            var items = await query
                .OrderByDescending(a => a.DataAlerta)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<IEnumerable<AlertaIA>> GetByUsuarioAsync(int usuarioId)
        {
            return await _dbSet
                .Where(a => a.IdUsuario == usuarioId)
                .OrderByDescending(a => a.DataAlerta)
                .ToListAsync();
        }
    }
}

