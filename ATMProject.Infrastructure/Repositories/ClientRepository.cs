using ATMProject.Domain.Interfaces;

namespace ATMProject.Infrastructure;
using ATMProject.Domain.Models;

public class ClientRepository  : IClientRepository
{
    private readonly string _path = @"/Users/likasabashvili/Desktop/c#/ATMProject/ATMProject.Infrastructure/Data/Client.txt";

    public void SaveClient(Client client)
    {
        File.AppendAllLines(_path, new List<string>{client.ToString()});
    }

    public List<Client> GetAllClients()
    {
        var ClientLines = File.ReadAllLines(_path).ToList();
        List<Client> clientList = new List<Client>();
        foreach (var line in ClientLines)
        {
            var parts = line.Split('|');
            Client client = new Client();
            
            client.Id = int.Parse(parts[0]);
            client.Username = parts[1];
            client.PasswordHash = parts[2];
            clientList.Add(client);
        }
        return clientList;
    }
}