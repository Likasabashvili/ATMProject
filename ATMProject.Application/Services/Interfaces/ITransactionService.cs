using ATMProject.Domain.Models;

namespace ATMProject.Application.Services.Interfaces;

public interface ITransactionService
{
    List<Transaction> GetTransactionsByClientId(int clientId);
}