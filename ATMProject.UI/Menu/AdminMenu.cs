
using ATMProject.Application.Services.Interfaces;
using ATMProject.Domain.Models;
using ATMProject.Domain.Enums;
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
            ShowHeader();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[teal]ADMIN DASHBOARD[/]")
                    .HighlightStyle(new Style(Color.Teal))
                    .PageSize(10)
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

            if (choice == "Logout")
            {
                AnsiConsole.MarkupLine(
                    "\n[teal]You have successfully logged out.[/]");
                Thread.Sleep(700);
                return;
            }

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
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine(
                    $"\n[red]Error:[/] {Markup.Escape(ex.Message)}");
            }

            Pause();
        }
    }

   

    private void ShowHeader()
    {
        AnsiConsole.Clear();

        AnsiConsole.Write(
            new FigletText("ATM BANK")
                .Centered()
                .Color(Color.Teal));

        AnsiConsole.MarkupLine(
            "[grey]ADMINISTRATION PANEL[/]");

        AnsiConsole.Write(
            new Rule("[teal]BANK MANAGEMENT SYSTEM[/]")
                .RuleStyle(Color.Teal));

        AnsiConsole.WriteLine();
    }

    

    private void ShowSectionTitle(string title)
    {
        AnsiConsole.WriteLine();

        AnsiConsole.Write(
            new Rule($"[teal]{Markup.Escape(title)}[/]")
                .RuleStyle(Color.Teal));

        AnsiConsole.WriteLine();
    }

 

    private void ShowEmptyMessage(string message)
    {
        AnsiConsole.Write(
            new Panel(
                new Markup($"[grey]{Markup.Escape(message)}[/]"))
                .Border(BoxBorder.Rounded)
                .BorderColor(Color.Grey)
                .Padding(2, 1));
    }

    

    private int AskId(string message)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>($"[teal]{message}[/]")
                .Validate(id => id > 0
                    ? ValidationResult.Success()
                    : ValidationResult.Error(
                        "[red]ID must be greater than zero![/]")));
    }

    private void Pause()
    {
        AnsiConsole.MarkupLine(
            "\n[grey]Press any key to continue...[/]");

        Console.ReadKey(true);
    }

   

    private void ViewAllClients()
    {
        ShowSectionTitle("ALL REGISTERED CLIENTS");

        var clients = _userService.GetAllClients();

        if (clients.Count == 0)
        {
            ShowEmptyMessage("No clients have been registered yet.");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Teal)
            .Expand();

        table.AddColumn("[teal]CLIENT ID[/]");
        table.AddColumn("[teal]USERNAME[/]");
        table.AddColumn("[teal]EMAIL[/]");
        table.AddColumn("[teal]VERIFICATION[/]");

        foreach (var client in clients)
        {
            table.AddRow(
                client.Id.ToString(),
                Markup.Escape(client.Username),
                Markup.Escape(client.Email),
                client.isVerified
                    ? "[green]Verified[/]"
                    : "[yellow]Not Verified[/]"
            );
        }

        AnsiConsole.Write(table);

        AnsiConsole.MarkupLine(
            $"\n[grey]Total clients: [teal]{clients.Count}[/][/]");
    }

    

    private void SearchClient()
    {
        ShowSectionTitle("SEARCH CLIENT");

        int clientId = AskId("Enter Client ID:");

        var client = _userService.GetClientById(clientId);

        if (client == null)
        {
            ShowEmptyMessage("Client not found.");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Teal)
            .Expand();

        table.AddColumn("[teal]INFORMATION[/]");
        table.AddColumn("[teal]DETAILS[/]");

        table.AddRow("Client ID", client.Id.ToString());
        table.AddRow("Username", Markup.Escape(client.Username));
        table.AddRow("Email", Markup.Escape(client.Email));
        table.AddRow(
            "Verification",
            client.isVerified
                ? "[green]Verified[/]"
                : "[yellow]Not Verified[/]"
        );

        AnsiConsole.Write(
            new Panel(table)
                .Header("[teal]CLIENT PROFILE[/]")
                .Border(BoxBorder.Rounded)
                .BorderColor(Color.Teal)
                .Padding(1, 1));
    }

   

    private void ViewAllAccounts()
    {
        ShowSectionTitle("ALL BANK ACCOUNTS");

        var accounts = _accountService.GetAllAccounts();

        if (accounts.Count == 0)
        {
            ShowEmptyMessage("No accounts found.");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Teal)
            .Expand();

        table.AddColumn("[teal]ACCOUNT ID[/]");
        table.AddColumn("[teal]CLIENT ID[/]");
        table.AddColumn("[teal]USERNAME[/]");
        table.AddColumn("[teal]BALANCE[/]");
        table.AddColumn("[teal]STATUS[/]");

        foreach (var account in accounts)
        {
            table.AddRow(
                account.Id.ToString(),
                account.ClientId.ToString(),
                Markup.Escape(account.UserName),
                $"₾ {account.Balance:N2}",
                account.IsBlocked
                    ? "[red]Blocked[/]"
                    : "[green]Active[/]"
            );
        }

        AnsiConsole.Write(table);

        AnsiConsole.MarkupLine(
            $"\n[grey]Total accounts: [teal]{accounts.Count}[/][/]");
    }

   

    private void ManageLoanRequests()
    {
        ShowSectionTitle("LOAN REQUEST MANAGEMENT");

        var loans = _loanService.GetAllLoanRequests();

        if (loans.Count == 0)
        {
            ShowEmptyMessage("No loan requests found.");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Teal)
            .Expand();

        table.AddColumn("[teal]LOAN ID[/]");
        table.AddColumn("[teal]CLIENT ID[/]");
        table.AddColumn("[teal]AMOUNT[/]");
        table.AddColumn("[teal]STATUS[/]");
        table.AddColumn("[teal]CREATED AT[/]");

        foreach (var loan in loans)
        {
            string status = loan.Status switch
            {
                LoanStatus.Approved => "[green]Approved[/]",
                LoanStatus.Rejected => "[red]Rejected[/]",
                LoanStatus.Pending => "[yellow]Pending[/]",
                _ => Markup.Escape(loan.Status.ToString())
            };

            table.AddRow(
                loan.Id.ToString(),
                loan.ClientId.ToString(),
                $"₾ {loan.Amount:N2}",
                status,
                loan.CreatedAt.ToString("dd MMM yyyy, HH:mm")
            );
        }

        AnsiConsole.Write(table);

        var pendingLoans = loans
            .Where(x => x.Status == LoanStatus.Pending)
            .ToList();

        if (pendingLoans.Count == 0)
        {
            AnsiConsole.MarkupLine(
                "\n[yellow]There are no pending loan requests.[/]");
            return;
        }

        AnsiConsole.MarkupLine(
            $"\n[yellow]Pending requests: {pendingLoans.Count}[/]");

        int loanId = AskId("Enter Loan ID to review:");

        var selectedLoan = pendingLoans.FirstOrDefault(
            x => x.Id == loanId);

        if (selectedLoan == null)
        {
            AnsiConsole.MarkupLine(
                "[red]This loan request does not exist or has already been reviewed.[/]");
            return;
        }

        var action = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[teal]CHOOSE ACTION[/]")
                .HighlightStyle(new Style(Color.Teal))
                .AddChoices(
                    "Approve Loan",
                    "Reject Loan",
                    "Back"
                ));

        switch (action)
        {
            case "Approve Loan":
                if (AnsiConsole.Confirm(
                        "Are you sure you want to approve this loan?"))
                {
                    _loanService.ApproveLoan(loanId);

                    AnsiConsole.MarkupLine(
                        "\n[green]Loan approved successfully![/]");
                }
                break;

            case "Reject Loan":
                if (AnsiConsole.Confirm(
                        "Are you sure you want to reject this loan?"))
                {
                    _loanService.RejectLoan(loanId);

                    AnsiConsole.MarkupLine(
                        "\n[red]Loan rejected successfully![/]");
                }
                break;

            case "Back":
                return;
        }
    }

   

    private void ViewAllTransactions()
    {
        ShowSectionTitle("TRANSACTION HISTORY");

        var transactions = _transactionService.GetAllTransactions();

        if (transactions.Count == 0)
        {
            ShowEmptyMessage("No transactions found.");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Teal)
            .Expand();

        table.AddColumn("[teal]TRANSACTION ID[/]");
        table.AddColumn("[teal]ACCOUNT ID[/]");
        table.AddColumn("[teal]TYPE[/]");
        table.AddColumn("[teal]AMOUNT[/]");
        table.AddColumn("[teal]DATE[/]");
        table.AddColumn("[teal]DESCRIPTION[/]");

        foreach (var transaction in transactions)
        {
            table.AddRow(
                transaction.Id.ToString(),
                transaction.AccountId.ToString(),
                Markup.Escape(transaction.Type),
                $"₾ {transaction.Amount:N2}",
                transaction.CreatedAt.ToString("dd MMM yyyy, HH:mm"),
                Markup.Escape(transaction.Description ?? "")
            );
        }

        AnsiConsole.Write(table);

        AnsiConsole.MarkupLine(
            $"\n[grey]Total transactions: [teal]{transactions.Count}[/][/]");
    }

   

    private void BlockAccount()
    {
        ShowSectionTitle("BLOCK ACCOUNT");

        int clientId = AskId("Enter Client ID:");

        if (!AnsiConsole.Confirm(
                $"Are you sure you want to block client #{clientId}?"))
        {
            AnsiConsole.MarkupLine("[yellow]Action cancelled.[/]");
            return;
        }

        _accountService.BlockAccount(clientId);

        AnsiConsole.MarkupLine(
            "\n[red]Account blocked successfully![/]");
    }

   

    private void UnblockAccount()
    {
        ShowSectionTitle("UNBLOCK ACCOUNT");

        int clientId = AskId("Enter Client ID:");

        if (!AnsiConsole.Confirm(
                $"Are you sure you want to unblock client #{clientId}?"))
        {
            AnsiConsole.MarkupLine("[yellow]Action cancelled.[/]");
            return;
        }

        _accountService.UnblockAccount(clientId);

        AnsiConsole.MarkupLine(
            "\n[green]Account unblocked successfully![/]");
    }

   

    private void ShowStatistics()
    {
        ShowSectionTitle("BANK OVERVIEW & STATISTICS");

        var clients = _userService.GetAllClients();
        var accounts = _accountService.GetAllAccounts();
        var transactions = _transactionService.GetAllTransactions();
        var loans = _loanService.GetAllLoanRequests();

        decimal totalBalance = accounts.Sum(x => x.Balance);

        int activeAccounts = accounts.Count(x => !x.IsBlocked);
        int blockedAccounts = accounts.Count(x => x.IsBlocked);

        int pendingLoans = loans.Count(
            x => x.Status == LoanStatus.Pending);

        int approvedLoans = loans.Count(
            x => x.Status == LoanStatus.Approved);

        int rejectedLoans = loans.Count(
            x => x.Status == LoanStatus.Rejected);

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Teal)
            .Expand();

        table.AddColumn("[teal]BANK OVERVIEW[/]");
        table.AddColumn("[teal]VALUE[/]");

        table.AddRow("Total Clients", $"[white]{clients.Count}[/]");
        table.AddRow("Total Accounts", $"[white]{accounts.Count}[/]");
        table.AddRow("Active Accounts", $"[green]{activeAccounts}[/]");
        table.AddRow("Blocked Accounts", $"[red]{blockedAccounts}[/]");
        table.AddRow("Total Bank Balance", $"[teal]₾ {totalBalance:N2}[/]");
        table.AddRow("Total Transactions", $"[white]{transactions.Count}[/]");

        AnsiConsole.Write(
            new Panel(table)
                .Header("[teal]BANK PERFORMANCE[/]")
                .Border(BoxBorder.Rounded)
                .BorderColor(Color.Teal)
                .Padding(1, 1));

        AnsiConsole.WriteLine();

        var loanTable = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Teal)
            .Expand();

        loanTable.AddColumn("[teal]LOAN STATISTICS[/]");
        loanTable.AddColumn("[teal]COUNT[/]");

        loanTable.AddRow("Pending Loans", $"[yellow]{pendingLoans}[/]");
        loanTable.AddRow("Approved Loans", $"[green]{approvedLoans}[/]");
        loanTable.AddRow("Rejected Loans", $"[red]{rejectedLoans}[/]");

        AnsiConsole.Write(
            new Panel(loanTable)
                .Header("[teal]LOAN OVERVIEW[/]")
                .Border(BoxBorder.Rounded)
                .BorderColor(Color.Teal)
                .Padding(1, 1));
    }
}

