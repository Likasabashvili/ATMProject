using ATMProject.Domain.Models;

namespace ATMProject.Application.Services.Interfaces;

public interface ITransactionService
{
    void AddTransaction(int accountId, string type, decimal amount, string description);
    List<Transaction> GetTransactionsByAccountId(int accountId);
    List<Transaction> GetAllTransactions();
}