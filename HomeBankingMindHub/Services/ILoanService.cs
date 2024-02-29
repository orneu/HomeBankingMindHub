using HomeBankingMindHub.Models.DTOs;
using HomeBankingMindHub.Models.Entities;

namespace HomeBankingMindHub.Services
{
    public interface ILoanService
    {
        List<Loan> findAll();
        Client findByEmail(string authUser);
        Loan findById(long loanId);
        Account findByNumber(string accountNumber);
        void save(ClientLoan clientLoan);
        void save(Transaction transaction);
        void save(Account account);
    }
}
