namespace Bet.Application.BaseExceptions;
public class LoginInvalidException : BetException
{
    public LoginInvalidException() : base("Login Invalido")
    {
    }
}

