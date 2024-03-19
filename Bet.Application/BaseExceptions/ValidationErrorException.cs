namespace Bet.Application.BaseExceptions;

public class ValidationErrorException : BetException
{
    public List<string> ErrorMessages { get; set; }
    public ValidationErrorException(List<string> errorMessages) : base(string.Empty)
    {
        ErrorMessages = errorMessages;
    }
}
