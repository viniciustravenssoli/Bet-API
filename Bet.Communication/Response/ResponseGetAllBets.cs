using Bet.Domain.Entities;

namespace Bet.Communication.Response;
public class ResponseGetAllBets
{
    public List<UserBet> bets { get; set; }

    public ResponseGetAllBets(List<UserBet> userBets)
    {
        bets = userBets;
    }
}
