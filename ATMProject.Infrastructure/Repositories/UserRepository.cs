using ATMProject.Domain.Enums;
using ATMProject.Domain.Interfaces;

namespace ATMProject.Infrastructure;
using ATMProject.Domain.Models;

public class UserRepository  : IUserRepository
{
    private readonly string _path = @"/Users/likasabashvili/Desktop/c#/ATMProject/ATMProject.Infrastructure/Data/User.txt";

    public void SaveUser(User user)
    {
        File.AppendAllLines(_path, new List<string>{user.ToString()});
    }
    
    public List<User> GetAllUsers()
    {
        var userLines = File.ReadAllLines(_path);
        List<User> users = new();

        foreach (var line in userLines)
        {
            var parts = line.Split('|');

            User user = new User
            {
                Id = int.Parse(parts[0]),
                Username = parts[1],
                Email = parts[2],
                PasswordHash = parts[3],
                Role = Enum.Parse<UserRole>(parts[4]),
                VerificationCode = parts[5],
                isVerified = bool.Parse(parts[6])
            };

            users.Add(user);
        }

        return users;
    }
    public User GetUserByEmail(string email)
    {
        var users = GetAllUsers();

        return users.FirstOrDefault(x =>
            x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    public void DeleteAllUsers()
    {
        File.Delete(_path);
    }

    public void SaveUsers(List<string> users)
    {
        File.AppendAllLines(_path, users);
    }

    
    
}