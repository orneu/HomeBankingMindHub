using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.Entities;
using HomeBankingMindHub.Repositories.Interfaces;
using HomeBankingMindHub.Repositories.Implemetation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HomeBankingMindHub.Repositories.Implemetation
{
    public class LoanRepository : RepositoryBase<Loan>, ILoanRepository
    {
        public LoanRepository(HomeBankingContext repositoryContext) : base(repositoryContext) { }
        public IEnumerable<Loan> GetAllLoans()
        {
            return base.FindAll().ToList();
        }

        public Loan FindById(long id)
        {
            return FindByCondition(loan => loan.Id == id).FirstOrDefault();
        }

        public IEnumerable<Loan> FindAll()
        {
            return base.FindAll().ToList();
        }

    }
}

