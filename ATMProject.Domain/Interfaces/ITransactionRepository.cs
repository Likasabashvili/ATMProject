using ATMProject.Domain.Models;

namespace ATMProject.Domain.Interfaces;

public interface ITransactionRepository
{
    List<Transaction> GetAllTransactions();

    void SaveTransaction(Transaction transaction);
}