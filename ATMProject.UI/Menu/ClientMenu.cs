using ATMProject.Application.Services.Interfaces;
using Spectre.Console;
using ATMProject.Domain.Models;

namespace ATMProject.Menu;

public class ClientMenu
{
    private readonly IAccountService _accountService;
    private readonly ITransactionService _transactionService;
    private readonly ILoanService _loanService;

    public ClientMenu(
        IAccountService accountService,
        ITransactionService transactionService,
        ILoanService loanService)
    {
        _accountService = accountService;
        _transactionService = transactionService;
        _loanService = loanService;
    }
    public void ShowMenu(User currentUser)
    {
        AnsiConsole.MarkupLine(
            $"[green]Welcome, {currentUser.Username}![/]"
        );
        
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]CLIENT MENU[/]")
                    .AddChoices(
                        "Check Balance",
                        "Deposit Money",
                        "Withdraw Money",
                        "Transfer Money",
                        "Transaction History",
                        "Request Loan",
                        "My Loans",
                        "Account Information",
                        "Logout"
                    ));
            try
            {

                switch (choice)
                {
                    case "Check Balance":
                        // CheckBalance();
                        break;

                    case "Deposit Money":
                        // Deposit();
                        break;

                    case "Withdraw Money":
                        // Withdraw();
                        break;

                    case "Transfer Money":
                        // Transfer();
                        break;

                    case "Transaction History":
                        // ShowHistory();
                        break;

                    case "Request Loan":
                        // RequestLoan();
                        break;

                    case "My Loans":
                        // ShowMyLoans();
                        break;

                    case "Account Information":
                        // ShowAccountInformation();
                        break;

                    case "Logout":
                        AnsiConsole.MarkupLine("[yellow]Logged out successfully![/]");
                        return;
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine(
                    $"[red]Error: {Markup.Escape(ex.Message)}[/]");
            }
            AnsiConsole.WriteLine();
        }
    }

    private void CheckBalance(int clientId)
    {
        var account = _accountService.GetAccountByClientId(clientId);
        AnsiConsole.MarkupLine("[green]Your current balance:[/]");
        AnsiConsole.Write(new Panel(
                $"[bold green]{account.Balance:N2} GEL[/]")
            .Header("Account Balance")
            .BorderColor(Color.Green));
    }

    private void Deposit(int clientId)
    {
        // decimal amount = AskAmount("Enter deposit amount:");
        //
        // _accountService.Deposit(clientId, amount);
        //
        // AnsiConsole.MarkupLine(
        //     $"[green]Successfully deposited {amount:N2} GEL![/]");
    }
}