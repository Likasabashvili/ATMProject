namespace ATMProject.Domain.Models;

public class Account
{
    public  int Id { get; set; }
    public string AccountNumber { get; set; }
    public decimal Balance { get; private set; }
    public DateTime CreatedAt { get; set; }
}