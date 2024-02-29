using HomeBankingMindHub.Models.DTOs;
using HomeBankingMindHub.Models.Entities;
using HomeBankingMindHub.Repositories.Implemetation;
using HomeBankingMindHub.Repositories.Interfaces;

namespace HomeBankingMindHub.Services.Impl
{
    public class AccountService : IAccountService
    {
            private readonly IAccountRepository _accountRepository;
            public AccountService(IAccountRepository accountRepository)
            {
                _accountRepository = accountRepository;
            }
        public List<AccountDTO> getAllAccounts() 
        {
            var accounts = _accountRepository.GetAllAccounts();
            var accountsDTO = new List<AccountDTO>();

            foreach (Account account in accounts)
            {
                var newAccountDTO = new AccountDTO(account);
                accountsDTO.Add(newAccountDTO);
            }
            return accountsDTO;
        }

        public AccountDTO findById(long Id) {
            var account = _accountRepository.FindById(Id);
            var accountDTO = new AccountDTO(account);
            return accountDTO;
        }
    }
}
