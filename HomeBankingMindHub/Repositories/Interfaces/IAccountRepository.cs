using HomeBankingMindHub.Models.Classes;

namespace HomeBankingMindHub.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAllAccounts();
        void Save(Account account);
        Account FindById(long id);
    }
}
