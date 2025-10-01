using Microsoft.EntityFrameworkCore;
using SimpleMovieSalaryAPI.Data;
using SimpleMovieSalaryAPI.Interfaces;
using SimpleMovieSalaryAPI.Models;

namespace SimpleMovieSalaryAPI.Services
{
    public class CastNewService:ICastNewService
    {
        private readonly AppDbContext _context;
        public CastNewService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CastMember>> GetAllAsync()
        {
            return await _context.CastMembers.ToListAsync();
        }

        public async Task<CastMember?> GetByIdAsync(int id)
        {
            return await _context.CastMembers.FindAsync(id);
        }

        public async Task<CastMember> CreateAsync(CastMember castMember)
        {
            _context.CastMembers.Add(castMember);
            await _context.SaveChangesAsync();
            return castMember;
        }

        public async Task<IEnumerable<CastMember>> SearchByParamsAsync(int? id, string? name, decimal? remuneration)
        {
            var query = _context.CastMembers.AsQueryable();

            if (id.HasValue)
                query = query.Where(c => c.Id == id.Value);

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(c => c.Name.ToLower().Contains(name.ToLower()));
            }

            if (remuneration.HasValue)
                query = query.Where(c => c.Remuneration == remuneration.Value);

            return await query.ToListAsync();
        }


        public async Task<bool> UpdateAsync(int id, CastMember castMember)
        {
            var existing = await _context.CastMembers.FindAsync(id);
            if (existing == null) return false;
            existing.Name = castMember.Name;
            existing.Remuneration = castMember.Remuneration;
            existing.AmountPaid = castMember.AmountPaid;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cast = await _context.CastMembers.FindAsync(id);
            if (cast == null) return false;
            _context.CastMembers.Remove(cast);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
