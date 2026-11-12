namespace ATMProject.Domain.Exceptions;

public class UserNotFoundException : Exception
{
    public  UserNotFoundException() 
        : base("User not found"){}
    
}