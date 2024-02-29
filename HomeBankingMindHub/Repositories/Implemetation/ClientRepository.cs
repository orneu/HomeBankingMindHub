using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.Entities;
using HomeBankingMindHub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HomeBankingMindHub.Repositories.Implemetation
{
    public class ClientRepository : RepositoryBase<Client>, IClientRepository
    {
        public ClientRepository(HomeBankingContext repositoryContext) : base(repositoryContext){}


        public Client FindByEmail(string email)
        {
            return FindByCondition(client => client.Email.ToUpper() == email.ToUpper())
            .Include(client => client.Accounts)
            .Include(client => client.ClientLoan)
            .ThenInclude(client => client.Loan)
            .Include(client => client.Cards)
            .FirstOrDefault();
        }
        public Client FindById(long id)
        {
            return FindByCondition(client => client.Id == id)
                .Include(client => client.Accounts)
                .Include(client => client.ClientLoan)
                .ThenInclude(client => client.Loan)
                .Include(client => client.Cards)


                .FirstOrDefault();
        }

        public IEnumerable<Client> GetAllClients()
        {
            return FindAll()
                .Include(client => client.Accounts)
                .Include(client => client.ClientLoan)
                .ThenInclude(client => client.Loan)
                .ToList();
        }

        public void Save(Client client)
        {
            Create(client);
            SaveChanges();
        }    
        public void Change(Client client)
        {
            Update(client);
            SaveChanges();
        }


    }
}