using Microsoft.EntityFrameworkCore;
using nexus.Data;
using nexus.Models;
using nexus.Repositories.Interfaces;

namespace nexus.Repositories
{
    public class BadgeRepository : Repository<Badge>, IBadgeRepository
    {
        public BadgeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<(IEnumerable<Badge> Items, long TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _dbSet.AsQueryable();
            var totalCount = await query.LongCountAsync();
            
            var items = await query
                .OrderBy(b => b.PontosRequeridos)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<IEnumerable<UsuarioBadge>> GetByUsuarioAsync(int usuarioId)
        {
            return await _context.Set<UsuarioBadge>()
                .Include(ub => ub.Badge)
                .Where(ub => ub.IdUsuario == usuarioId)
                .OrderByDescending(ub => ub.DataConquista)
                .ToListAsync();
        }

        public async Task<UsuarioBadge?> GetUsuarioBadgeAsync(int usuarioId, int badgeId)
        {
            return await _context.Set<UsuarioBadge>()
                .Include(ub => ub.Badge)
                .FirstOrDefaultAsync(ub => ub.IdUsuario == usuarioId && ub.IdBadge == badgeId);
        }

        public async Task<UsuarioBadge> AddUsuarioBadgeAsync(UsuarioBadge usuarioBadge)
        {
            await _context.Set<UsuarioBadge>().AddAsync(usuarioBadge);
            await _context.SaveChangesAsync();
            return usuarioBadge;
        }
    }
}

