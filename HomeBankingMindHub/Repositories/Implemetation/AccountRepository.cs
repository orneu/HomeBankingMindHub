using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.Entities;
using HomeBankingMindHub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HomeBankingMindHub.Repositories.Implemetation
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
            public AccountRepository(HomeBankingContext repositoryContext) : base(repositoryContext) { }

            public Account FindById(long id)
            {
          
                return FindByCondition(account => account.Id == id)
                    .Include(account => account.Transactions)
                    .FirstOrDefault();
            }  
        public IEnumerable<Account> FindAllById(long id)
            {

            return FindByCondition(account => account.Id == id)
                .Include(account => account.Id)
                .ToList();

            }

            public IEnumerable<Account> GetAllAccounts()
            {
                return FindAll()
                    .Include(account => account.Transactions)
                    .ToList();
            }

            public void Save(Account account)
            {
                Create(account);
                SaveChanges();
            }


            public IEnumerable<Account> GetAccountsByClient(long clientId)

            {

                return FindByCondition(account => account.ClientId == clientId)

                .Include(account => account.Transactions)

                .ToList();

            }
    }

    }



   
