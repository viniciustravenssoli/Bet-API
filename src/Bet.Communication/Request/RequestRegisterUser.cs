namespace Bet.Communication.Request;
public class RequestRegisterUser
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public int MaxDailyBets { get; set; }
}
