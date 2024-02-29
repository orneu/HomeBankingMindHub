using HomeBankingMindHub.Models.Entities;
using HomeBankingMindHub.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using HomeBankingMindHub.Services;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;
        

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_clientService.getAllClients());
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
                ClientDTO clientDTO = _clientService.getClientById(id);
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
                Client user = _clientService.findByEmail(client.Email);

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

                _clientService.save(newClient);
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

                Client client = _clientService.findByEmail(email);

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
        public IActionResult CreateAccount()
        {
            try
            {
                string authUser = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (authUser == string.Empty)
                {
                    return StatusCode(401, "Unauthorized");
                }

                Client client = _clientService.findByEmail(authUser);

                if (client == null)
                {
                    return StatusCode(401, "No existe el cliente");
                }

                if ((_clientService.getAllAccounts(client)).Count() >= 3) {
                    return StatusCode(403, "Limite maximo de cuentas alcanzado");
                }

                Account account = new(client);
                this._clientService.saveAccount(account);
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

                Client client = _clientService.findByEmail(authUser);

                if (client == null)
                {
                    return StatusCode(401, "Unauthorized");
                }
                return Ok(_clientService.findAccountById(client.Id));
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

                Client client = _clientService.findByEmail(authUser);
                if (client == null)
                {
                    return StatusCode(401, "Unauthorized");
                }
                ICollection<Card> cards = client.Cards;
                if (cards.Count >= 6) 
                    return StatusCode(401,"Ya posee el maximo permitido de tarjetas.");

                var cardAlredyExist = cards.FirstOrDefault(c => cardFormDTO.Type.ToUpper().Equals(c.Type.ToString()) && cardFormDTO.Color.ToUpper().Equals(c.Color.ToString()));
                
                if (cardAlredyExist != default)
                    return StatusCode(401,"Usted ya posee una tarjeta con estas caracteristicas.");
                
                return Ok(_clientService.createNewCard(client, cardFormDTO));   
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

                Client client = _clientService.findByEmail(authUser);

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

