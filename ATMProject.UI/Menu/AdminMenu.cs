using ATMProject.Application.Services;
using ATMProject.Domain.Enums;
using Spectre.Console;
namespace ATMProject.Menu;

public class AdminMenu
{
    	private readonly UserService _userService;

		public AdminMenu(UserService userService)
		{
			_userService = userService;
		}

		// public void ShowMenu()
		// {
		// 	string choice = "";
		// 	while (choice !="0")
		// 	{
		// 		Console.WriteLine("1. Show All Clients");
		// 		Console.WriteLine("2. Show Client By Id");
		// 		Console.WriteLine("3. Register Client");
		// 		Console.WriteLine("4. Update Client");
		// 		Console.WriteLine("5. Delete All Clients");
		// 		Console.WriteLine("6. Delete Client By Id");
		// 		Console.WriteLine("0. Exit");
		// 		 choice = Console.ReadLine();
		// 		switch (choice)
		// 		{
		// 			case "1":
		// 				// 
		// 				break;
		// 			case "2":
		// 				//
		// 				break;
		// 			case "3":
		// 				//
		// 				break;
		// 			case "4":
		// 				//
		// 				break;
		// 			case "5":
		// 				//
		// 				break;
		// 			case "6":
		// 				//
		// 				break;
		// 			case "0":
		// 				Console.WriteLine("Exited");
		// 				return;
		// 			default:
		// 				Console.WriteLine("Invalid choice. Please try again.");
		// 				break;
		// 		}
		// 	}
		// }
		//

		
}