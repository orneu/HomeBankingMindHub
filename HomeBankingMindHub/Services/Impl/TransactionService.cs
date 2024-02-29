using HomeBankingMindHub.Models.DTOs;
using HomeBankingMindHub.Models.Entities;
using HomeBankingMindHub.Repositories.Interfaces;

namespace HomeBankingMindHub.Services.Impl
{
    public class TransactionService : ITransactionService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(IClientRepository clientRepository, IAccountRepository accountRepository, ITransactionRepository transactionRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }

        public Client findByEmail(string email) {
            Client client = _clientRepository.FindByEmail(email);
            return client;
        }

        public Account findByNumber(string accountNumber){
           Account fromAccount = _accountRepository.FindByNumber(accountNumber);
            return fromAccount;
            }

        public void save(Transaction transaction) { 
        _transactionRepository.Save(transaction);
        }
        public void save(Account account)
        {
            _accountRepository.Save(account);

        }

    }
}
