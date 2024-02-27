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
using HomeBankingMindHub.Services;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private IAccountRepository _accountRepository;
        private readonly IClientService clientService;
        private CardColor cardColorAux;

        public ClientsController(IClientService clientService, IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
         }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(clientService.getAllClients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            if (id <= 0) 
            { 
            return BadRequest("Invalid id");
            }
            try
            {
                ClientDTO clientDTO = clientService.getClientById(id);
                if (clientDTO == null)
                {
                    return NotFound();
                }
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
                Client user = clientService.findByEmail(client.Email);

                if (user != null)
                {
                    return StatusCode(403, "Email está en uso");
                }

                PasswordHasher<Client> passwordHasher = new PasswordHasher<Client>();
                string hashedPassword = passwordHasher.HashPassword(user, client.Password);
                Client newClient = new Client();

                clientService.save(newClient);
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

                Client client = clientService.findByEmail(email);

                if (client == null)
                {
                    return StatusCode(401, "Unauthorized");
                }

                var clientDTO = new ClientDTO(client);
                

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

                Client client = clientService.findByEmail(authUser);

                if (client == null)
                {
                    return StatusCode(401, "Unauthorized");
                }

                IEnumerable<Account> accounts = _accountRepository.GetAccountsByClient(client.Id);
                if(accounts.Count() >= 3) {
                    return Forbid();
                }

                Account account = new(client);
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

                Client client = clientService.findByEmail(authUser);

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
        public IActionResult CreateNewCard([FromBody] CardFormDTO cardFormDTO)
        {
            try
            {
                string authUser = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (authUser == string.Empty)
                {
                    return StatusCode(401, "Unauthorized");
                }

                Client client = clientService.findByEmail(authUser);
                if (client == null)
                {
                    return StatusCode(401, "Unauthorized");
                }
                ICollection<Card> cards = client.Cards;
                if (cards.Count >= 6) return Forbid("Ya posee el maximo permitido de tarjetas.");

                var cardAlredyExist = cards.FirstOrDefault(c => cardFormDTO.Type.ToUpper().Equals(c.Type.ToString()) && cardFormDTO.Color.ToUpper().Equals(c.Color.ToString()));
                
                if (cardAlredyExist != default) return Forbid("Usted ya posee una tarjeta con estas caracteristicas.");
                
                return Ok(clientService.createNewCard(client, cardFormDTO, cardColorAux));   
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

                Client client = clientService.findByEmail(authUser);

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

