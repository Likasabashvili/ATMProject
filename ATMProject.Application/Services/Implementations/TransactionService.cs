using ATMProject.Application.Services.Interfaces;
using ATMProject.Application.Services.Implementations;
using ATMProject.Domain.Interfaces;
using ATMProject.Domain.Models;

namespace ATMProject.Application.Services.Implementations;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }
    
    public void AddTransaction(int accountId, string type, decimal amount, string description)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero!");

        var transactions = _transactionRepository.GetAllTransactions();

        int newId = transactions.Count == 0
            ? 1
            : transactions.Max(x => x.Id) + 1;

        Transaction transaction = new Transaction
        {
            Id = newId,
            AccountId = accountId,
            Type = type,
            Amount = amount,
            CreatedAt = DateTime.Now,
            Description = description
        };

        _transactionRepository.SaveTransaction(transaction);
    }
    
    public List<Transaction> GetTransactionsByAccountId(int accountId)
    {
        return _transactionRepository.GetAllTransactions()
            .Where(x => x.AccountId == accountId)
            .OrderByDescending(x => x.CreatedAt)
            .ToList();
    }
    
    public List<Transaction> GetAllTransactions()
    {
        return _transactionRepository.GetAllTransactions();
    }
}