using Spectre.Console;

namespace ATMProject.Menu;

public static class UIHelper
{
    public static void ShowLogo()
    {
        AnsiConsole.Clear();

        var logo = new FigletText("BANK")
            .Centered()
            .Color(Color.Teal);

        AnsiConsole.Write(logo);

        AnsiConsole.MarkupLine(
            "[grey]YOUR PERSONAL BANKING SYSTEM[/]"
        );

        AnsiConsole.Write(
            new Rule("[deepskyblue1]━━━━━━━━━━━━━━━━━━━━[/]")
                .Centered()
        );
    }
}