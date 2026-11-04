namespace ATMProject.Domain.Exceptions;

public class InvalidCredentialsExeption: Exception
{
    public  InvalidCredentialsExeption() :
        base("Invalid Username or Password"){}
    
}