using Microsoft.EntityFrameworkCore;
using SimpleMovieSalaryAPI.Data;
using SimpleMovieSalaryAPI.Interfaces;
using SimpleMovieSalaryAPI.Models;

namespace SimpleMovieSalaryAPI.Services
{
    public class CastMemberService:ICastMemberService
    {
        private readonly AppDbContext _context;
        public CastMemberService(AppDbContext context)
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
        public async Task<CastMember> CreateAsync(CastMember member)
        {
            _context.CastMembers.Add(member);
            await _context.SaveChangesAsync();
            return member;
        }
        public async Task<bool> UpdateAsync(int id, CastMember updated)
        {
            var existing = await _context.CastMembers.FindAsync(id);
            if (existing == null) return false;
            existing.Name = updated.Name;
            existing.Remuneration = updated.Remuneration;
            existing.AmountPaid = updated.AmountPaid;
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
