using ATMProject.Application.Services.Interfaces;
using ATMProject.Domain.Enums;
using ATMProject.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ATMProject.Application.Services;
using ATMProject.Domain.Models;

public class UserService : IUserService
{

    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly string _configuration;

    public UserService(IUserRepository userRepository, IEmailService emailService)
    {
        _userRepository = userRepository;
        _emailService = emailService;
    }

    public void RegisterUser(string username, string email, string password, UserRole role)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Username and Email are required!");
        }
        email = email.Trim();
        if (string.IsNullOrWhiteSpace(email) || !new EmailAddressAttribute().IsValid(email))
        {
            throw new ArgumentException("Invalid Email Address!Please enter a valid email.");
        }
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

        User user;

        if (role == UserRole.Admin)
        {
            user = new Admin
            {
                Id = maxId,
                Username = username,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                VerificationCode = code,
                isVerified = false,
                Role = role
            };
        }
        else
        {
            user = new User
            {
                Id = maxId,
                Username = username,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                VerificationCode = code,
                isVerified = false,
                Role = role
            };
        }
        _userRepository.SaveUser(user);
        _emailService.SendVerificationCode(email, code);
        if (role == UserRole.Admin)
        {
            Console.WriteLine($"Welcome Admin {user.Username}!");
        }
        else
        {
            Console.WriteLine($"User {user.Username} successfully registered");
        }
        
    }
    public void Verfy(string email, string verificationCode)
    {
        User user = _userRepository.GetUserByEmail(email);

        if (user == null)
            throw new ArgumentException("User not found!");

        if (user.isVerified)
            throw new InvalidOperationException("Account is already verified!");

        if (user.VerificationCode != verificationCode)
            throw new ArgumentException("Invalid verification code!");

        user.isVerified = true;

        _userRepository.UpdateUser(user);

        Console.WriteLine("Account successfully verified!");
    }

    public User LoginUser(string email, string password)
    {
        var user = _userRepository.GetUserByEmail(email);

        if (user == null)
            throw new UnauthorizedAccessException("Invalid Email or Password");

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid Email or Password");

        if (!user.isVerified)
            throw new UnauthorizedAccessException("Please verify your account first!");

        return user;
    
    }
}