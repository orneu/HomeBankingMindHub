using HomeBankingMindHub.Models.DTOs;
using HomeBankingMindHub.Models.Entities;
using HomeBankingMindHub.Models.ENUM;

namespace HomeBankingMindHub.Services
{
    public interface IClientService
    {
        List<ClientDTO> getAllClients();
        ClientDTO getClientById(long id);
        Client findByEmail(string email);
        void save(Client client);
        Card createNewCard(Client client, CardFormDTO cardFormDTO);
        IEnumerable<Account> getAllAccounts(Client client);
        void saveAccount(Account account);
        IEnumerable<Account> findAccountById(long id);
    }
}
