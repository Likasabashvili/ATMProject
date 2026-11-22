using ATMProject.Domain.Models;

namespace ATMProject.Domain.Interfaces;

public interface IAccountRepository
{
    List<Account> GetAllAccounts();

    Account GetAccountByClientId(int clientId);

    void SaveAccount(Account account);

    void UpdateAccount(Account account);
}