using ATMProject.Domain.Enums;

namespace ATMProject.Domain.Models;

public class LoanRequest
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public decimal Amount { get; set; }
    public LoanStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
}