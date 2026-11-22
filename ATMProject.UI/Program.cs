
using ATMProject.Application.Services;
using ATMProject.Application.Services.Interfaces;
using ATMProject.Application.Services.Implementations;
using ATMProject.Domain.Interfaces;
using ATMProject.Infrastructure;
using ATMProject.Menu;
using Microsoft.Extensions.Configuration;
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

        try
        {
          
            IUserRepository userRepository = new UserRepository();
            IEmailService emailService = new EmailService(config);

           
            IAccountRepository accountRepository = new AccountRepository();

            IAccountService accountService = new AccountService(
                accountRepository
            );

         
            IUserService userService = new UserService(
                userRepository,
                emailService,
                accountRepository
            );

        
            ITransactionRepository transactionRepository = new TransactionRepository();

            ITransactionService transactionService = new TransactionService(
                transactionRepository
            );

       
            ILoanRepository loanRepository = new LoanRepository();

            ILoanService loanService = new LoanService(
                loanRepository,
                accountService
            );

         
            MainMenu mainMenu = new MainMenu(
                userService,
                config,
                accountService,
                transactionService,
                loanService
            );

            while (mainMenu.ShowMenu())
            {
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message}");
        }
    }
}
