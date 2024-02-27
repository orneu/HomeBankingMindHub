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

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
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

        public Card createNewCard(Client client, CardFormDTO cardFormDTO, CardColor cardColorAux) 
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

            Card card = new(client, cardColorAux, cardFormDTO);
            client.Cards.Add(card);

            _clientRepository.Change(client);

            return (card);
        }

        
    }
}
