using HomeBankingMindHub.Models.DTOs;
using HomeBankingMindHub.Models.Entities;
using HomeBankingMindHub.Repositories.Implemetation;
using HomeBankingMindHub.Repositories.Interfaces;

namespace HomeBankingMindHub.Services.Impl
{
    public class LoanService : ILoanService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IClientLoanRepository _clientLoanRepository;


        public LoanService(IClientRepository clientRepository, IAccountRepository accountRepository, ILoanRepository loanRepository, ITransactionRepository transactionRepository, IClientLoanRepository clientLoanRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _loanRepository = loanRepository;
            _transactionRepository = transactionRepository;
            _clientLoanRepository = clientLoanRepository;
        }
    
        public List<Loan> findAll()
        {
            List<Loan> loans = _loanRepository.FindAll().ToList();
            return loans;
        }

        public Client findByEmail(string authUser)
        { Client client = _clientRepository.FindByEmail(authUser);
            return client;
        }

        public Loan findById(long loanId)
        {
            Loan loan = _loanRepository.FindById(loanId);
            return loan;
        }

        public Account findByNumber(string accountNumber) {
            var account = _accountRepository.FindByNumber(accountNumber);
            return account;
        }


        public void save (ClientLoan clientLoan)
        {
            _clientLoanRepository.Save(clientLoan);
        }

        public void save(Transaction transaction)
        {
            _transactionRepository.Save(transaction);
        }
        public void save(Account account) {
            _accountRepository.Save(account);
        }
    }
}
