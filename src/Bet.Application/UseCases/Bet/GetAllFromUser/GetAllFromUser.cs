
using Bet.Application.Services.LoggedUser;
using Bet.Communication.Response;
using Bet.Domain.Entities;
using Bet.Domain.Repository.UserBet;

namespace Bet.Application.UseCases.Bet.GetAllFromUser;
public class GetAllFromUser : IGetAllFromUser
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserBetWriteOnlyRepository _userBetWriteOnlyRepository;

    public GetAllFromUser(ILoggedUser loggedUser, IUserBetWriteOnlyRepository userBetWriteOnlyRepository)
    {
        _loggedUser = loggedUser;
        _userBetWriteOnlyRepository = userBetWriteOnlyRepository;
    }

    public async Task<ResponseGetAllBets> Execute(int page, int PageSize)
    {
        var user = await _loggedUser.GetUser();

        var betsFromUser = await _userBetWriteOnlyRepository.GetUserBetsWithPaginationAsync(user.Id, page, PageSize);

        var response = new ResponseGetAllBets(ConvertToBetFromUserList(betsFromUser));

        return response;

    }

    private List<BetFromUser> ConvertToBetFromUserList(List<UserBet> userBets)
    {
        var betsFromUser = new List<BetFromUser>();

        foreach (var userBet in userBets)
        {
            var betFromUser = new BetFromUser
            {
                Odd = userBet.Odd,
                BetId = userBet.BetId,
                BetAmount = userBet.BetAmount,
                CreatedAt = userBet.CreatedAt,
            };

            betsFromUser.Add(betFromUser);
        }

        return betsFromUser;
    }
}
