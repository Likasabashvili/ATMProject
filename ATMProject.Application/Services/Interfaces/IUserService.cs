namespace ATMProject.Application.Services.Interfaces;
using ATMProject.Domain.Enums;
using ATMProject.Domain.Models;
public interface IUserService

{
    void RegisterUser(string username, string email, string password, UserRole role);
    void Verfy(string email, string verificationCode);
    User LoginUser(string email, string password);
    List<User> GetAllClients();

    User GetClientById(int clientId);
}