namespace ATMProject.Domain.Interfaces;
using ATMProject.Domain.Models;

public interface IClientRepository
{
    void SaveClient(Client client);

    List<Client> GetAllClients();
}