
using ATMProject.Domain.Interfaces;
using ATMProject.Domain.Models;

namespace ATMProject.Infrastructure;

public class AccountRepository : IAccountRepository
{
    private readonly string _path =
        "/Users/likasabashvili/Desktop/c#/ATMProject/ATMProject.Infrastructure/Data/Account.txt";

    public AccountRepository()
    {
        string? directory = Path.GetDirectoryName(_path);

        if (directory != null)
            Directory.CreateDirectory(directory);

        if (!File.Exists(_path))
            File.Create(_path).Close();
    }
    
    public List<Account> GetAllAccounts()
    {
        List<Account> accounts = new();

        var lines = File.ReadAllLines(_path);

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var parts = line.Split('|');

            Account account = new Account
            {
                Id = int.Parse(parts[0]),
                ClientId = int.Parse(parts[1]),
                UserName = parts[2],
                Balance = decimal.Parse(parts[3]),
                LoanStatus = bool.Parse(parts[4]),
                LoanAmount = decimal.Parse(parts[5]),
                IsBlocked = bool.Parse(parts[6]),
            };

            accounts.Add(account);
        }

        return accounts;
    }
    
    public Account GetAccountByClientId(int clientId)
    {
        return GetAllAccounts()
            .FirstOrDefault(x => x.ClientId == clientId);
    }
    
    public void SaveAccount(Account account)
    {
        File.AppendAllText(_path, account + Environment.NewLine);
    }
    
    public void UpdateAccount(Account account)
    {
        var accounts = GetAllAccounts();

        var existingAccount = accounts.FirstOrDefault(x => x.Id == account.Id);

        if (existingAccount == null)
            throw new ArgumentException("Account not found!");

        existingAccount.ClientId = account.ClientId;
        existingAccount.UserName = account.UserName;
        existingAccount.Balance = account.Balance;
        existingAccount.LoanStatus = account.LoanStatus;
        existingAccount.LoanAmount = account.LoanAmount;
        existingAccount.IsBlocked = account.IsBlocked;

        File.WriteAllLines(_path, accounts.Select(x => x.ToString()));
    }
}
