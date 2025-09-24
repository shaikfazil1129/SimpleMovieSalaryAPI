using SimpleMovieSalaryAPI.Models;

namespace SimpleMovieSalaryAPI.Interfaces
{
    public interface ICastMemberService
    {
        Task<IEnumerable<CastMember>> GetAllAsync();
        Task<CastMember?> GetByIdAsync(int id);
        Task<CastMember> CreateAsync(CastMember member);
        Task<bool> UpdateAsync(int id, CastMember updated);
        Task<bool> DeleteAsync(int id);
    }
}
