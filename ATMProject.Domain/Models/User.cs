using ATMProject.Domain.Enums;

namespace ATMProject.Domain.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public UserRole Role { get; set; }
    public string VerificationCode { get; set; }
    public bool isVerified { get; set; } = false;
   
    


    public override string ToString()
    {
        return $"{Id}|{Username}|{Email}|{PasswordHash}|{Role}|{VerificationCode}|{isVerified}";
    }
}
