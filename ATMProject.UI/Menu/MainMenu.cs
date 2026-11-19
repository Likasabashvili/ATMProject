using ATMProject.Application.Services.Interfaces;
using ATMProject.Domain.Enums;
using ATMProject.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
namespace ATMProject.Menu;


public class MainMenu
{
		private readonly IConfiguration _configuration;
		private readonly IUserService _service;
		private readonly AdminMenu _adminMenu;
		private readonly ClientMenu _clientMenu;

		public MainMenu(IUserService service,  IConfiguration configuration)
		{
			_service = service;
			_configuration = configuration;

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
					Verify();
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


			var role = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("Choose Role")
					.AddChoices("Admin", "Client"));
			
			UserRole selectedRole = role == "Admin"
				? UserRole.Admin
				: UserRole.Client;
			
			if(selectedRole == UserRole.Admin)
			{
				string secretword = AnsiConsole.Prompt(
					new TextPrompt<string>("Enter Secret Word:").Secret()
				);
				if(secretword != _configuration["SecretWordForAdmin"])
				{
					Console.WriteLine("Invalid Secret Word. Defaulting to Client role.");
					selectedRole = UserRole.Client;
				}
			}

			_service.RegisterUser(userName, email, passwordResult, selectedRole);

		}

		public void LoginUser()
		{
			Console.WriteLine("Enter Email");
			string email = Console.ReadLine();

			var password = new TextPrompt<string>("Enter Password:").Secret();
			string passwordResult = AnsiConsole.Prompt(password);

			var user = _service.LoginUser(email, passwordResult);

			if (user.Role == UserRole.Admin)
			{
				_adminMenu.ShowMenu();
			}
			else if (user.Role == UserRole.Client)
			{
				_clientMenu.ShowMenu();
			}
		}
		
		public void Verify()
		{
			Console.WriteLine("Enter Email");
			string email = Console.ReadLine();
			Console.WriteLine("Enter Verification Code");
			string verificationCode = Console.ReadLine();
			_service.Verfy(email, verificationCode);
		}
	}
