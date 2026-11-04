using ATMProject.Domain.Interfaces;

namespace ATMProject.Application.Services;
using ATMProject.Domain.Models;

public class ClientService
{
    private IClientRepository _clientRepository;
    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }
    
    public void RegisterClient(Client client)
    {
        if (client == null)
        {
            Console.WriteLine("Client cannot be null.");
            return;
        }

        if (client.Age < 0 || client.Age > 100)
        {
            Console.WriteLine("Age must be between 0 and 100.");
            return;
        }
        _clientRepository.SaveClient(client);
    }
    
}