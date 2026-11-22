namespace ATMProject.Application.Services.Interfaces;
using ATMProject.Domain.Models;
public interface IAccountService
{
    Account GetAccountByClientId(int clientId);
    List<Account> GetAllAccounts();

    void Deposit(int clientId, decimal amount);

    void Withdraw(int clientId, decimal amount);

    void Transfer(int senderId, int receiverId, decimal amount);

    void ChangeAccountInformation(int clientId);
    void BlockAccount(int clientId);

    void UnblockAccount(int clientId);
}