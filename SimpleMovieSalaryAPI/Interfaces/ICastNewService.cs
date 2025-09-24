using SimpleMovieSalaryAPI.Models;

namespace SimpleMovieSalaryAPI.Interfaces
{
    public interface ICastNewService
    {
        Task<IEnumerable<CastMember>> GetAllAsync();
        Task<CastMember?> GetByIdAsync(int id);
        Task<CastMember> CreateAsync(CastMember castMember);
        Task<bool> UpdateAsync(int id, CastMember castMember);
        Task<bool> DeleteAsync(int id);
    }
}
