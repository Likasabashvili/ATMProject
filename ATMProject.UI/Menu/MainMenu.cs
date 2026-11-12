using ATMProject.Application.Services.Interfaces;
using ATMProject.Domain.Enums;
using ATMProject.Domain.Interfaces;
using Spectre.Console;
namespace ATMProject.Menu;


public class MainMenu
{
	public class Mainmenu
	{
		private readonly IUserService _service;


		public Mainmenu(IUserService service)
		{
			_service = service;

		}


		public void ShowMenu()
		{
			var choice = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("1. Register, 2. Login, 3. Verify, 0. Exit")
					.AddChoices("1. Register", "2. Login", "3. Verify", "0. Exit"));

			switch (choice)
			{
				case "1. Register":
					RegisterUser();
					break;
				case "2. Login":
					LoginUser();
					break;
				case "3. Verify":
					//
					break;
				case "0. Exit":
					Console.WriteLine("Exited");
					break;
				default:
					Console.WriteLine("Invalid choice. Please try again.");
					ShowMenu();
					break;
			}
		}

		public void RegisterUser()
		{
			Console.WriteLine("Enter UserName");
			string userName = Console.ReadLine();
			Console.WriteLine("Enter Email");
			string email = Console.ReadLine();


			var password = new TextPrompt<string>("Enter your [green]password[/]:").Secret();
			var passwordResult = AnsiConsole.Prompt(password);


			Console.WriteLine("Enter Role");
			UserRole role = Enum.Parse<UserRole>(Console.ReadLine());

			_service.RegisterUser(userName, email, passwordResult, role);

		}

		public void LoginUser() // dont allow login if not verified
		{
			Console.WriteLine("Enter Email");
			string email = Console.ReadLine();
			Console.WriteLine("Enter Password");
			string password = Console.ReadLine();

			var user = _service.LoginUser(email, password);




		}
	}
}