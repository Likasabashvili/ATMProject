
using ATMProject.Application.Services.Interfaces;
using ATMProject.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

namespace ATMProject.Menu;

public class MainMenu
{
    private readonly IConfiguration _configuration;
    private readonly IUserService _service;
    private readonly AdminMenu _adminMenu;
    private readonly ClientMenu _clientMenu;

    public MainMenu(
        IUserService service,
        IConfiguration configuration,
        IAccountService accountService,
        ITransactionService transactionService,
        ILoanService loanService)
    {
        _service = service;
        _configuration = configuration;

        _adminMenu = new AdminMenu(
            service,
            accountService,
            transactionService,
            loanService
        );

        _clientMenu = new ClientMenu(
            accountService,
            transactionService,
            loanService
        );
    }

   
    private void ShowHeader()
    {
        AnsiConsole.Clear();

        var logo = new FigletText("ATM BANK")
            .Centered()
            .Color(Color.Teal);

        AnsiConsole.Write(logo);

        AnsiConsole.MarkupLine(
            "[grey]YOUR PERSONAL BANKING SYSTEM[/]"
        );

        AnsiConsole.Write(
            new Rule("[teal]SECURE BANKING[/]")
                .RuleStyle(Color.Teal)
                .Centered()
        );

        AnsiConsole.WriteLine();
    }

  
    public bool ShowMenu()
    {
        ShowHeader();

        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[teal]Welcome![/] [white]Please select an option:[/]")
                .PageSize(5)
                .HighlightStyle(new Style(Color.Teal))
                .AddChoices(
                    "Register",
                    "Login",
                    "Verify Account",
                    "Exit"
                )
        );

        switch (choice)
        {
            case "Register":
                RegisterUser();
                return true;

            case "Login":
                LoginUser();
                return true;

            case "Verify Account":
                Verify();
                return true;

            case "Exit":
                AnsiConsole.Clear();

                AnsiConsole.MarkupLine(
                    "[teal]Thank you for choosing ATM BANK![/]"
                );

                AnsiConsole.MarkupLine(
                    "[grey]Have a wonderful day![/]"
                );

                return false;

            default:
                return true;
        }
    }
    
    public void RegisterUser()
    {
        ShowHeader();

        AnsiConsole.Write(
            new Rule("[teal]CREATE YOUR ACCOUNT[/]")
                .RuleStyle(Color.Teal)
        );

        string userName = AnsiConsole.Ask<string>(
            "[teal]Enter Username:[/]"
        );

        string email = AnsiConsole.Ask<string>(
            "[teal]Enter Email:[/]"
        );

        string password = AnsiConsole.Prompt(
            new TextPrompt<string>("[teal]Enter Password:[/]")
                .Secret()
        );

        var role = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[teal]Choose Your Role:[/]")
                .HighlightStyle(new Style(Color.Teal))
                .AddChoices("Admin", "Client")
        );

        UserRole selectedRole = role == "Admin"
            ? UserRole.Admin
            : UserRole.Client;

        if (selectedRole == UserRole.Admin)
        {
            string secretWord = AnsiConsole.Prompt(
                new TextPrompt<string>(
                    "[teal]Enter Admin Secret Word:[/]"
                ).Secret()
            );

            if (secretWord != _configuration["SecretWordForAdmin"])
            {
                AnsiConsole.MarkupLine(
                    "[red]Invalid Secret Word. Defaulting to Client role.[/]"
                );

                selectedRole = UserRole.Client;
            }
        }

        try
        {
            _service.RegisterUser(
                userName,
                email,
                password,
                selectedRole
            );

            AnsiConsole.MarkupLine(
                "[green]Registration completed successfully![/]"
            );
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine(
                $"[red]{Markup.Escape(ex.Message)}[/]"
            );
        }

        AnsiConsole.MarkupLine(
            "\n[grey]Press any key to continue...[/]"
        );

        Console.ReadKey(true);
    }

   
    public void LoginUser()
    {
        ShowHeader();

        AnsiConsole.Write(
            new Rule("[teal]LOGIN TO YOUR ACCOUNT[/]")
                .RuleStyle(Color.Teal)
        );

        string email = AnsiConsole.Ask<string>(
            "[teal]Enter Email:[/]"
        );

        string password = AnsiConsole.Prompt(
            new TextPrompt<string>("[teal]Enter Password:[/]")
                .Secret()
        );

        try
        {
            var user = _service.LoginUser(email, password);

            AnsiConsole.MarkupLine(
                $"\n[green]Welcome back, {Markup.Escape(user.Username)}![/]"
            );

            AnsiConsole.MarkupLine(
                "[grey]Loading your dashboard...[/]"
            );

            Thread.Sleep(700);

            if (user.Role == UserRole.Admin)
                _adminMenu.ShowMenu();

            else if (user.Role == UserRole.Client)
                _clientMenu.ShowMenu(user);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine(
                $"[red]{Markup.Escape(ex.Message)}[/]"
            );

            Console.ReadKey(true);
        }
    }
    
    public void Verify()
    {
        ShowHeader();

        AnsiConsole.Write(
            new Rule("[teal]EMAIL VERIFICATION[/]")
                .RuleStyle(Color.Teal)
        );

        string email = AnsiConsole.Ask<string>(
            "[teal]Enter Email:[/]"
        );

        string verificationCode = AnsiConsole.Ask<string>(
            "[teal]Enter Verification Code:[/]"
        );

        try
        {
            _service.Verfy(email, verificationCode);

            AnsiConsole.MarkupLine(
                "\n[green]Your account has been verified successfully![/]"
            );
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine(
                $"[red]{Markup.Escape(ex.Message)}[/]"
            );
        }

        Console.ReadKey(true);
    }
}
