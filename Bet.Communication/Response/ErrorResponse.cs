namespace Bet.Communication.Response;
public class ErrorResponse
{
    public List<string> Messages { get; set; }

    public ErrorResponse(string menssage)
    {
        Messages = new List<string>
        {
            menssage
        };
    }

    public ErrorResponse(List<string> messages)
    {
        Messages = messages;
    }
}
