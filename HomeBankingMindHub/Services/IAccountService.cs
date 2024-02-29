using HomeBankingMindHub.Models.DTOs;

namespace HomeBankingMindHub.Services
{
    public interface IAccountService
    {
        List<AccountDTO> getAllAccounts();
        AccountDTO findById(long Id);
    }
}
