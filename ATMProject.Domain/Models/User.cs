using ATMProject.Domain.Enums;

namespace ATMProject.Domain.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public UserRole Role { get; set; }
    


    public override string ToString()
    {
        return $"{Id} | {Username} | {PasswordHash}  | {Role}";
    }
}
