using Microsoft.EntityFrameworkCore;
using challenge_3_net.Data;
using challenge_3_net.Models;
using challenge_3_net.Repositories.Interfaces;

namespace challenge_3_net.Repositories
{
    public class HumorRepository : Repository<Humor>, IHumorRepository
    {
        public HumorRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<(IEnumerable<Humor> Items, long TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _dbSet.AsQueryable();
            var totalCount = await query.LongCountAsync();
            
            var items = await query
                .OrderByDescending(h => h.DataRegistro)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<IEnumerable<Humor>> GetByUsuarioAsync(int usuarioId)
        {
            return await _dbSet
                .Where(h => h.IdUsuario == usuarioId)
                .OrderByDescending(h => h.DataRegistro)
                .ToListAsync();
        }
    }
}

