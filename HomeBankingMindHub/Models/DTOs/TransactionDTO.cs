using HomeBankingMindHub.Models.Entities;

namespace HomeBankingMindHub.Models.DTOs
{
    public class TransactionDTO
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

         public TransactionDTO(Transaction tr) {
          Id = tr.Id;
          Type = tr.Type.ToString();
          Amount = tr.Amount;
          Description = tr.Description;
          Date = tr.Date; }
    }
}
