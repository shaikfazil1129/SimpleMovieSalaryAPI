using System.ComponentModel.DataAnnotations;

namespace SimpleMovieSalaryWebApp.Models
{
    public class CastMemberViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Remuneration must be non-negative")]
        public decimal Remuneration { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Amount Paid must be non-negative")]
        public decimal AmountPaid { get; set; }
        public decimal RemainingAmount { get; set; }
        public string ?Status { get; set; }
    }
}
