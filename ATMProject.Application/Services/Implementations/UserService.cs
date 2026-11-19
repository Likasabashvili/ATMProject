using ATMProject.Application.Services.Interfaces;
using ATMProject.Domain.Enums;
using ATMProject.Domain.Interfaces;

namespace ATMProject.Application.Services;
using ATMProject.Domain.Models;

public class UserService : IUserService
{

    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    public UserService(IUserRepository userRepository, IEmailService emailService)
    {
        _userRepository = userRepository;
        _emailService = emailService;
    }

    public void RegisterUser(string username, string email, string password, UserRole role)
    {
        List<User> users = _userRepository.GetAllUsers();

        if (users.Any(x => x.Email == email))
        {
            throw new ArgumentException("Email already in use");
        }

        int maxId = 0;
        if (users.Count > 0)
        {
            maxId = users.Max(x => x.Id);
            maxId++;
        }

        Random random = new();
        string code = random.Next(1000, 9999).ToString();

        var user = new User()
        {
            Id = maxId,
            Username = username,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            VerificationCode = code,
            isVerified = false,
            Role = role
        };

        _userRepository.SaveUser(user);
        //send vercode
    }

    public void Verfy(string email, string verificationCode)
    {
        List<string> newUserList = new List<string>();
        _userRepository.DeleteAllUsers();
        _userRepository.SaveUsers(newUserList);
    }

    public User LoginUser(string email, string password)
    {
        var user = _userRepository.GetUserByEmail(email);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid Email or Password");
        }

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid Email or Password");
        }

        Console.WriteLine("Login Successful!");
        return user;
    }
}
        
        
    


