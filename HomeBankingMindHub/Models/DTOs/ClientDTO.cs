using HomeBankingMindHub.DTOs;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.Entities;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HomeBankingMindHub.Models.DTOs
{
    public class ClientDTO
    {   public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<AccountDTO> Accounts { get; set; }
        public ICollection<ClientLoanDTO> Credits { get; set; }
        public ICollection<CardDTO> Cards { get; set; }

        public ClientDTO(Client client)
        {
            Id = client.Id;
            FirstName = client.FirstName;
            LastName = client.LastName;
            Email = client.Email;
            Accounts = client.Accounts.Select(ac => new AccountDTO(ac)).ToList();
            Credits = client.ClientLoan.Select(cl => new ClientLoanDTO(cl)).ToList();
            Cards = client.Cards.Select(c => new CardDTO(c)).ToList();
        }
    }

}