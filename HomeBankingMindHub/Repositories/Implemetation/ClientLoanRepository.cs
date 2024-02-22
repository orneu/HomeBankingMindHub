using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.Entities;
using HomeBankingMindHub.Repositories.Interfaces;
using HomeBankingMindHub.Repositories.Implemetation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HomeBankingMindHub.Repositories.Implemetation
{
    public class ClientLoanRepository : RepositoryBase<ClientLoan>, IClientLoanRepository
    {
        public ClientLoanRepository(HomeBankingContext repositoryContext) : base(repositoryContext) { }
        public void Save(ClientLoan clientLoan)
        {
            Create(clientLoan);
            SaveChanges();
        }
    }
}
