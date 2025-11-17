using Microsoft.EntityFrameworkCore;
using nexus.Data;
using nexus.Models;
using nexus.Repositories.Interfaces;

namespace nexus.Repositories
{
    public class SprintRepository : Repository<Sprint>, ISprintRepository
    {
        public SprintRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<(IEnumerable<Sprint> Items, long TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _dbSet.AsQueryable();
            var totalCount = await query.LongCountAsync();
            
            var items = await query
                .OrderByDescending(s => s.DataInicio)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<IEnumerable<Sprint>> GetByUsuarioAsync(int usuarioId)
        {
            return await _dbSet
                .Where(s => s.IdUsuario == usuarioId)
                .OrderByDescending(s => s.DataInicio)
                .ToListAsync();
        }

        public async Task<Sprint?> GetByUsuarioAndNomeAsync(int usuarioId, string nomeSprint)
        {
            return await _dbSet
                .FirstOrDefaultAsync(s => s.IdUsuario == usuarioId && s.NomeSprint == nomeSprint);
        }
    }
}

