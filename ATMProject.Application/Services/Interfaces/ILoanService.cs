using ATMProject.Domain.Models;

namespace ATMProject.Application.Services.Interfaces;

public interface ILoanService
{
    void RequestLoan(int clientId, decimal amount);

    List<LoanRequest> GetLoansByClientId(int clientId);
}