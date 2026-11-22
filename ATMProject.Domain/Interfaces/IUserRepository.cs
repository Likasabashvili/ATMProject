namespace ATMProject.Domain.Interfaces;
using ATMProject.Domain.Models;

public interface IUserRepository
{
    void SaveUser(User user);
    List<User> GetAllUsers();
    User GetUserByEmail(string email);
    void UpdateUser(User user);
}