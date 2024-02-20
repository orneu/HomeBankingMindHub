using HomeBankingMindHub.Models.Entities;

namespace HomeBankingMindHub.Repositories.Interfaces

{
    public interface ITransactionRepository
    {
        void Save(Transaction transaction);
        Transaction FindByNumber(long id);
    }
}
