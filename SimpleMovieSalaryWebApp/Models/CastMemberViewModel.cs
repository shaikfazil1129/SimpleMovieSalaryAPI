namespace SimpleMovieSalaryWebApp.Models
{
    public class CastMemberViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Remuneration { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal RemainingAmount { get; set; }
        public string Status { get; set; }
    }
}
