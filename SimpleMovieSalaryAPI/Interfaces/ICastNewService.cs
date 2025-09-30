using SimpleMovieSalaryAPI.Models;

namespace SimpleMovieSalaryAPI.Interfaces
{
    public interface ICastNewService
    {
        Task<IEnumerable<CastMember>> GetAllAsync();
        Task<CastMember?> GetByIdAsync(int id);
        Task<IEnumerable<CastMember>> SearchByParamsAsync(int? id, string? name, decimal? remuneration);
        Task<CastMember> CreateAsync(CastMember castMember);
        Task<bool> UpdateAsync(int id, CastMember castMember);
        Task<bool> DeleteAsync(int id);
    }
}
