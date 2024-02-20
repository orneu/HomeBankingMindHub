using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOs;
using HomeBankingMindHub.Models.Entities;
using HomeBankingMindHub.Models.ENUM;
using HomeBankingMindHub.Repositories;
using HomeBankingMindHub.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Principal;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private ITransactionRepository _transactionRepository;

        public TransactionsController(IClientRepository clientRepository, IAccountRepository accountRepository, ITransactionRepository transactionRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }

        [HttpPost]
        public IActionResult Post([FromBody] TransferDTO transferDTO)
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return StatusCode(403, "Email vacío");
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return StatusCode(403, "No existe el cliente");
                }

                //agregamos validaciones para las transferencias

                if (transferDTO.FromAccountNumber == transferDTO.ToAccountNumber)
                {
                    return StatusCode(403, "No se permite la transferencia a la misma cuenta.");
                }

                if (transferDTO.Amount <= 0 )
                {
                    return StatusCode(403, "Ingresar un monto mayor a 0");
                }

                if (transferDTO.Description == string.Empty)
                {
                    return StatusCode(403, "Descripcion no proporcionada");
                }


                if (transferDTO.FromAccountNumber.IsNullOrEmpty())
                {
                    return StatusCode(403, "Cuenta origen no proporcionada");
                }
                if (transferDTO.ToAccountNumber.IsNullOrEmpty()) 
                {
                    return StatusCode(403, "Cuenta destino no proporcionada");
                }

                //buscamos las cuentas
                Account fromAccount = _accountRepository.FinByNumber(transferDTO.FromAccountNumber);
                if (fromAccount == null)
                {
                    return StatusCode(403, "Cuenta de origen no existe");
                }

                //controlamos el monto
                if (fromAccount.Balance < transferDTO.Amount)
                {
                    return StatusCode(403, "Fondos insuficientes");
                }

                //buscamos la cuenta de destino
                Account toAccount = _accountRepository.FinByNumber(transferDTO.ToAccountNumber);
                if (toAccount == null)
                {
                    return StatusCode(403, "Cuenta de destino no existe");
                }

               
                _transactionRepository.Save(new Transaction
                {
                    Type = TransactionType.DEBIT,
                    Amount = transferDTO.Amount * -1,
                    Description = transferDTO.Description + " " + toAccount.Number,
                    AccountId = fromAccount.Id,
                    Date = DateTime.Now,
                });

                
                _transactionRepository.Save(new Transaction
                {
                    Type = TransactionType.CREDIT,
                    Amount = transferDTO.Amount,
                    Description = transferDTO.Description + " " + fromAccount.Number,
                    AccountId = toAccount.Id,
                    Date = DateTime.Now,
                });

                //seteamos los valores de las cuentas, a la ccuenta de origen le restamos el monto
                fromAccount.Balance = fromAccount.Balance - transferDTO.Amount;
                
                //actualizamos la cuenta de origen
                _accountRepository.Save(fromAccount);

                //a la cuenta de destino le sumamos el monto
                toAccount.Balance = toAccount.Balance + transferDTO.Amount;
                
                //actualizamos la cuenta de destino
                _accountRepository.Save(toAccount);


                return Created("Creado con exito", fromAccount);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }

        }
    }
}