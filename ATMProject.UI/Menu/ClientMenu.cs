
using ATMProject.Application.Services.Interfaces;
using Spectre.Console;
using ATMProject.Domain.Models;
using ATMProject.Domain.Enums;
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
    
    private void ShowHeader(User currentUser)
    {
        AnsiConsole.Clear();

        var title = new FigletText("ATM BANK")
            .Centered()
            .Color(Color.Teal);

        AnsiConsole.Write(title);

        AnsiConsole.MarkupLine(
            "[grey]PERSONAL BANKING DASHBOARD[/]"
        );

        AnsiConsole.Write(
            new Rule("[teal]CLIENT PORTAL[/]")
                .RuleStyle(Color.Teal)
                .Centered()
        );

        AnsiConsole.MarkupLine(
            $"\n[white]Welcome back, [/][teal bold]{Markup.Escape(currentUser.Username)}[/]"
        );

        try
        {
            var account = _accountService.GetAccountByClientId(currentUser.Id);

            var accountPanel = new Panel(
                new Markup(
                    $"[grey]ACCOUNT NUMBER[/]\n" +
                    $"[white bold]#{account.Id}[/]\n\n" +
                    $"[grey]AVAILABLE BALANCE[/]\n" +
                    $"[teal bold]₾ {account.Balance:N2}[/]\n\n" +
                    $"[grey]ACCOUNT STATUS[/]\n" +
                    $"{(account.IsBlocked ? "[red]BLOCKED[/]" : "[green]ACTIVE[/]")}"
                )
            )
            .Header("[teal]MY ACCOUNT[/]")
            .BorderColor(Color.Teal)
            .Padding(2, 1);

            AnsiConsole.Write(accountPanel);
        }
        catch
        {
            AnsiConsole.MarkupLine(
                "[yellow]Account information unavailable.[/]"
            );
        }

        AnsiConsole.WriteLine();
    }

    public void ShowMenu(User currentUser)
    {
        while (true)
        {
            ShowHeader(currentUser);

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[teal]What would you like to do?[/]")
                    .PageSize(10)
                    .HighlightStyle(new Style(Color.Teal))
                    .AddChoices(
                        "Check Balance",
                        "Deposit Money",
                        "Withdraw Money",
                        "Transfer Money",
                        "Transaction History",
                        "Request Loan",
                        "My Loans",
                        "Account Information",
                        "My Statistics",
                        "Logout"
                    )
            );
            try
            {
                AnsiConsole.Clear();

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
                    
                    case "My Statistics":
                        ShowStatistics(currentUser.Id);
                        break;

                    case "Logout":
                        AnsiConsole.MarkupLine(
                            "[teal]You have successfully logged out![/]"
                        );

                        return;
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine(
                    $"[red]Error: {Markup.Escape(ex.Message)}[/]"
                );
            }

            AnsiConsole.WriteLine();

            AnsiConsole.MarkupLine(
                "[grey]Press any key to return to the dashboard...[/]"
            );

            Console.ReadKey(true);
        }
    }

    
    private void CheckBalance(int clientId)
    {
        var account = _accountService.GetAccountByClientId(clientId);

        var panel = new Panel(
            new Markup(
                $"[grey]YOUR CURRENT BALANCE[/]\n\n" +
                $"[teal bold]₾ {account.Balance:N2}[/]"
            )
        )
        .Header("[teal]ACCOUNT BALANCE[/]")
        .BorderColor(Color.Teal)
        .Padding(2, 1);

        AnsiConsole.Write(panel);
    }

   
    private void Deposit(int clientId)
    {
        AnsiConsole.Write(
            new Rule("[teal]DEPOSIT MONEY[/]")
                .RuleStyle(Color.Teal)
        );

        decimal amount = AskAmount("[teal]Enter deposit amount (GEL):[/]");

        var account = _accountService.GetAccountByClientId(clientId);

        _accountService.Deposit(clientId, amount);

        _transactionService.AddTransaction(
            account.Id,
            "Deposit",
            amount,
            "Money deposited"
        );

        AnsiConsole.MarkupLine(
            $"\n[green]Successfully deposited ₾ {amount:N2}![/]"
        );

        AnsiConsole.MarkupLine(
            $"[grey]New balance: ₾ {_accountService.GetAccountByClientId(clientId).Balance:N2}[/]"
        );
    }

    
    private void Withdraw(int clientId)
    {
        AnsiConsole.Write(
            new Rule("[teal]WITHDRAW MONEY[/]")
                .RuleStyle(Color.Teal)
        );

        decimal amount = AskAmount("[teal]Enter withdrawal amount (GEL):[/]");

        var account = _accountService.GetAccountByClientId(clientId);

        _accountService.Withdraw(clientId, amount);

        _transactionService.AddTransaction(
            account.Id,
            "Withdraw",
            amount,
            "Money withdrawn"
        );

        AnsiConsole.MarkupLine(
            $"\n[green]Successfully withdrawn ₾ {amount:N2}![/]"
        );

        AnsiConsole.MarkupLine(
            $"[grey]Remaining balance: ₾ {_accountService.GetAccountByClientId(clientId).Balance:N2}[/]"
        );
    }

    
   
    private void Transfer(int senderId)
    {
        AnsiConsole.Write(
            new Rule("[teal]TRANSFER MONEY[/]")
                .RuleStyle(Color.Teal)
        );

        
        int receiverId = AnsiConsole.Prompt(
            new TextPrompt<int>("[teal]Enter receiver's Client ID:[/]")
                .Validate(id => id > 0
                    ? ValidationResult.Success()
                    : ValidationResult.Error("[red]ID must be positive![/]"))
        );


        decimal amount = AskAmount(
            "[teal]Enter transfer amount (GEL):[/]"
        );

        
        var senderAccount =
            _accountService.GetAccountByClientId(senderId);

        var receiverAccount =
            _accountService.GetAccountByClientId(receiverId);

       
        _accountService.Transfer(senderId, receiverId, amount);

       
        _transactionService.AddTransaction(
            senderAccount.Id,
            "Transfer Out",
            amount,
            $"Money transferred to Client ID: {receiverId}"
        );

      
        _transactionService.AddTransaction(
            receiverAccount.Id,
            "Transfer In",
            amount,
            $"Money received from Client ID: {senderId}"
        );

        AnsiConsole.MarkupLine(
            $"\n[green]Successfully transferred ₾ {amount:N2}![/]"
        );

        AnsiConsole.MarkupLine(
            $"[grey]Recipient: Client ID {receiverId}[/]"
        );

        AnsiConsole.MarkupLine(
            $"[grey]Remaining balance: ₾ {_accountService.GetAccountByClientId(senderId).Balance:N2}[/]"
        );
    }

    
    private void ShowHistory(int clientId)
    {
        AnsiConsole.Write(
            new Rule("[teal]TRANSACTION HISTORY[/]")
                .RuleStyle(Color.Teal)
        );

        var account = _accountService.GetAccountByClientId(clientId);

        var transactions =
            _transactionService.GetTransactionsByAccountId(account.Id);

        if (transactions.Count == 0)
        {
            AnsiConsole.MarkupLine(
                "[yellow]No transactions found.[/]"
            );

            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Teal)
            .Expand();

        table.AddColumn("[teal]ID[/]");
        table.AddColumn("[teal]Type[/]");
        table.AddColumn("[teal]Amount[/]");
        table.AddColumn("[teal]Date[/]");
        table.AddColumn("[teal]Description[/]");

        foreach (var transaction in transactions)
        {
            table.AddRow(
                transaction.Id.ToString(),
                Markup.Escape(transaction.Type),
                $"₾ {transaction.Amount:N2}",
                transaction.CreatedAt.ToString("dd.MM.yyyy HH:mm"),
                Markup.Escape(transaction.Description ?? "")
            );
        }

        AnsiConsole.Write(table);
    }

 
    private void RequestLoan(int clientId)
    {
        AnsiConsole.Write(
            new Rule("[teal]LOAN REQUEST[/]")
                .RuleStyle(Color.Teal)
        );

        decimal amount = AskAmount("[teal]Enter requested loan amount (GEL):[/]");

        _loanService.RequestLoan(clientId, amount);

        AnsiConsole.MarkupLine(
            "\n[green]Your loan request was submitted successfully![/]"
        );
    }

  
    private void ShowMyLoans(int clientId)
    {
        AnsiConsole.Write(
            new Rule("[teal]MY LOANS[/]")
                .RuleStyle(Color.Teal)
        );

        var loans = _loanService.GetLoansByClientId(clientId);

        if (loans.Count == 0)
        {
            AnsiConsole.MarkupLine(
                "[yellow]You have no loan requests.[/]"
            );

            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Teal)
            .Expand();

        table.AddColumn("[teal]Loan ID[/]");
        table.AddColumn("[teal]Amount[/]");
        table.AddColumn("[teal]Status[/]");
        table.AddColumn("[teal]Created At[/]");
        table.AddColumn("[teal]Reviewed At[/]");

        foreach (var loan in loans)
        {
            string status = loan.Status.ToString() switch
            {
                "Approved" => "[green]Approved[/]",
                "Rejected" => "[red]Rejected[/]",
                "Pending" => "[yellow]Pending[/]",
                _ => Markup.Escape(loan.Status.ToString())
            };

            table.AddRow(
                loan.Id.ToString(),
                $"₾ {loan.Amount:N2}",
                status,
                loan.CreatedAt.ToString("dd.MM.yyyy"),
                loan.ReviewedAt?.ToString("dd.MM.yyyy") ?? "Not reviewed"
            );
        }

        AnsiConsole.Write(table);
    }

   
    private void ShowAccountInformation(int clientId)
    {
        AnsiConsole.Write(
            new Rule("[teal]ACCOUNT INFORMATION[/]")
                .RuleStyle(Color.Teal)
        );

        var account = _accountService.GetAccountByClientId(clientId);

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Teal)
            .AddColumn("[teal]Property[/]")
            .AddColumn("[teal]Information[/]");

        table.AddRow("Account ID", account.Id.ToString());
        table.AddRow("Client ID", account.ClientId.ToString());
        table.AddRow("Username", Markup.Escape(account.UserName));
        table.AddRow("Balance", $"₾ {account.Balance:N2}");
        table.AddRow("Loan Status", account.LoanStatus ? "Active" : "Inactive");
        table.AddRow("Loan Amount", $"₾ {account.LoanAmount:N2}");
        table.AddRow("Account Status", account.IsBlocked ? "[red]Blocked[/]" : "[green]Active[/]");

        AnsiConsole.Write(table);
    }

   
    private decimal AskAmount(string message)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<decimal>(message)
                .Validate(amount => amount > 0
                    ? ValidationResult.Success()
                    : ValidationResult.Error(
                        "[red]Amount must be greater than zero![/]"
                    ))
        );
    }
    
    
private void ShowStatistics(int clientId)
{
    AnsiConsole.Write(
        new Rule("[teal]MY ACCOUNT STATISTICS[/]")
            .RuleStyle(Color.Teal)
    );

  
    var account = _accountService.GetAccountByClientId(clientId);

   
    var transactions = _transactionService
        .GetTransactionsByAccountId(account.Id);

   
    var loans = _loanService.GetLoansByClientId(clientId);

   
    decimal totalDeposited = transactions
        .Where(x => x.Type == "Deposit")
        .Sum(x => x.Amount);

    decimal totalWithdrawn = transactions
        .Where(x => x.Type == "Withdraw")
        .Sum(x => x.Amount);

    decimal totalSent = transactions
        .Where(x => x.Type == "Transfer Out")
        .Sum(x => x.Amount);

    decimal totalReceived = transactions
        .Where(x => x.Type == "Transfer In")
        .Sum(x => x.Amount);

    int totalTransactions = transactions.Count;

    int transfersSent = transactions
        .Count(x => x.Type == "Transfer Out");

    int transfersReceived = transactions
        .Count(x => x.Type == "Transfer In");

   
    int pendingLoans = loans.Count(x => x.Status == LoanStatus.Pending);
    int approvedLoans = loans.Count(x => x.Status == LoanStatus.Approved);
    int rejectedLoans = loans.Count(x => x.Status == LoanStatus.Rejected);

   
    string accountStatus = account.IsBlocked
        ? "[red]BLOCKED[/]"
        : "[green]ACTIVE[/]";

   
    var accountPanel = new Panel(
        new Markup(
            $"[grey]CURRENT BALANCE[/]\n" +
            $"[teal bold]₾ {account.Balance:N2}[/]\n\n" +
            $"[grey]ACCOUNT STATUS[/]\n" +
            $"{accountStatus}\n\n" +
            $"[grey]TOTAL TRANSACTIONS[/]\n" +
            $"[white bold]{totalTransactions}[/]"
        )
    )
    .Header("[teal]ACCOUNT OVERVIEW[/]")
    .BorderColor(Color.Teal)
    .Padding(2, 1);

    AnsiConsole.Write(accountPanel);

  
    var moneyTable = new Table()
        .Border(TableBorder.Rounded)
        .BorderColor(Color.Teal)
        .Expand();

    moneyTable.AddColumn("[teal]TRANSACTION TYPE[/]");
    moneyTable.AddColumn("[teal]TOTAL AMOUNT[/]");
    moneyTable.AddColumn("[teal]COUNT[/]");

    moneyTable.AddRow(
        "[green]Deposited[/]",
        $"₾ {totalDeposited:N2}",
        transactions.Count(x => x.Type == "Deposit").ToString()
    );

    moneyTable.AddRow(
        "[red]Withdrawn[/]",
        $"₾ {totalWithdrawn:N2}",
        transactions.Count(x => x.Type == "Withdraw").ToString()
    );

    moneyTable.AddRow(
        "[yellow]Money Sent[/]",
        $"₾ {totalSent:N2}",
        transfersSent.ToString()
    );

    moneyTable.AddRow(
        "[green]Money Received[/]",
        $"₾ {totalReceived:N2}",
        transfersReceived.ToString()
    );

    AnsiConsole.Write(moneyTable);

    
    var loanPanel = new Panel(
        new Markup(
            $"[yellow]Pending Loans:[/] {pendingLoans}\n" +
            $"[green]Approved Loans:[/] {approvedLoans}\n" +
            $"[red]Rejected Loans:[/] {rejectedLoans}\n\n" +
            $"[grey]TOTAL LOAN REQUESTS:[/] [white bold]{loans.Count}[/]"
        )
    )
    .Header("[teal]LOAN STATISTICS[/]")
    .BorderColor(Color.Teal)
    .Padding(2, 1);

    AnsiConsole.Write(loanPanel);
}

}
