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
        var UserLines = File.ReadAllLines(_path).ToList();
        List<User> clientList = new List<User>();
        foreach (var line in UserLines)
        {
            var parts = line.Split('|');
            User client = new User();
            
            client.Id = int.Parse(parts[0]);
            client.Username = parts[1];
            client.PasswordHash = parts[2];
            clientList.Add(client);
        }
        return clientList;
    }

    public User GetUserByEmail(string email)
    {
        var userLines = File.ReadAllLines(_path).ToList();
        foreach (var line in userLines)
        {
            var parts = line.Split('|');
            if (parts[2] == email)
            {
                User user = new User();
                user.Id = int.Parse(parts[0]);
                user.Username = parts[1];
                user.Email = parts[2];
                user.PasswordHash = parts[3];
                user.Role = Enum.Parse<UserRole>(parts[4]);
                user.VerificationCode = parts[5];
                user.isVerified = bool.Parse(parts[6]);
                
                return user;
            }
        }
        return null;
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