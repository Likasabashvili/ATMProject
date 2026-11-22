
using ATMProject.Application.Services.Interfaces;
using ATMProject.Domain.Interfaces;
using ATMProject.Domain.Models;

namespace ATMProject.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    
    public Account GetAccountByClientId(int clientId)
    {
        var account = _accountRepository.GetAccountByClientId(clientId);

        if (account == null)
            throw new ArgumentException("Account not found!");

        return account;
    }
    
    public List<Account> GetAllAccounts()
    {
        return _accountRepository.GetAllAccounts();
    }

    
    public void Deposit(int clientId, decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero!");

        var account = GetAccountByClientId(clientId);

        CheckIfBlocked(account);

        account.Balance += amount;

        _accountRepository.UpdateAccount(account);
    }
    
    public void Withdraw(int clientId, decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero!");

        var account = GetAccountByClientId(clientId);

        CheckIfBlocked(account);

        if (account.Balance < amount)
            throw new InvalidOperationException("Insufficient balance!");

        account.Balance -= amount;

        _accountRepository.UpdateAccount(account);
    }
    public void Transfer(int senderId, int receiverId, decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero!");

        if (senderId == receiverId)
            throw new ArgumentException("You cannot transfer money to yourself!");

        var senderAccount = GetAccountByClientId(senderId);
        var receiverAccount = GetAccountByClientId(receiverId);

        CheckIfBlocked(senderAccount);
        CheckIfBlocked(receiverAccount);

        if (senderAccount.Balance < amount)
            throw new InvalidOperationException("Insufficient balance!");

        senderAccount.Balance -= amount;
        receiverAccount.Balance += amount;

        _accountRepository.UpdateAccount(senderAccount);
        _accountRepository.UpdateAccount(receiverAccount);
    }
    
    public void ChangeAccountInformation(int clientId)
    {
        var account = GetAccountByClientId(clientId);

        Console.Write("Enter new username: ");
        string newUsername = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(newUsername))
            throw new ArgumentException("Username cannot be empty!");

        account.UserName = newUsername;

        _accountRepository.UpdateAccount(account);
    }
    
    public void BlockAccount(int clientId)
    {
        var account = GetAccountByClientId(clientId);

        if (account.IsBlocked)
            throw new InvalidOperationException("Account is already blocked!");

        account.IsBlocked = true;

        _accountRepository.UpdateAccount(account);
    }

    public void UnblockAccount(int clientId)
    {
        var account = GetAccountByClientId(clientId);

        if (!account.IsBlocked)
            throw new InvalidOperationException("Account is not blocked!");

        account.IsBlocked = false;

        _accountRepository.UpdateAccount(account);
    }
    private void CheckIfBlocked(Account account)
    {
        if (account.IsBlocked)
            throw new InvalidOperationException("This account is blocked!");
    }
}
