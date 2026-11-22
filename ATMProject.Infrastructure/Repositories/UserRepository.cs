using ATMProject.Domain.Enums;
using ATMProject.Domain.Interfaces;

namespace ATMProject.Infrastructure;
using ATMProject.Domain.Models;

public class UserRepository  : IUserRepository
{
    private readonly string _Userpath = @"/Users/likasabashvili/Desktop/c#/ATMProject/ATMProject.Infrastructure/Data/User.txt";

    private readonly string _Adminpath =
        @"/Users/likasabashvili/Desktop/c#/ATMProject/ATMProject.Infrastructure/Data/Admin.txt";

    public UserRepository()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_Userpath)!);
        
        if (!File.Exists(_Userpath))
            File.Create(_Userpath).Dispose();

        if (!File.Exists(_Adminpath))
            File.Create(_Adminpath).Dispose();
    }
    public void SaveUser(User user)
    {
        string path = user.Role == UserRole.Admin
            ? _Adminpath
            : _Userpath;
        
        File.AppendAllText(path, user.ToString() + Environment.NewLine);
    }
    
    public List<User> GetAllUsers()
    {
        List<User> users = new();

        users.AddRange(ReadUsersFromFile(_Userpath));
        users.AddRange(ReadUsersFromFile(_Adminpath));

        return users;
    }
    private List<User> ReadUsersFromFile(string path)
    {
        List<User> users = new();

        if (!File.Exists(path))
            return users;

        var lines = File.ReadAllLines(path);

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var parts = line.Split('|');

            User user;

            if (Enum.Parse<UserRole>(parts[4].Trim()) == UserRole.Admin)
            {
                user = new Admin
                {
                    AdminCode = parts.Length > 7 ? parts[7].Trim() : ""
                };
            }
            else
            {
                user = new User();
            }

            user.Id = int.Parse(parts[0].Trim());
            user.Username = parts[1].Trim();
            user.Email = parts[2].Trim();
            user.PasswordHash = parts[3].Trim();
            user.Role = Enum.Parse<UserRole>(parts[4].Trim());
            user.VerificationCode = parts[5].Trim();
            user.isVerified = bool.Parse(parts[6].Trim());

            users.Add(user);
        }

        return users;
    }
    public User GetUserByEmail(string email)
    {
        var users = GetAllUsers();

        return users.FirstOrDefault(x =>
            string.Equals(
                x.Email,
                email.Trim(),
                StringComparison.OrdinalIgnoreCase));
    }

    public void UpdateUser(User user)
    {
        string path = user.Role == UserRole.Admin
            ? _Adminpath
            : _Userpath;

        var lines = File.ReadAllLines(path);
        bool found = false;

        for (int i = 0; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            var parts = lines[i].Split('|');

            if (int.Parse(parts[0].Trim()) == user.Id)
            {
                lines[i] = user.ToString();
                found = true;
                break;
            }
        }

        if (!found)
            throw new ArgumentException("User not found!");

        File.WriteAllLines(path, lines);
    }
}