namespace ATMProject.Domain.Exceptions;

public class InsufficientBalanceException : Exception
{
    public InsufficientBalanceException()
        : base("Insufficient Balance")
    {
    }

}