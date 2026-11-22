using ATMProject.Domain.Models;

namespace ATMProject.Application.Services.Interfaces;

public interface ILoanService
{
    void RequestLoan(int clientId, decimal amount);

    List<LoanRequest> GetLoansByClientId(int clientId);
    List<LoanRequest> GetAllLoanRequests();

    void ApproveLoan(int loanId);

    void RejectLoan(int loanId);
}