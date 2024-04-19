namespace Bet.Communication.Request;
public class RequestChangeUserData
{
    public string NewEmail { get; set; }
    public string NewName { get; set; }
    public string NewPhone { get; set; }
    public int MaxLimitPerDay { get; set; }
}
