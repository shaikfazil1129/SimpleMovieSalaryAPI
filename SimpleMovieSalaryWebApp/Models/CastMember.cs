namespace SimpleMovieSalaryWebApp.Models
{
    public class CastMember
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Remuneration { get; set; }
        public decimal AmountPaid { get; set; }

        public decimal RemainingAmount => Remuneration - AmountPaid;

        public string Status
        {
            get
            {
                if (AmountPaid == 0) return "Unpaid";
                if (AmountPaid < Remuneration) return "Partially Paid";
                return "Paid";
            }
        }
    }

}
