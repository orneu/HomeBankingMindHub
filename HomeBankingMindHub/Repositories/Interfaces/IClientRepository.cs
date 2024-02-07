using HomeBankingMindHub.Models.Classes;

namespace HomeBankingMindHub.Repositories.Interfaces
{
    public interface IClientRepository
    {
        IEnumerable<Client> GetAllClients();
        void Save(Client client);
        Client FindById(long id);
    }
}
