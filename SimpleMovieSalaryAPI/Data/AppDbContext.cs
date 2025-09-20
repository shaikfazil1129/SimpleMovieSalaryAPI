using Microsoft.EntityFrameworkCore;
using SimpleMovieSalaryAPI.Models;

namespace SimpleMovieSalaryAPI.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<CastMember> CastMembers { get; set; }
    }
}
