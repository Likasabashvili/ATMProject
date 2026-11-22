namespace ATMProject.Domain.Models;

public class Account
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string UserName { get; set; }
    public decimal Balance { get; set; } = 0;
    public bool LoanStatus { get; set; } = false;
    public decimal LoanAmount { get; set; } = 0;

    public override string ToString()
    {
        return $"{Id}|{ClientId}|{UserName}|{Balance}|{LoanStatus}|{LoanAmount}";
    }
}