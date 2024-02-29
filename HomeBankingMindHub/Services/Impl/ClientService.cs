using HomeBankingMindHub.Models.DTOs;
using HomeBankingMindHub.Models.Entities;
using HomeBankingMindHub.Models.ENUM;
using HomeBankingMindHub.Repositories.Implemetation;
using HomeBankingMindHub.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System.Web.Http.Controllers;

namespace HomeBankingMindHub.Services.Impl
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        private readonly IAccountRepository _accountRepository;

        private CardColor cardColorAux;

        public ClientService(IClientRepository clientRepository, IAccountRepository accountRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
        }
        public List<ClientDTO> getAllClients()
        {
            var clients = _clientRepository.GetAllClients();
            var clientsDTO = new List<ClientDTO>();

            foreach (var client in clients)
            {
                ClientDTO clientDTO = new ClientDTO(client);
                clientsDTO.Add(clientDTO);
            }
            return clientsDTO;
        }

        public ClientDTO getClientById(long id)
        {
            Client client = _clientRepository.FindById(id);
            if (client == null)
            {
                return null;
            }
            ClientDTO clientDTO = new ClientDTO(client);
            return clientDTO;
        }

        public Client findByEmail(string email)
        {
            Client user = _clientRepository.FindByEmail(email);
            return user;
        }

        public void save(Client client)
        {
            _clientRepository.Save(client);
        }

        public Card createNewCard(Client client, CardFormDTO cardFormDTO) 
            {
            if (cardFormDTO.Color.ToUpper().Equals(CardColor.GOLD.ToString()))
            {
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

            //            Card card = new(client, cardColorAux, cardFormDTO);

            Random rand = new Random();
            string randomCardNumber = "";
            for (int i = 0; i < 4; i++)
            {
                randomCardNumber += (1000 + rand.Next(8999));
                if (i == 3) { break; }

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

            return (card);
            }
        public IEnumerable<Account> getAllAccounts(Client client) 
        {
            IEnumerable<Account> accounts = _accountRepository.GetAccountsByClient(client.Id);
           return accounts;
        }

        public void saveAccount(Account account) 
        {
            this._accountRepository.Save(account);
        }

        public IEnumerable<Account> findAccountById(long id) {
            IEnumerable<Account> accounts = _accountRepository.FindAllById(id);
            return accounts;
        }
    }
}
