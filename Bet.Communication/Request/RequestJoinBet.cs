using Bet.Domain.Entities;

namespace Bet.Communication.Request;
public class RequestJoinBet
{
    public Team Chose {  get; set; }
    public double BetAmount { get; set; }
    public long BetId { get; set; } 
}
