using HomeBankingMindHub.DTOs;
using HomeBankingMindHub.Models;
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
    }

}