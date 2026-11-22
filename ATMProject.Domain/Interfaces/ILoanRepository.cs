using ATMProject.Domain.Models;

namespace ATMProject.Domain.Interfaces;

public interface ILoanRepository
{
    List<LoanRequest> GetAllLoanRequests();

    void SaveLoanRequest(LoanRequest request);

    void UpdateLoanRequest(LoanRequest request);
}