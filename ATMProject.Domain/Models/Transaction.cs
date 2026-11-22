namespace ATMProject.Domain.Models;

public class Transaction
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string Type { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Description { get; set; }

    public override string ToString()
    {
        return $"{Id}|{AccountId}|{Type}|{Amount}|{CreatedAt:O}|{Description}";
    }
}