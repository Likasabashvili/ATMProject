
using ATMProject.Domain.Interfaces;
using ATMProject.Domain.Models;
using ATMProject.Domain.Enums;
namespace ATMProject.Infrastructure;

public class LoanRepository : ILoanRepository
{
    private readonly string _path =
        @"/Users/likasabashvili/Desktop/c#/ATMProject/ATMProject.Infrastructure/Data/Loan.txt";

    public LoanRepository()
    {
        string? directory = Path.GetDirectoryName(_path);

        if (directory != null)
            Directory.CreateDirectory(directory);

        if (!File.Exists(_path))
            File.Create(_path).Close();
    }
    
    public List<LoanRequest> GetAllLoanRequests()
    {
        List<LoanRequest> loans = new();

        var lines = File.ReadAllLines(_path);

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var parts = line.Split('|');

            LoanRequest loan = new LoanRequest
            {
                Id = int.Parse(parts[0]),
                ClientId = int.Parse(parts[1]),
                Amount = decimal.Parse(parts[2]),
                Status = Enum.Parse<LoanStatus>(parts[3]),
                CreatedAt = DateTime.Parse(parts[4]),
                ReviewedAt = string.IsNullOrWhiteSpace(parts[5])
                    ? null
                    : DateTime.Parse(parts[5])
            };

            loans.Add(loan);
        }

        return loans;
    }
    
    public void SaveLoanRequest(LoanRequest request)
    {
        File.AppendAllText(
            _path,
            request + Environment.NewLine
        );
    }
    
    public void UpdateLoanRequest(LoanRequest request)
    {
        var loans = GetAllLoanRequests();

        var existingLoan = loans.FirstOrDefault(x => x.Id == request.Id);

        if (existingLoan == null)
            throw new ArgumentException("Loan request not found!");

        existingLoan.ClientId = request.ClientId;
        existingLoan.Amount = request.Amount;
        existingLoan.Status = request.Status;
        existingLoan.CreatedAt = request.CreatedAt;
        existingLoan.ReviewedAt = request.ReviewedAt;

        File.WriteAllLines(_path, loans.Select(x => x.ToString()));
    }
}
