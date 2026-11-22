namespace ATMProject.Domain.Models;

public class Admin : User
{
    public string AdminCode { get; set; }
    public override string ToString()
    {
        return $"{base.ToString()}|{AdminCode}";
    }
}