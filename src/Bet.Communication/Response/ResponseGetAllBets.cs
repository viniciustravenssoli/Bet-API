using Bet.Domain.Entities;

namespace Bet.Communication.Response;
public class ResponseGetAllBets
{
    public List<BetFromUser> BetFromUsers { get; }


    public ResponseGetAllBets(List<BetFromUser> betFromUsers)
    {
        BetFromUsers = betFromUsers;
    }
}
