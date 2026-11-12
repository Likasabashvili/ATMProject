namespace ATMProject.Domain.Exceptions;

public class LoanNotFoundException : Exception
{
    public  LoanNotFoundException() 
        : base("Loan request not found."){}
    
}