using HomeBankingMindHub.Models.Entities;

namespace HomeBankingMindHub.Services
{
    public interface ITransactionService
    {
        Client findByEmail(string email);
        Account findByNumber(string accountNumber);
        void save(Transaction transaction);
        void save(Account account);
    }
}
