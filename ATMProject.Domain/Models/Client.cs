namespace ATMProject.Domain.Models;

public class Client : User
{
    public int Age { get; set; }
    public Account Account { get; set; }
    
}