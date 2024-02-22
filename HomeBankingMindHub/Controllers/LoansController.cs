using HomeBankingMindHub.Models.Entities;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.ENUM;
using HomeBankingMindHub.Models.DTOs;
using HomeBankingMindHub.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using HomeBankingMindHub.DTOs;
using HomeBankingMindHub.Repositories.Implemetation;
using System.Diagnostics.Eventing.Reader;
using Humanizer;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private IClientRepository _clientRepository;
        private ILoanRepository _loanRepository;
        private IAccountRepository _accountRepository;
        private ITransactionRepository _transactionRepository;
        private IClientLoanRepository _clientLoanRepository;


        public LoansController(IClientRepository clientRepository, ILoanRepository loanRepository, IAccountRepository accountRepository, ITransactionRepository transactionRepository,IClientLoanRepository clientLoanRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _loanRepository = loanRepository;
            _transactionRepository = transactionRepository;
            _clientLoanRepository = clientLoanRepository;

        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                string authUser = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (authUser == string.Empty)
                {
                    return StatusCode(401, "Unauthorized");
                }
                IEnumerable<Loan> loans =  _loanRepository.GetAllLoans();
            return Ok(loans);

            }catch(Exception ex)
            {
                return Forbid(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] LoanApplicationDTO loanApplicationDTO)
        {
            try
            {   //verifica que este autorizado
                string authUser = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (authUser == string.Empty)
                {
                    return StatusCode(401, "Unauthorized");
                }
                Client client = _clientRepository.FindByEmail(authUser);


                //verifica que el monto NO sea menor a 0 
                if (loanApplicationDTO.Amount <= 0)
                {
                    return StatusCode(406, "Ingrese un monto valido");
                }

                //verifica que no exista ese prestamo
                var loan = _loanRepository.FindById(loanApplicationDTO.Id);

                if (loan == null) return StatusCode(403, "No existe ese prestamo"); 

                //Verifica que no sobrepase el máximo autorizado.
                if (loanApplicationDTO.Amount > loan.MaxAmount) {
                    return StatusCode(403, "Ingrese un monto que no supere el maximo autorizado");
                }


                // valida que exista la cuenta destino
                var account = _accountRepository.FindByNumber(loanApplicationDTO.ToAccountNumber);
                if (account == null)
                {return StatusCode(403, "No existe la cuenta destino");
                }

                // Valida que las cuotas sean las indicadas

                if (loanApplicationDTO.Payments == "0") {
                    return StatusCode(406, "Seleccione un monto de pagos correcto");
                }

                

                _clientLoanRepository.Save(new ClientLoan
                {
                    Id = client.ClientLoan.Count()+1,
                    LoanId = loanApplicationDTO.Id,
                    Amount = loanApplicationDTO.Amount * 1.20,
                    Payments = loanApplicationDTO.Payments,
                }
                    );

                _transactionRepository.Save(new Transaction
                {
                    Type = TransactionType.CREDIT,
                    Amount = loanApplicationDTO.Amount,
                    Description = loan.Name.ToString() + " Loan approved",
                    AccountId = account.Id,
                    Date = DateTime.Now,
                });

                account.Balance += loanApplicationDTO.Amount;

                _accountRepository.Save(account);

                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
