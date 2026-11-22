using ATMProject.Application.Services;
using ATMProject.Application.Services.Interfaces;
using ATMProject.Domain.Interfaces;
using ATMProject.Infrastructure;
using ATMProject.Menu;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using University.Application.Services.Implemetations;
using Spectre.Console;


namespace ATMProject;

class Program
{

    static void Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        IUserRepository iuserRepository = new UserRepository();
        IEmailService emailService = new EmailService();
        IUserService userService = new UserService(iuserRepository, emailService);
        MainMenu mainMenu = new MainMenu(userService, config);
        
        try
        {
            while (mainMenu.ShowMenu())
            {
            }
            
            
        }
        catch (Exception ex)
        {

            AnsiConsole.MarkupLine($"[red]Error message[/] {ex.Message} ");
        }
    }
}