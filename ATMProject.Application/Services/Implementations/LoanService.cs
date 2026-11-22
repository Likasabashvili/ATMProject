
using ATMProject.Application.Services.Interfaces;
using ATMProject.Domain.Enums;
using ATMProject.Domain.Interfaces;
using ATMProject.Domain.Models;

namespace ATMProject.Application.Services;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _loanRepository;
    private readonly IAccountService _accountService;

    public LoanService(
        ILoanRepository loanRepository,
        IAccountService accountService)
    {
        _loanRepository = loanRepository;
        _accountService = accountService;
    }
    
    public void RequestLoan(int clientId, decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Loan amount must be greater than zero!");

        var loans = _loanRepository.GetAllLoanRequests();

        bool hasPendingRequest = loans.Any(x =>
            x.ClientId == clientId &&
            x.Status == LoanStatus.Pending);

        if (hasPendingRequest)
            throw new InvalidOperationException(
                "You already have a pending loan request!");

        int newId = loans.Count == 0
            ? 1
            : loans.Max(x => x.Id) + 1;

        LoanRequest request = new LoanRequest
        {
            Id = newId,
            ClientId = clientId,
            Amount = amount,
            Status = LoanStatus.Pending,
            CreatedAt = DateTime.Now,
            ReviewedAt = null
        };

        _loanRepository.SaveLoanRequest(request);
    }
    
    public List<LoanRequest> GetLoansByClientId(int clientId)
    {
        return _loanRepository.GetAllLoanRequests()
            .Where(x => x.ClientId == clientId)
            .OrderByDescending(x => x.CreatedAt)
            .ToList();
    }

   
    public List<LoanRequest> GetAllLoanRequests()
    {
        return _loanRepository.GetAllLoanRequests()
            .OrderByDescending(x => x.CreatedAt)
            .ToList();
    }


    public void ApproveLoan(int loanId)
    {
        var loan = _loanRepository.GetAllLoanRequests()
            .FirstOrDefault(x => x.Id == loanId);

        if (loan == null)
            throw new ArgumentException("Loan request not found!");

        if (loan.Status != LoanStatus.Pending)
            throw new InvalidOperationException(
                "This loan request has already been reviewed!");

        
        var account = _accountService.GetAccountByClientId(loan.ClientId);

       
        _accountService.Deposit(loan.ClientId, loan.Amount);

        loan.Status = LoanStatus.Approved;
        loan.ReviewedAt = DateTime.Now;

        _loanRepository.UpdateLoanRequest(loan);
    }
    
    public void RejectLoan(int loanId)
    {
        var loan = _loanRepository.GetAllLoanRequests()
            .FirstOrDefault(x => x.Id == loanId);

        if (loan == null)
            throw new ArgumentException("Loan request not found!");

        if (loan.Status != LoanStatus.Pending)
            throw new InvalidOperationException(
                "This loan request has already been reviewed!");

        loan.Status = LoanStatus.Rejected;
        loan.ReviewedAt = DateTime.Now;

        _loanRepository.UpdateLoanRequest(loan);
    }
}
