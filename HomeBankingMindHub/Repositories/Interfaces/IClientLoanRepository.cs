using HomeBankingMindHub.Models.Entities;

namespace HomeBankingMindHub.Repositories.Interfaces
{
    public interface IClientLoanRepository
    {
        void Save(ClientLoan clientLoan);
    }
}
