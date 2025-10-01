using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
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

        public async Task<byte[]> ExportToExcelAsync()
        {
            var castMembers = await _context.CastMembers.ToListAsync();

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("CastMembers");

            // Add headers
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Name";
            worksheet.Cells[1, 3].Value = "Remuneration";
            worksheet.Cells[1, 4].Value = "Amount Paid";
            worksheet.Cells[1, 5].Value = "Remaining Amount";
            worksheet.Cells[1, 6].Value = "Status";

            // Add data rows
            int row = 2;
            foreach (var cast in castMembers)
            {
                worksheet.Cells[row, 1].Value = cast.Id;
                worksheet.Cells[row, 2].Value = cast.Name;
                worksheet.Cells[row, 3].Value = cast.Remuneration;
                worksheet.Cells[row, 4].Value = cast.AmountPaid;
                worksheet.Cells[row, 5].Value = cast.RemainingAmount;
                worksheet.Cells[row, 6].Value = cast.Status;
                row++;
            }

            worksheet.Cells.AutoFitColumns();

            return package.GetAsByteArray();
        }

}
}
