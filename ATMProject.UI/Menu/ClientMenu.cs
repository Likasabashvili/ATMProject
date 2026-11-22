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
                        CheckBalance(currentUser.Id);
                        break;

                    case "Deposit Money":
                        Deposit(currentUser.Id);
                        break;

                    case "Withdraw Money":
                        Withdraw(currentUser.Id);
                        break;

                    case "Transfer Money":
                        Transfer(currentUser.Id);
                        break;

                    case "Transaction History":
                        ShowHistory(currentUser.Id);
                        break;

                    case "Request Loan":
                        RequestLoan(currentUser.Id);
                        break;

                    case "My Loans":
                      ShowMyLoans(currentUser.Id);
                        break;

                    case "Account Information":
                        ShowAccountInformation(currentUser.Id);
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
        decimal amount = AskAmount("Enter deposit amount:");

        var account = _accountService.GetAccountByClientId(clientId);

        _accountService.Deposit(clientId, amount);

        _transactionService.AddTransaction(
            account.Id,
            "Deposit",
            amount,
            "Money deposited"
        );

        AnsiConsole.MarkupLine(
            $"[green]Successfully deposited {amount:N2} GEL![/]");
    }
    
   
    private void Withdraw(int clientId)
    {
        decimal amount = AskAmount("Enter withdrawal amount:");

        var account = _accountService.GetAccountByClientId(clientId);

        _accountService.Withdraw(clientId, amount);

        _transactionService.AddTransaction(
            account.Id,
            "Withdraw",
            amount,
            "Money withdrawn"
        );

        AnsiConsole.MarkupLine(
            $"[green]Successfully withdrawn {amount:N2} GEL![/]");
    }

    
    private void Transfer(int senderId)
    {
        int receiverId = AnsiConsole.Prompt(
            new TextPrompt<int>("Enter receiver's Client ID:")
                .Validate(id => id > 0
                    ? ValidationResult.Success()
                    : ValidationResult.Error("[red]ID must be positive![/]")));

        decimal amount = AskAmount("Enter transfer amount:");

        _accountService.Transfer(senderId, receiverId, amount);

        AnsiConsole.MarkupLine(
            $"[green]Successfully transferred {amount:N2} GEL![/]");
    }
    private void ShowHistory(int clientId)
    {
        var account = _accountService.GetAccountByClientId(clientId);

        var transactions =
            _transactionService.GetTransactionsByAccountId(account.Id);

        if (transactions.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No transactions found.[/]");
            return;
        }

        var table = new Table();

        table.AddColumn("ID");
        table.AddColumn("Type");
        table.AddColumn("Amount");
        table.AddColumn("Date");
        table.AddColumn("Description");

        foreach (var transaction in transactions)
        {
            table.AddRow(
                transaction.Id.ToString(),
                Markup.Escape(transaction.Type),
                $"{transaction.Amount:N2} GEL",
                transaction.CreatedAt.ToString("dd.MM.yyyy HH:mm"),
                Markup.Escape(transaction.Description ?? "")
            );
        }

        AnsiConsole.Write(table);
    }
    private void RequestLoan(int clientId)
    {
        decimal amount = AskAmount("Enter requested loan amount:");

        _loanService.RequestLoan(clientId, amount);

        AnsiConsole.MarkupLine(
            "[green]Your loan request was submitted successfully![/]");
    }

    private void ShowMyLoans(int clientId)
    {
        var loans = _loanService.GetLoansByClientId(clientId);

        if (loans.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]You have no loan requests.[/]");
            return;
        }

        var table = new Table();

        table.AddColumn("Loan ID");
        table.AddColumn("Amount");
        table.AddColumn("Status");
        table.AddColumn("Created At");
        table.AddColumn("Reviewed At");

        foreach (var loan in loans)
        {
            table.AddRow(
                loan.Id.ToString(),
                $"{loan.Amount:N2} GEL",
                loan.Status.ToString(),
                loan.CreatedAt.ToString("dd.MM.yyyy"),
                loan.ReviewedAt?.ToString("dd.MM.yyyy") ?? "Not reviewed"
            );
        }

        AnsiConsole.Write(table);
    }

    private void ShowAccountInformation(int clientId)
    {
        var account = _accountService.GetAccountByClientId(clientId);

        var panel = new Panel(
            $"[bold]Account ID:[/] {account.Id}\n" +
            $"[bold]Client ID:[/] {account.ClientId}\n" +
            $"[bold]Username:[/] {Markup.Escape(account.UserName)}\n" +
            $"[bold]Balance:[/] {account.Balance:N2} GEL\n" +
            $"[bold]Loan Status:[/] {(account.LoanStatus ? "Active" : "Inactive")}\n" +
            $"[bold]Loan Amount:[/] {account.LoanAmount:N2} GEL"
        );

        panel.Header("Account Information");
        panel.BorderColor(Color.Blue);

        AnsiConsole.Write(panel);
    }

    private decimal AskAmount(string message)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<decimal>(message)
                .Validate(amount => amount > 0
                    ? ValidationResult.Success()
                    : ValidationResult.Error(
                        "[red]Amount must be greater than zero![/]")));
    }
}
