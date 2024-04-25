namespace Bet.Communication.Response;
public class BetInfo
{
    public long BetId { get; set; }
    public string HomeTeamName { get; set; }
    public double TeamAOdd { get; set; }
    public string VisitorTeamName { get; set; }
    public double TeamBOdd { get; set; }
}
