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

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private CardColor cardColorAux;

        public ClientsController(IClientRepository clientRepository, IAccountRepository accountRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var clients = _clientRepository.GetAllClients();
                var clientsDTO = new List<ClientDTO>();

                foreach (Client client in clients)
                {
                    var newClientDTO = new ClientDTO

                    {
                        Id = client.Id,
                        Email = client.Email,
                        FirstName = client.FirstName,
                        LastName = client.LastName,
                        Accounts = client.Accounts.Select(ac => new AccountDTO

                        {
                            Id = ac.Id,
                            Balance = ac.Balance,
                            CreationDate = ac.CreationDate,
                            Number = ac.Number

                        }).ToList(),

                        Credits = client.ClientLoan.Select(cl => new ClientLoanDTO
                        {
                            Id = cl.Id,
                            LoanId = cl.LoanId,
                            Name = cl.Loan.Name,
                            Amount = cl.Amount,
                            Payments = int.Parse(cl.Payments)
                        }).ToList(),

                        Cards = client.Cards.Select(c => new CardDTO
                        {
                            Id = c.Id,
                            CardHolder = c.CardHolder,
                            Color = c.Color.ToString(),
                            Cvv = c.Cvv,
                            FromDate = c.FromDate,
                            Number = c.Number,
                            ThruDate = c.ThruDate,
                            Type = c.Type.ToString()
                        }).ToList()

                    };
                    clientsDTO.Add(newClientDTO);
                }
                return Ok(clientsDTO);

            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);

            }

        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            try
            {
                var client = _clientRepository.FindById(id);

                if (client == null)

                {
                    return Forbid();
                }
                var clientDTO = new ClientDTO

                {
                    Id = client.Id,
                    Email = client.Email,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Accounts = client.Accounts.Select(ac => new AccountDTO
                    {
                        Id = ac.Id,
                        Balance = ac.Balance,
                        CreationDate = ac.CreationDate,
                        Number = ac.Number
                    }).ToList(),

                    Credits = client.ClientLoan.Select(cl => new ClientLoanDTO
                    {
                        Id = cl.Id,
                        LoanId = cl.LoanId,
                        Name = cl.Loan.Name,
                        Amount = cl.Amount,
                        Payments = int.Parse(cl.Payments)
                    }).ToList(),

                    Cards = client.Cards.Select(c => new CardDTO
                    {
                        Id = c.Id,
                        CardHolder = c.CardHolder,
                        Color = c.Color.ToString(),
                        Cvv = c.Cvv,
                        FromDate = c.FromDate,
                        Number = c.Number,
                        ThruDate = c.ThruDate,
                        Type = c.Type.ToString()
                    }).ToList()

                };
                return Ok(clientDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Client client)
        {
            try
            {
                //validamos datos antes
                if (String.IsNullOrEmpty(client.Email) || String.IsNullOrEmpty(client.Password) || String.IsNullOrEmpty(client.FirstName) || String.IsNullOrEmpty(client.LastName))
                    return StatusCode(403, "datos inválidos");

                //buscamos si ya existe el usuario
                Client user = _clientRepository.FindByEmail(client.Email);

                if (user != null)
                {
                    return StatusCode(403, "Email está en uso");
                }
                PasswordHasher<Client> passwordHasher = new PasswordHasher<Client>();
                string hashedPassword = passwordHasher.HashPassword(user, client.Password);
                Client newClient = new Client
                {
                    Email = client.Email,
                    Password = hashedPassword,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                };

                _clientRepository.Save(newClient);
                return Created("", newClient);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("current")]
        public IActionResult GetCurrent()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return StatusCode(401, "Unauthorized");
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return StatusCode(401, "Unauthorized");
                }

                var clientDTO = new ClientDTO
                {
                    Id = client.Id,
                    Email = client.Email,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Accounts = client.Accounts.Select(ac => new AccountDTO
                    {
                        Id = ac.Id,
                        Balance = ac.Balance,
                        CreationDate = ac.CreationDate,
                        Number = ac.Number
                    }).ToList(),
                    Credits = client.ClientLoan.Select(cl => new ClientLoanDTO
                    {
                        Id = cl.Id,
                        LoanId = cl.LoanId,
                        Name = cl.Loan.Name,
                        Amount = cl.Amount,
                        Payments = int.Parse(cl.Payments)
                    }).ToList(),
                    Cards = client.Cards.Select(c => new CardDTO
                    {
                        Id = c.Id,
                        CardHolder = c.CardHolder,
                        Type = c.Type.ToString(),
                        Color = c.Color.ToString(),
                        Cvv = c.Cvv,
                        FromDate = c.FromDate,
                        Number = c.Number,
                        ThruDate = c.ThruDate

                    }).ToList()
                };

                return Ok(clientDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("current/accounts")]
        public IActionResult PostAccounts()
        {
            try
            {
                string authUser = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (authUser == string.Empty)
                {
                    return StatusCode(401, "Unauthorized");
                }

                Client client = _clientRepository.FindByEmail(authUser);

                if (client == null)
                {
                    return StatusCode(401, "Unauthorized");
                }

                Random rand = new Random();
                int[] randomAccountNumber = new int[8];
                for (int i = 0; i < 8; i++)
                {
                    randomAccountNumber[i] = rand.Next(10);
                }

                IEnumerable<Account> accounts = _accountRepository.GetAccountsByClient(client.Id);
                if(accounts.Count() >= 3) {
                    return Forbid();
                }

                Account account = new()
                {
                    Number = "VIN" + string.Join("",randomAccountNumber),
                    CreationDate = DateTime.Now,
                    ClientId = client.Id,
                    Balance = 0
                };
                this._accountRepository.Save(account);
                return Created("Nueva cuenta creada",account);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet("current/accounts")]
        public IActionResult GetAccounts()
        {
            try
            {
                string authUser = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (authUser == string.Empty)
                {
                    return StatusCode(401, "Unauthorized");
                }

                Client client = _clientRepository.FindByEmail(authUser);

                if (client == null)
                {
                    return StatusCode(401, "Unauthorized");
                }
                IEnumerable<Account> accounts = _accountRepository.FindAllById(client.Id);

                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpPost("current/cards")]
        public IActionResult Post([FromBody] CardFormDTO cardFormDTO)
        {
            try
            {
                string authUser = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (authUser == string.Empty)
                {
                    return StatusCode(401, "Unauthorized");
                }

                Client client = _clientRepository.FindByEmail(authUser);

                if (client == null)
                {
                    return StatusCode(401, "Unauthorized");
                }
                ICollection<Card> cards = client.Cards;
                if (cards.Count >= 6) return Forbid("Ya posee el maximo permitido de tarjetas.");
                
                var cardAlredyExist = cards.FirstOrDefault(c => cardFormDTO.Type.ToUpper().Equals(c.Type.ToString()) && cardFormDTO.Color.ToUpper().Equals(c.Color.ToString()));
                if (cardAlredyExist != default) return Forbid("Usted ya posee una tarjeta con estas caracteristicas.");

                if (cardFormDTO.Color.ToUpper().Equals(CardColor.GOLD.ToString())) { 
                    cardColorAux = CardColor.GOLD;
                }
                if (cardFormDTO.Color.ToUpper().Equals(CardColor.SILVER.ToString()))
                {
                    cardColorAux = CardColor.SILVER;
                }
                if (cardFormDTO.Color.ToUpper().Equals(CardColor.TITANIUM.ToString()))
                {
                    cardColorAux = CardColor.TITANIUM;
                }

                Random rand = new Random();
                string randomCardNumber = "";
                for (int i = 0; i < 4; i++)
                {
                    randomCardNumber += (1000 + rand.Next(8999));
                    if(i == 3) { break; }

                    randomCardNumber += "-";
                }
                Card card = new()
                {
                    ClientId = client.Id,
                    CardHolder = client.FirstName + " " + client.LastName,
                    Type = cardFormDTO.Type.ToUpper().Equals(CardType.DEBIT.ToString()) ? CardType.DEBIT : CardType.CREDIT,
                    Color = cardColorAux,
                    Number = randomCardNumber,
                    Cvv = 100 + rand.Next(899),
                    FromDate = DateTime.Now,
                    ThruDate = DateTime.Now.AddYears(4),
                };
                client.Cards.Add(card);

                _clientRepository.Change(client);

                return Ok(card);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet("current/cards")]
        public IActionResult GetCards()
        {
            try
            {
                string authUser = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (authUser == string.Empty)
                {
                    return StatusCode(401, "Unauthorized");
                }

                Client client = _clientRepository.FindByEmail(authUser);

                if (client == null)
                {
                    return StatusCode(401, "Unauthorized");
                }
                ICollection<Card> cards = client.Cards;

                return Ok(cards);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}

