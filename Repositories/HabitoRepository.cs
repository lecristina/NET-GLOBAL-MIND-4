using Microsoft.EntityFrameworkCore;
using nexus.Data;
using nexus.Models;
using nexus.Repositories.Interfaces;

namespace nexus.Repositories
{
    public class HabitoRepository : Repository<Habito>, IHabitoRepository
    {
        public HabitoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<(IEnumerable<Habito> Items, long TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _dbSet.AsQueryable();
            var totalCount = await query.LongCountAsync();
            
            var items = await query
                .OrderByDescending(h => h.DataHabito)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<IEnumerable<Habito>> GetByUsuarioAsync(int usuarioId)
        {
            return await _dbSet
                .Where(h => h.IdUsuario == usuarioId)
                .OrderByDescending(h => h.DataHabito)
                .ToListAsync();
        }
    }
}

