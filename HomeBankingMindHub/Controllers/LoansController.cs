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
using HomeBankingMindHub.Services;
using HomeBankingMindHub.Services.Impl;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;


        public LoansController(ILoanService loanService)
        {
            _loanService = loanService;

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
                return Ok(_loanService.findAll());

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

                Client client = _loanService.findByEmail(authUser);

                //verifica que el monto NO sea menor a 0 
                if (loanApplicationDTO.Amount <= 0)
                {
                    return StatusCode(406, "Ingrese un monto valido");
                }

                //verifica que no exista ese prestamo
                Loan loan = _loanService.findById(loanApplicationDTO.LoanId);

                if (loan == null)
                {
                    return StatusCode(403, "No existe ese prestamo");
                }

                //Verifica que no sobrepase el máximo autorizado.
                if (loanApplicationDTO.Amount > loan.MaxAmount) {
                    return StatusCode(403, "Ingrese un monto que no supere el maximo autorizado");
                }

                // valida que exista la cuenta destino
                var account = _loanService.findByNumber(loanApplicationDTO.ToAccountNumber);
                if (account == null)
                {return StatusCode(403, "No existe la cuenta destino");
                }

                // Valida que las cuotas sean las indicadas

                if (loanApplicationDTO.Payments == "0") {
                    return StatusCode(406, "Seleccione un monto de pagos correcto");
                }

                _loanService.save(new ClientLoan
                {
                    LoanId = loanApplicationDTO.LoanId,
                    Amount = loanApplicationDTO.Amount * 1.20,
                    Payments = loanApplicationDTO.Payments,
                    ClientId = client.Id
                }
                    ) ;

                _loanService.save(new Transaction
                {
                    Type = TransactionType.CREDIT,
                    Amount = loanApplicationDTO.Amount,
                    Description = loan.Name.ToString() + " Loan approved",
                    AccountId = account.Id,
                    Date = DateTime.Now,
                });

                account.Balance += loanApplicationDTO.Amount;

                _loanService.save(account);

                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
