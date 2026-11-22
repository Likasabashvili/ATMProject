
using ATMProject.Application.Services.Interfaces;
using ATMProject.Domain.Models;
using Spectre.Console;

namespace ATMProject.Menu;

public class AdminMenu
{
    private readonly IUserService _userService;
    private readonly IAccountService _accountService;
    private readonly ITransactionService _transactionService;
    private readonly ILoanService _loanService;

    public AdminMenu(
        IUserService userService,
        IAccountService accountService,
        ITransactionService transactionService,
        ILoanService loanService)
    {
        _userService = userService;
        _accountService = accountService;
        _transactionService = transactionService;
        _loanService = loanService;
    }

    public void ShowMenu()
    {
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[red]ADMIN MENU[/]")
                    .AddChoices(
                        "View All Clients",
                        "Search Client",
                        "View All Accounts",
                        "Manage Loan Requests",
                        "View All Transactions",
                        "Block Account",
                        "Unblock Account",
                        "Bank Statistics",
                        "Logout"
                    ));

            try
            {
                switch (choice)
                {
                    case "View All Clients":
                        ViewAllClients();
                        break;

                    case "Search Client":
                        SearchClient();
                        break;

                    case "View All Accounts":
                        ViewAllAccounts();
                        break;

                    case "Manage Loan Requests":
                        ManageLoanRequests();
                        break;

                    case "View All Transactions":
                        ViewAllTransactions();
                        break;

                    case "Block Account":
                        BlockAccount();
                        break;

                    case "Unblock Account":
                        UnblockAccount();
                        break;

                    case "Bank Statistics":
                        ShowStatistics();
                        break;

                    case "Logout":
                        AnsiConsole.MarkupLine(
                            "[yellow]Logged out successfully![/]");
                        return;
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error: {Markup.Escape(ex.Message)}[/]");
            }
        }
    }

    // 1. View All Clients
    private void ViewAllClients()
    {
        var clients = _userService.GetAllClients();

        if (clients.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No clients found.[/]");
            return;
        }

        var table = new Table();

        table.AddColumn("ID");
        table.AddColumn("Username");
        table.AddColumn("Email");
        table.AddColumn("Verified");

        foreach (var client in clients)
        {
            table.AddRow(
                client.Id.ToString(),
                Markup.Escape(client.Username),
                Markup.Escape(client.Email),
                client.isVerified ? "[green]Yes[/]" : "[red]No[/]"
            );
        }

        AnsiConsole.Write(table);
    }

    // 2. Search Client
    private void SearchClient()
    {
        int clientId = AnsiConsole.Ask<int>("Enter Client ID:");

        var client = _userService.GetClientById(clientId);

        var table = new Table();

        table.AddColumn("ID");
        table.AddColumn("Username");
        table.AddColumn("Email");
        table.AddColumn("Verified");

        table.AddRow(
            client.Id.ToString(),
            Markup.Escape(client.Username),
            Markup.Escape(client.Email),
            client.isVerified ? "[green]Yes[/]" : "[red]No[/]"
        );

        AnsiConsole.Write(table);
    }

    // 3. View All Accounts
    private void ViewAllAccounts()
    {
        var accounts = _accountService.GetAllAccounts();

        if (accounts.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No accounts found.[/]");
            return;
        }

        var table = new Table();

        table.AddColumn("Account ID");
        table.AddColumn("Client ID");
        table.AddColumn("Username");
        table.AddColumn("Balance");
        table.AddColumn("Blocked");

        foreach (var account in accounts)
        {
            table.AddRow(
                account.Id.ToString(),
                account.ClientId.ToString(),
                Markup.Escape(account.UserName),
                account.Balance.ToString("F2"),
                account.IsBlocked ? "[red]Yes[/]" : "[green]No[/]"
            );
        }

        AnsiConsole.Write(table);
    }

    // 4. Manage Loan Requests
    private void ManageLoanRequests()
    {
        var loans = _loanService.GetAllLoanRequests();

        if (loans.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No loan requests found.[/]");
            return;
        }

        var table = new Table();

        table.AddColumn("Loan ID");
        table.AddColumn("Client ID");
        table.AddColumn("Amount");
        table.AddColumn("Status");
        table.AddColumn("Created At");

        foreach (var loan in loans)
        {
            table.AddRow(
                loan.Id.ToString(),
                loan.ClientId.ToString(),
                loan.Amount.ToString("F2"),
                loan.Status.ToString(),
                loan.CreatedAt.ToString("g")
            );
        }

        AnsiConsole.Write(table);

        var pendingLoans = loans
            .Where(x => x.Status == ATMProject.Domain.Enums.LoanStatus.Pending)
            .ToList();

        if (pendingLoans.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]There are no pending loan requests.[/]");
            return;
        }

        int loanId = AnsiConsole.Ask<int>("Enter Loan ID to review:");

        var action = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose action:")
                .AddChoices("Approve", "Reject", "Back")
        );

        switch (action)
        {
            case "Approve":
                _loanService.ApproveLoan(loanId);
                AnsiConsole.MarkupLine("[green]Loan approved successfully![/]");
                break;

            case "Reject":
                _loanService.RejectLoan(loanId);
                AnsiConsole.MarkupLine("[red]Loan rejected![/]");
                break;

            case "Back":
                return;
        }
    }

    // 5. View All Transactions
    private void ViewAllTransactions()
    {
        var transactions = _transactionService.GetAllTransactions();

        if (transactions.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No transactions found.[/]");
            return;
        }

        var table = new Table();

        table.AddColumn("Transaction ID");
        table.AddColumn("Account ID");
        table.AddColumn("Type");
        table.AddColumn("Amount");
        table.AddColumn("Date");
        table.AddColumn("Description");

        foreach (var transaction in transactions)
        {
            table.AddRow(
                transaction.Id.ToString(),
                transaction.AccountId.ToString(),
                Markup.Escape(transaction.Type),
                transaction.Amount.ToString("F2"),
                transaction.CreatedAt.ToString("g"),
                Markup.Escape(transaction.Description)
            );
        }

        AnsiConsole.Write(table);
    }

    // 6. Block Account
    private void BlockAccount()
    {
        int clientId = AnsiConsole.Ask<int>("Enter Client ID:");

        _accountService.BlockAccount(clientId);

        AnsiConsole.MarkupLine("[red]Account blocked successfully![/]");
    }

    // 7. Unblock Account
    private void UnblockAccount()
    {
        int clientId = AnsiConsole.Ask<int>("Enter Client ID:");

        _accountService.UnblockAccount(clientId);

        AnsiConsole.MarkupLine("[green]Account unblocked successfully![/]");
    }

    // 8. Bank Statistics
    private void ShowStatistics()
    {
        var clients = _userService.GetAllClients();
        var accounts = _accountService.GetAllAccounts();
        var transactions = _transactionService.GetAllTransactions();
        var loans = _loanService.GetAllLoanRequests();

        decimal totalBalance = accounts.Sum(x => x.Balance);

        int pendingLoans = loans.Count(
            x => x.Status == ATMProject.Domain.Enums.LoanStatus.Pending);

        int approvedLoans = loans.Count(
            x => x.Status == ATMProject.Domain.Enums.LoanStatus.Approved);

        int rejectedLoans = loans.Count(
            x => x.Status == ATMProject.Domain.Enums.LoanStatus.Rejected);

        var table = new Table();

        table.AddColumn("Bank Statistics");
        table.AddColumn("Value");

        table.AddRow("Total Clients", clients.Count.ToString());
        table.AddRow("Total Accounts", accounts.Count.ToString());
        table.AddRow("Total Balance", totalBalance.ToString("F2"));
        table.AddRow("Total Transactions", transactions.Count.ToString());
        table.AddRow("Pending Loans", pendingLoans.ToString());
        table.AddRow("Approved Loans", approvedLoans.ToString());
        table.AddRow("Rejected Loans", rejectedLoans.ToString());

        AnsiConsole.Write(table);
    }
}
