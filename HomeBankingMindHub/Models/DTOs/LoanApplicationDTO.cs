using HomeBankingMindHub.Models.Entities;

namespace HomeBankingMindHub.Models.DTOs
{
    public class LoanApplicationDTO
    {
        public long Id { get; set; }
        public double Amount { get; set; }
        public string Payments { get; set; }
        public string ToAccountNumber { get; set; }

    }
}
