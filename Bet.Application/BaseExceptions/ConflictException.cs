namespace Bet.Application.BaseExceptions;
public class ConflictException : BetException
{
    public ConflictException(string message) : base(message)
    {
    }
}
