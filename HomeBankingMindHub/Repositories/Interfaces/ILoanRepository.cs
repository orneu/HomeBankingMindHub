using HomeBankingMindHub.Models.Entities;

namespace HomeBankingMindHub.Repositories.Interfaces
{
    public interface ILoanRepository
    {
        IEnumerable<Loan> GetAllLoans();
        Loan FindById(long id);
        IEnumerable<Loan> FindAllById(long id);

    }
}
