using ATMProject.Application.Services.Interfaces;
using Spectre.Console;

namespace ATMProject.Menu;

public class AdminMenu
{
    private readonly IUserService _userService;

    public AdminMenu(IUserService userService)
    {
        _userService = userService;
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

            switch (choice)
            {
                case "View All Clients":
                    // ViewAllClients();
                    break;

                case "Search Client":
                    // SearchClient();
                    break;

                case "View All Accounts":
                    // ViewAllAccounts();
                    break;

                case "Manage Loan Requests":
                    // ManageLoanRequests();
                    break;

                case "View All Transactions":
                    // ViewAllTransactions();
                    break;

                case "Block Account":
                    // BlockAccount();
                    break;

                case "Unblock Account":
                    // UnblockAccount();
                    break;

                case "Bank Statistics":
                    // ShowStatistics();
                    break;

                case "Logout":
                    AnsiConsole.MarkupLine("[yellow]Logged out successfully![/]");
                    return;
            }
        }
    }
}