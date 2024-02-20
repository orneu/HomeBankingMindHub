using HomeBankingMindHub.Models.Entities;

namespace HomeBankingMindHub.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAllAccounts();
        Account FindById(long id);
        IEnumerable<Account> FindAllById(long id);
        void Save(Account account);
        IEnumerable<Account> GetAccountsByClient(long clientId);
        Account FinByNumber(string number);

    }
}
