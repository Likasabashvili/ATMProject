using ATMProject.Domain.Interfaces;
using ATMProject.Domain.Models;

namespace ATMProject.Infrastructure;

public class TransactionRepository : ITransactionRepository
{
    private readonly string _path =
        @"/Users/likasabashvili/Desktop/c#/ATMProject/ATMProject.Infrastructure/Data/Transaction.txt";

    public TransactionRepository()
    {
        string? directory = Path.GetDirectoryName(_path);

        if (directory != null)
            Directory.CreateDirectory(directory);

        if (!File.Exists(_path))
            File.Create(_path).Close();
    }
    
    public List<Transaction> GetAllTransactions()
    {
        List<Transaction> transactions = new();

        var lines = File.ReadAllLines(_path);

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var parts = line.Split('|');

            Transaction transaction = new Transaction
            {
                Id = int.Parse(parts[0]),
                AccountId = int.Parse(parts[1]),
                Type = parts[2],
                Amount = decimal.Parse(parts[3]),
                CreatedAt = DateTime.Parse(parts[4]),
                Description = parts[5]
            };

            transactions.Add(transaction);
        }

        return transactions;
    }
    
    public void SaveTransaction(Transaction transaction)
    {
        File.AppendAllText(
            _path,
            transaction + Environment.NewLine
        );
    }
}