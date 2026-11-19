using Spectre.Console;

namespace ATMProject.Menu;

public class ClientMenu
{
    public void ShowMenu()
    {
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
                        "Change Password",
                        "Logout"
                    ));

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

                case "Change Password":
                    // ChangePassword();
                    break;

                case "Logout":
                    AnsiConsole.MarkupLine("[yellow]Logged out successfully![/]");
                    return;
            }
        }
    }
}