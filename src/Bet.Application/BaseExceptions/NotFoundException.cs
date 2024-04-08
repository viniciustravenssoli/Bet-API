namespace Bet.Application.BaseExceptions;
public class NotFoundException : BetException
{
    public NotFoundException(string message) : base(message) { }
}
